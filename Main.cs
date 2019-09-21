using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PassiveWallpaperF;

namespace PassiveWallpaperF
{
    public partial class PassiveWallpaper : Form
    {
        public PassiveWallpaper()
        {
            InitializeComponent();
            int monId = 0;
            string msg = "";
            //Fill out monitor info so the user knows which screen is selected when choosing a gif location
            foreach (Screen screen in Screen.AllScreens)
            {
                string str = $"Monitor {monId}: {screen.Bounds.Width} x {screen.Bounds.Height} @ {screen.Bounds.X},{screen.Bounds.Y} Primary: {screen.Primary}\n";
                msg += str;
                monId++;
            }

            //Attempt to fetch the current wallpaper path.
            string currentWallpaper = new string('\0', 260);
            Natives.SystemParametersInfo((uint)Natives.SPIWALL.GET, currentWallpaper.Length, currentWallpaper, 0);
            currentWallpaper = currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0'));

            monitor_info.Text = msg;
            numericUpDown1.Maximum = monId - 1;

            //Set closing events to remove spawned forms and moved windows.
            if (!File.Exists(currentWallpaper))
            {
                //If current wallpaper path is not set then warn the user.
                var message = $"Your currently set wallpaper {currentWallpaper} doesn't exist on file ie. you have moved it since it was set.\n" +
                              "It is recommended that you set your wallpaper prior to using this tool.\n" +
                              "This is because when disabling the overlays the program sets your wallpaper in order to \"refresh\" your desktop back to the original state.";
                MessageBox.Show(message);
                Closing += (a, b) => 
                    {
                        disable_all(false);
                        RestoreMovedProcesses();
                    };
            }
            else
            {
                Closing += (a, b) => {
                        disable_all();
                        RestoreMovedProcesses();
                    };
            }
        }


        public Dictionary<int, BackgroundForm> Forms = new Dictionary<int, BackgroundForm>();

        /// <summary>
        /// Gets the window handle for a window below the desktop icons
        /// Courtesy https://www.codeproject.com/Articles/856020/Draw-behind-Desktop-Icons-in-Windows
        /// 
        /// when you change the desktop wallpaper, a new WorkerW window between the WorkerW instance that holds the Desktop Icons (SysListView32) and the Desktop Manager is created.
        /// take the handle of that new WorkerW window in order to be able to draw behind the desktop icons, directly above the wallpaper!
        /// </summary>
        /// <returns></returns>
        public IntPtr GetHandle()
        {
            //Obtain Program Manager Handle
            IntPtr progman = Natives.FindWindow("Progman", null);
                        
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            IntPtr result = IntPtr.Zero;
            Natives.SendMessageTimeout(progman,
                0x052C,
                new IntPtr(0),
                IntPtr.Zero,
                Natives.SendMessageTimeoutFlags.SMTO_NORMAL,
                1000,
                out result);
            


            // Spy++ output
            // .....
            // 0x00010190 "" WorkerW
            //   ...
            //   0x000100EE "" SHELLDLL_DefView
            //     0x000100F0 "FolderView" SysListView32
            // 0x00100B8A "" WorkerW       <-- This is the WorkerW instance we are after!
            // 0x000100EC "Program Manager" Progman
            IntPtr workerw = IntPtr.Zero;

            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            Natives.EnumWindows((tophandle, topparamhandle) =>
            {
                IntPtr p = Natives.FindWindowEx(tophandle,
                    IntPtr.Zero,
                    "SHELLDLL_DefView",
                    null);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = Natives.FindWindowEx(IntPtr.Zero,
                        tophandle,
                        "WorkerW",
                        null);
                }

                return true;
            }, IntPtr.Zero);

            return workerw;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var workerw = GetHandle();

            string gifPath = "";

            //Select a gif file to be placed in the picture box.
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Gif files (*.gif)|*.gif",
                Title = "Open gif file",
                AutoUpgradeEnabled = false
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    //Set variables to match the data returned by the selected image
                    gifPath = openFileDialog.FileName;

                    //Set the value in the picturebox as a preview of the selected image
                    pictureBox1.ImageLocation = gifPath;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            }

            // Create a new background form
            var form = new BackgroundForm();

            //Fetch the selected monitor
            int monitorId = int.Parse(numericUpDown1.Value.ToString());

            //Attempt to remove previously allocated items from that monitor
            if (Forms.TryGetValue(monitorId, out var currentForm))
            {
                currentForm.Form.Close();
                Forms.Remove(monitorId);
            }
            Forms.Add(monitorId, form);

