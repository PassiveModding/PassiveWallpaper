namespace PassiveWallpaperF
{
    partial class PassiveWallpaper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PassiveWallpaper));
            this.enable_button = new System.Windows.Forms.Button();
            this.disable_button = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.monitor_info = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.disable_all_button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.transparent = new System.Windows.Forms.CheckBox();
            this.minimise_to_tray = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.processCombo = new System.Windows.Forms.ComboBox();
            this.loadProc = new System.Windows.Forms.Button();
            this.moveToDesktop = new System.Windows.Forms.Button();
            this.moveForeground = new System.Windows.Forms.Button();
            this.belowProcesses = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // enable_button
            // 
            this.enable_button.Location = new System.Drawing.Point(113, 19);
            this.enable_button.Name = "enable_button";
            this.enable_button.Size = new System.Drawing.Size(84, 23);
            this.enable_button.TabIndex = 0;
            this.enable_button.Text = "Enable";
            this.enable_button.UseVisualStyleBackColor = true;
            this.enable_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // disable_button
            // 
            this.disable_button.Location = new System.Drawing.Point(203, 19);
            this.disable_button.Name = "disable_button";
            this.disable_button.Size = new System.Drawing.Size(84, 23);
            this.disable_button.TabIndex = 1;
            this.disable_button.Text = "Disable";
            this.disable_button.UseVisualStyleBackColor = true;
            this.disable_button.Click += new System.EventHandler(this.disable_button_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(9, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(278, 155);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // monitor_info
            // 
            this.monitor_info.AutoSize = true;
            this.monitor_info.Location = new System.Drawing.Point(323, 9);
            this.monitor_info.Name = "monitor_info";
            this.monitor_info.Size = new System.Drawing.Size(63, 13);
            this.monitor_info.TabIndex = 8;
            this.monitor_info.Text = "Monitor Info";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(9, 19);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(98, 20);
            this.numericUpDown1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Selected Monitor";
            // 
            // disable_all_button
            // 
            this.disable_all_button.Location = new System.Drawing.Point(12, 292);
            this.disable_all_button.Name = "disable_all_button";
            this.disable_all_button.Size = new System.Drawing.Size(305, 23);
            this.disable_all_button.TabIndex = 11;
            this.disable_all_button.Text = "Disable All";
            this.disable_all_button.UseVisualStyleBackColor = true;
            this.disable_all_button.Click += new System.EventHandler(this.disable_all_button_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 206);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(278, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Sync All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // transparent
            // 
            this.transparent.AutoSize = true;
            this.transparent.Location = new System.Drawing.Point(9, 235);
            this.transparent.Name = "transparent";
            this.transparent.Size = new System.Drawing.Size(221, 17);
            this.transparent.TabIndex = 13;
            this.transparent.Text = "Transparent Background (may cause lag)";
            this.transparent.UseVisualStyleBackColor = true;
            this.transparent.CheckedChanged += new System.EventHandler(this.transparent_CheckedChanged);
            // 
            // minimise_to_tray
            // 
            this.minimise_to_tray.Location = new System.Drawing.Point(12, 321);
            this.minimise_to_tray.Name = "minimise_to_tray";
            this.minimise_to_tray.Size = new System.Drawing.Size(305, 23);
            this.minimise_to_tray.TabIndex = 14;
            this.minimise_to_tray.Text = "Minimise to tray";
            this.minimise_to_tray.UseVisualStyleBackColor = true;
            this.minimise_to_tray.Click += new System.EventHandler(this.minimise_to_tray_Click);
            this.minimise_to_tray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.minimise_to_tray_MouseClick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "PassiveWallpaper";
            this.notifyIcon1.BalloonTipTitle = "PassiveWallpaper";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "PassiveWallpaper";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // processCombo
            // 
            this.processCombo.FormattingEnabled = true;
            this.processCombo.Location = new System.Drawing.Point(6, 19);
            this.processCombo.Name = "processCombo";
            this.processCombo.Size = new System.Drawing.Size(288, 21);
            this.processCombo.TabIndex = 15;
            this.processCombo.SelectedIndexChanged += new System.EventHandler(this.processCombo_SelectedIndexChanged);
            // 
            // loadProc
            // 
            this.loadProc.Location = new System.Drawing.Point(6, 46);
            this.loadProc.Name = "loadProc";
            this.loadProc.Size = new System.Drawing.Size(288, 23);
            this.loadProc.TabIndex = 16;
            this.loadProc.Text = "Update";
            this.loadProc.UseVisualStyleBackColor = true;
            this.loadProc.Click += new System.EventHandler(this.loadProc_Click);
            // 
            // moveToDesktop
            // 
            this.moveToDesktop.Location = new System.Drawing.Point(6, 75);
            this.moveToDesktop.Name = "moveToDesktop";
            this.moveToDesktop.Size = new System.Drawing.Size(288, 23);
            this.moveToDesktop.TabIndex = 19;
            this.moveToDesktop.Text = "Move To Desktop";
            this.moveToDesktop.UseVisualStyleBackColor = true;
            this.moveToDesktop.Click += new System.EventHandler(this.moveToDesktop_Click);
            // 
            // moveForeground
            // 
            this.moveForeground.Location = new System.Drawing.Point(3, 144);
            this.moveForeground.Name = "moveForeground";
            this.moveForeground.Size = new System.Drawing.Size(288, 23);
            this.moveForeground.TabIndex = 21;
            this.moveForeground.Text = "Bring To Foreground";
            this.moveForeground.UseVisualStyleBackColor = true;
            this.moveForeground.Click += new System.EventHandler(this.moveForeground_Click);
            // 
            // belowProcesses
            // 
            this.belowProcesses.FormattingEnabled = true;
            this.belowProcesses.Location = new System.Drawing.Point(3, 117);
            this.belowProcesses.Name = "belowProcesses";
            this.belowProcesses.Size = new System.Drawing.Size(288, 21);
            this.belowProcesses.TabIndex = 20;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(305, 282);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.enable_button);
            this.tabPage1.Controls.Add(this.disable_button);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.transparent);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(297, 256);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Gifs";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.processCombo);
            this.tabPage2.Controls.Add(this.moveForeground);
            this.tabPage2.Controls.Add(this.loadProc);
            this.tabPage2.Controls.Add(this.belowProcesses);
            this.tabPage2.Controls.Add(this.moveToDesktop);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(297, 256);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Applications";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Moved Windows";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Current Windows";
            // 
            // PassiveWallpaper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 361);
            this.Controls.Add(this.monitor_info);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.minimise_to_tray);
            this.Controls.Add(this.disable_all_button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PassiveWallpaper";
            this.Text = "PassiveWallpaper";
            this.Load += new System.EventHandler(this.PassiveWallpaper_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button enable_button;
        private System.Windows.Forms.Button disable_button;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label monitor_info;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button disable_all_button;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox transparent;
        private System.Windows.Forms.Button minimise_to_tray;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ComboBox processCombo;
        private System.Windows.Forms.Button loadProc;
        private System.Windows.Forms.Button moveToDesktop;
        private System.Windows.Forms.Button moveForeground;
        private System.Windows.Forms.ComboBox belowProcesses;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}