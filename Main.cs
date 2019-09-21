using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PassiveWallpaperF
{
    public partial class PassiveWallpaper : Form
    {
        public PassiveWallpaper()
        {
            InitializeComponent();
            int monId = 0;
            string msg = "";
            foreach (Screen screen in Screen.AllScreens)
            {
                string str = $"Monitor {monId}: {screen.Bounds.Width} x {screen.Bounds.Height} @ {screen.Bounds.X},{screen.Bounds.Y} Primary: {screen.Primary}\n";
                msg += str;
                monId++;
            }

            string currentWallpaper = new string('\0', 260);
            SystemParametersInfo(SPI_GETDESKWALLPAPER, currentWallpaper.Length, currentWallpaper, 0);
            currentWallpaper = currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0'));

            monitor_info.Text = msg;
            numericUpDown1.Maximum = monId - 1;

            if (!File.Exists(currentWallpaper))
            {
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



        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags fuFlags,
            uint uTimeout,
            out IntPtr lpdwResult);

        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);


        [Flags()]
        enum DeviceContextValues : uint
        {
            /// <summary>DCX_WINDOW: Returns a DC that corresponds to the window rectangle rather 
            /// than the client rectangle.</summary>
            Window = 0x00000001,
            /// <summary>DCX_CACHE: Returns a DC from the cache, rather than the OWNDC or CLASSDC 
            /// window. Essentially overrides CS_OWNDC and CS_CLASSDC.</summary>
            Cache = 0x00000002,
            /// <summary>DCX_NORESETATTRS: Does not reset the attributes of this DC to the 
            /// default attributes when this DC is released.</summary>
            NoResetAttrs = 0x00000004,
            /// <summary>DCX_CLIPCHILDREN: Excludes the visible regions of all child windows 
            /// below the window identified by hWnd.</summary>
            ClipChildren = 0x00000008,
            /// <summary>DCX_CLIPSIBLINGS: Excludes the visible regions of all sibling windows 
            /// above the window identified by hWnd.</summary>
            ClipSiblings = 0x00000010,
            /// <summary>DCX_PARENTCLIP: Uses the visible region of the parent window. The 
            /// parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is 
            /// set to the upper-left corner of the window identified by hWnd.</summary>
            ParentClip = 0x00000020,
            /// <summary>DCX_EXCLUDERGN: The clipping region identified by hrgnClip is excluded 
            /// from the visible region of the returned DC.</summary>
            ExcludeRgn = 0x00000040,
            /// <summary>DCX_INTERSECTRGN: The clipping region identified by hrgnClip is 
            /// intersected with the visible region of the returned DC.</summary>
            IntersectRgn = 0x00000080,
            /// <summary>DCX_EXCLUDEUPDATE: Unknown...Undocumented</summary>
            ExcludeUpdate = 0x00000100,
            /// <summary>DCX_INTERSECTUPDATE: Unknown...Undocumented</summary>
            IntersectUpdate = 0x00000200,
            /// <summary>DCX_LOCKWINDOWUPDATE: Allows drawing even if there is a LockWindowUpdate 
            /// call in effect that would otherwise exclude this window. Used for drawing during 
            /// tracking.</summary>
            LockWindowUpdate = 0x00000400,
            /// <summary>DCX_VALIDATE When specified with DCX_INTERSECTUPDATE, causes the DC to 
            /// be completely validated. Using this function with both DCX_INTERSECTUPDATE and 
            /// DCX_VALIDATE is identical to using the BeginPaint function.</summary>
            Validate = 0x00200000,
        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public Dictionary<int, BackfroundForm> Forms = new Dictionary<int, BackfroundForm>();

        public IntPtr GetHandle()
        {
            IntPtr progman = FindWindow("Progman", null);
            IntPtr result = IntPtr.Zero;
            SendMessageTimeout(progman,
                0x052C,
                new IntPtr(0),
                IntPtr.Zero,
                SendMessageTimeoutFlags.SMTO_NORMAL,
                1000,
                out result);

            IntPtr workerw = IntPtr.Zero;

            EnumWindows((tophandle, topparamhandle) =>
            {
                IntPtr p = FindWindowEx(tophandle,
                    IntPtr.Zero,
                    "SHELLDLL_DefView",
                    null);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = FindWindowEx(IntPtr.Zero,
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
                    gifPath = openFileDialog.FileName;
                    pictureBox1.ImageLocation = gifPath;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            }


            var form = new BackfroundForm();
            int monitorId = int.Parse(numericUpDown1.Value.ToString());
            if (Forms.TryGetValue(monitorId, out var currentForm))
            {
                currentForm.Form.Close();
                Forms.Remove(monitorId);
            }
            Forms.Add(monitorId, form);

            form.Form.FormBorderStyle = FormBorderStyle.None;
            form.Form.Text = "PassiveWallpaper";

            Screen selectedScreen = Screen.AllScreens[monitorId];

            form.Form.Load += (s, e2) =>
            {
                form.Form.Width = selectedScreen.Bounds.Width;
                form.Form.Height = selectedScreen.Bounds.Height;
                form.Form.Left = selectedScreen.Bounds.X;
                form.Form.Top = selectedScreen.Bounds.Y;

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
                    pic.BackColor = Color.Transparent;
                    pic.BackgroundImage = new Bitmap(GetCurrentWallpaperPath());
                }
                else
                {
                    pic.BackColor = Color.White;
                    pic.BackgroundImage = null;
                }

                form.Form.Controls.Add(pic);

                SetParent(form.Form.Handle, workerw);
            };

            form.Form.Closing += (s2, a2) => SetImage();
            form.Form.Show();
        }

        private void disable_button_Click(object sender, EventArgs e)
        {
            int monitorId = int.Parse(numericUpDown1.Value.ToString());
            if (Forms.TryGetValue(monitorId, out var form))
            {
                form.Form.Close();
                form.Form.Dispose();
                Forms.Remove(monitorId);
                SetImage();
            }

            RestoreMovedProcesses();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, SPIF fWinIni);

        [Flags()]
        public enum SPIF
        {

            None = 0x00,
            /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
            SPIF_UPDATEINIFILE = 0x01,
            /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
            SPIF_SENDCHANGE = 0x02
        }

        const int SPI_SETDESKWALLPAPER = 0x14;
        private const int SPI_GETDESKWALLPAPER = 0x73;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(UInt32 uAction, int uParam, string lpvParam, int fuWinIni);

        public string GetCurrentWallpaperPath()
        {
            string currentWallpaper = new string('\0', 260);
            SystemParametersInfo(SPI_GETDESKWALLPAPER, currentWallpaper.Length, currentWallpaper, 0);
            currentWallpaper = currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0'));
            return currentWallpaper;
        }

        public void SetImage()
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, GetCurrentWallpaperPath(), SPIF.SPIF_UPDATEINIFILE | SPIF.SPIF_SENDCHANGE);
        }

        private void disable_all_button_Click(object sender, EventArgs e)
        {
            disable_all();
        }

        public void disable_all(bool wallpaper = true)
        {
            foreach (var form in Forms)
            {
                form.Value.Form.Close();
            }
            if (MovedProcesses.Count > 0)
            {
                RestoreMovedProcesses();
            }

            if (wallpaper) SetImage();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (var form in Forms)
            {
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
            Visible = false;
            notifyIcon1.Visible = true;
            ShowInTaskbar = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
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
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            Visible = true;
            Activate();
        }

        // External window stuff.
        public void ListProcesses()
        {
            Process[] processes = Process.GetProcesses();
            Processes.Clear();
            var current = Process.GetCurrentProcess();
            foreach (Process p in processes)
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle != current.MainWindowTitle)
                {
                    Processes[p.Id] = p;
                }
            }

            processCombo.Items.Clear();
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

        public Dictionary<int, MovedProcess> MovedProcesses = new Dictionary<int, MovedProcess>();
        public class MovedProcess
        {
            public MovedProcess(IntPtr originalParent, Process proc)
            {
                OriginalParent = originalParent;
                Process = proc;
            }

            public IntPtr OriginalParent;
            public Process Process;
        }

        private void moveToDesktop_Click(object sender, EventArgs e)
        {
            var line = processCombo.SelectedItem;
            if (line == null) return;

            if (line is string val)
            {
                var id = val.Split(' ').First();
                int idValue = int.Parse(id);

                var process = Processes.FirstOrDefault(x => x.Key == idValue);
                if (process.Value == null)
                {
                    MessageBox.Show("Program with the given ID was not found.");
                    return;
                }

                if (MovedProcesses.ContainsKey(idValue))
                {
                    MessageBox.Show("Program is already below the desktop.");
                    return;
                }

                var originalParent = SetParent(process.Value.MainWindowHandle, GetHandle());
                belowProcesses.Items.Add(val);
                MovedProcesses.Add(idValue, new MovedProcess(originalParent, process.Value));
            }
        }

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void bringToFront(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }

            SetForegroundWindow(handle);
        }

        private void moveForeground_Click(object sender, EventArgs e)
        {
            var line = belowProcesses.SelectedItem;
            if (line == null) return;

            if (line is string val)
            {
                var id = val.Split(' ').First();
                int idValue = int.Parse(id);

                if (MovedProcesses.TryGetValue(idValue, out var processMatch))
                {
                    SetParent(processMatch.Process.MainWindowHandle, processMatch.OriginalParent);
                    bringToFront(processMatch.Process.MainWindowHandle);
                    belowProcesses.Items.Remove(val);
                    SetImage();
                    MovedProcesses.Remove(idValue);
                }
            }
        }

        public void RestoreMovedProcesses()
        {
            foreach (var pair in MovedProcesses)
            {
                SetParent(pair.Value.Process.MainWindowHandle, pair.Value.OriginalParent);
                bringToFront(pair.Value.Process.MainWindowHandle);         
            }

            MovedProcesses.Clear();
            belowProcesses.Items.Clear();
            SetImage();
        }

        private void PassiveWallpaper_Load(object sender, EventArgs e)
        {

        }
    }
}