            // Set the background forum to be borderless
            form.Form.FormBorderStyle = FormBorderStyle.None;
            form.Form.Text = "PassiveWallpaper";

            Screen selectedScreen = Screen.AllScreens[monitorId];

            //On the loading of the background form set the new properties
            form.Form.Load += (s, e2) =>
            {
                //Ensure the form takes up the entire screen size of the selected monitor
                form.Form.Width = selectedScreen.Bounds.Width;
                form.Form.Height = selectedScreen.Bounds.Height;
                form.Form.Left = selectedScreen.Bounds.X;
                form.Form.Top = selectedScreen.Bounds.Y;

                //Create a new picturebox that fills the form and set it's image contents
                PictureBox pic = new PictureBox
                {
                    Width = form.Form.Width,
                    Height = form.Form.Height,
                    Left = 0,
                    Top = 0,
                    Name = "PictureBox",
                    ImageLocation = gifPath,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                if (transparent.Checked)
                {
                    //Set the image to be transparent backgrounded and set it's background image to be the current desktop background.
                    pic.BackColor = Color.Transparent;
                    pic.BackgroundImage = new Bitmap(GetCurrentWallpaperPath());
                }
                else
                {
                    pic.BackColor = Color.White;
                    pic.BackgroundImage = null;
                }

                //Add the picturebox to the form
                form.Form.Controls.Add(pic);

                //Ensure the parent of the new form is the WorkerW process that was spawned between the desktop wallpaper and icons
                Natives.SetParent(form.Form.Handle, workerw);
            };

            //Ensure that the closing event of the form is set in order to re-set the current desktop wallpaper and 'refresh' it, removing any artifacts
            form.Form.Closing += (s2, a2) => SetImage();

            //Finally display the new form to the user.
            form.Form.Show();
        }

        private void disable_button_Click(object sender, EventArgs e)
        {
            //Attempt to find a background window allocated to the selected monitor
            int monitorId = int.Parse(numericUpDown1.Value.ToString());
            if (Forms.TryGetValue(monitorId, out var form))
            {
                //Close and dispose of any content related to the form
                form.Form.Close();
                form.Form.Dispose();
                Forms.Remove(monitorId);

                //Set the image to the wallpaper again in order to remove leftover artifacts
                SetImage();
            }            
        }


        public string GetCurrentWallpaperPath()
        {
            //Attempts the return the currently set wallpaper path
            string currentWallpaper = new string('\0', 260);
            Natives.SystemParametersInfo((uint)Natives.SPIWALL.GET, currentWallpaper.Length, currentWallpaper, 0);
            currentWallpaper = currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0'));
            return currentWallpaper;
        }

        /// <summary>
        /// Sets the current wallpaper with the image at it's designated path. Can cause issues/black wallpaper if path incorrect/not set.
        /// </summary>
        public void SetImage()
        {
            Natives.SystemParametersInfo((uint)Natives.SPIWALL.SET, 0, GetCurrentWallpaperPath(), Natives.SPIF.SPIF_UPDATEINIFILE | Natives.SPIF.SPIF_SENDCHANGE);
        }

        private void disable_all_button_Click(object sender, EventArgs e)
        {
            disable_all();
        }

        public void disable_all(bool wallpaper = true)
        {
            //Iterate through forms and close them.
            foreach (var form in Forms)
            {
                form.Value.Form.Close();
            }
            Forms.Clear();
            if (MovedProcesses.Count > 0)
            {
                //Restore all process windows to above desktop icons.
                RestoreMovedProcesses();
            }

            //Attempt to refresh the wallpaper
            if (wallpaper) SetImage();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (var form in Forms)
            {
                //Reset images in each picturebox by removing and then setting it's image location quickly.
                //This is method of syncing the windows
                if (form.Value.Form.Controls.Find("PictureBox", false).FirstOrDefault() is PictureBox pic)
                {
                    var loc = pic.ImageLocation;
                    pic.ImageLocation = "";
                    pic.ImageLocation = loc;
                }
            }
        }

        private void transparent_CheckedChanged(object sender, EventArgs e)
        {
            //Iterate through each window and either set or remove transparent background settings
            foreach (var form in Forms)
            {
                if (form.Value.Form.Controls.Find("PictureBox", false).FirstOrDefault() is PictureBox pic)
                {
                    if (transparent.Checked)
                    {
                        pic.BackColor = Color.Transparent;
                        pic.BackgroundImage = new Bitmap(GetCurrentWallpaperPath());
                    }
                    else
                    {
                        pic.BackColor = Color.White;
                        pic.BackgroundImage = null;
                    }
                }
            }
        }

        private void minimise_to_tray_Click(object sender, EventArgs e)
        {
            //Places the program in the tray icons
            Visible = false;
            notifyIcon1.Visible = true;
            ShowInTaskbar = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Restores the program back to a normal window
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            Show();
        }

        private void minimise_to_tray_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //Restores the program back to a normal window
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            Visible = true;
            Activate();
        }

        // External window stuff.
        public void ListProcesses()
        {
            //Get all other parent window processes ie. those which are displayed to the user and not hidden.
            Process[] processes = Process.GetProcesses();
            Processes.Clear();
            var current = Process.GetCurrentProcess();
            foreach (Process p in processes)
            {
                //Ensure the window has a title and is not the current program
                if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle != current.MainWindowTitle)
                {
                    Processes[p.Id] = p;
                }
            }

            //Clear old processes from the combobox
            processCombo.Items.Clear();

            //Format each process and display the process id and it's title
            var windows = Processes.OrderBy(x => x.Key).Select(x => $"{x.Key} - {x.Value.MainWindowTitle}");
            foreach (var window in windows)
            {
                processCombo.Items.Add(window);
            }
        }

        public Dictionary<int, Process> Processes = new Dictionary<int, Process>();

        private void loadProc_Click(object sender, EventArgs e)
        {
            ListProcesses();
        }

        private void processCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        //Format: Process ID - process info.
        public Dictionary<int, MovedProcess> MovedProcesses = new Dictionary<int, MovedProcess>();
        public class MovedProcess
        {
            //This is an intermittent class used to hold information about processes moved below the desktop icons
            public MovedProcess(IntPtr originalParent, Process proc)
            {
                OriginalParent = originalParent;
                Process = proc;
            }

            //Store the original parent of the process so it can be restored later
            public IntPtr OriginalParent;
            public Process Process;
        }

        private void moveToDesktop_Click(object sender, EventArgs e)
        {
            var line = processCombo.SelectedItem;
            if (line == null) return;

            if (line is string val)
            {
                //Parse the combobox item for it's process id.
                var id = val.Split(' ').First();
                int idValue = int.Parse(id);

                var process = Processes.FirstOrDefault(x => x.Key == idValue);
                if (process.Value == null)
                {
                    //Show error if program was closed after being added to list but prior to being added to desktop.
                    MessageBox.Show("Program with the given ID was not found.");
                    return;
                }

                if (MovedProcesses.ContainsKey(idValue))
                {
                    //Ensure the process is not already on the desktop.
                    MessageBox.Show("Program is already below the desktop.");
                    return;
                }


                //Set the processes parent to be the workerW process
                var originalParent = Natives.SetParent(process.Value.MainWindowHandle, GetHandle());
                belowProcesses.Items.Add(val);
                MovedProcesses.Add(idValue, new MovedProcess(originalParent, process.Value));
            }
        }

        public static void bringToFront(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }

            Natives.SetForegroundWindow(handle);
        }

        private void moveForeground_Click(object sender, EventArgs e)
        {
            var line = belowProcesses.SelectedItem;
            if (line == null) return;

            if (line is string val)
            {
                //Parse for the process id.
                var id = val.Split(' ').First();
                int idValue = int.Parse(id);

                if (MovedProcesses.TryGetValue(idValue, out var processMatch))
                {
                    //Reset the process parent, moving it back above the desktop.
                    Natives.SetParent(processMatch.Process.MainWindowHandle, processMatch.OriginalParent);
                    //Focus the window
                    bringToFront(processMatch.Process.MainWindowHandle);

                    //Reset wallpaper and remove stored process info.
                    belowProcesses.Items.Remove(val);
                    SetImage();
                    MovedProcesses.Remove(idValue);
                }
            }
        }

        public void RestoreMovedProcesses()
        {
            //Iterate through each moved process and restore it.
            foreach (var pair in MovedProcesses)
            {
                Natives.SetParent(pair.Value.Process.MainWindowHandle, pair.Value.OriginalParent);
                bringToFront(pair.Value.Process.MainWindowHandle);         
            }

            //Clear stored info and reset wallpaper
            MovedProcesses.Clear();
            belowProcesses.Items.Clear();
            SetImage();
        }

        private void PassiveWallpaper_Load(object sender, EventArgs e)
        {

        }
    }
}
