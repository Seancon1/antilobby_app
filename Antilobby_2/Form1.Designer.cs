﻿namespace Antilobby_2
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.websiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.getToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mACAddresssaveToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPAddressclipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionValueclipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabAlerts = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSelectedProcessName = new System.Windows.Forms.Label();
            this.listProcesses = new System.Windows.Forms.ListBox();
            this.lblCurrentProcess = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lblMyInfoSessionID = new System.Windows.Forms.Label();
            this.tabPageMyAlerts = new System.Windows.Forms.TabPage();
            this.btnAlertRemove = new System.Windows.Forms.Button();
            this.listViewCurrentAlerts = new System.Windows.Forms.ListView();
            this.comboBoxAlertTime = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxSelectableProcessesAlert = new System.Windows.Forms.ComboBox();
            this.lblAlertProcessListTitle = new System.Windows.Forms.Label();
            this.btnAddAlert = new System.Windows.Forms.Button();
            this.lblActiveAlertsTitle = new System.Windows.Forms.Label();
            this.flowLayoutActiveAlerts = new System.Windows.Forms.FlowLayoutPanel();
            this.TimerMainUIComponents = new System.Windows.Forms.Timer(this.components);
            this.TimerProcesses = new System.Windows.Forms.Timer(this.components);
            this.lblTimeElapsed = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabAlerts.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPageMyAlerts.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.viewToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.getToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(529, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.startSessionToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem2.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click_1);
            // 
            // startSessionToolStripMenuItem
            // 
            this.startSessionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.startSessionToolStripMenuItem.Name = "startSessionToolStripMenuItem";
            this.startSessionToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.startSessionToolStripMenuItem.Text = "Session";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(110, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.websiteToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // websiteToolStripMenuItem
            // 
            this.websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
            this.websiteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.websiteToolStripMenuItem.Text = "Website";
            this.websiteToolStripMenuItem.Click += new System.EventHandler(this.websiteToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.toolStripMenuItem1.Text = "Alert Settings";
            // 
            // getToolStripMenuItem
            // 
            this.getToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mACAddresssaveToClipboardToolStripMenuItem,
            this.iPAddressclipboardToolStripMenuItem,
            this.sessionValueclipboardToolStripMenuItem});
            this.getToolStripMenuItem.Name = "getToolStripMenuItem";
            this.getToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.getToolStripMenuItem.Text = "Get";
            // 
            // mACAddresssaveToClipboardToolStripMenuItem
            // 
            this.mACAddresssaveToClipboardToolStripMenuItem.Name = "mACAddresssaveToClipboardToolStripMenuItem";
            this.mACAddresssaveToClipboardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.mACAddresssaveToClipboardToolStripMenuItem.Text = "MAC Address (clipboard)";
            this.mACAddresssaveToClipboardToolStripMenuItem.Click += new System.EventHandler(this.mACAddresssaveToClipboardToolStripMenuItem_Click);
            // 
            // iPAddressclipboardToolStripMenuItem
            // 
            this.iPAddressclipboardToolStripMenuItem.Name = "iPAddressclipboardToolStripMenuItem";
            this.iPAddressclipboardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.iPAddressclipboardToolStripMenuItem.Text = "IP Address (clipboard)";
            this.iPAddressclipboardToolStripMenuItem.Click += new System.EventHandler(this.iPAddressclipboardToolStripMenuItem_Click);
            // 
            // sessionValueclipboardToolStripMenuItem
            // 
            this.sessionValueclipboardToolStripMenuItem.Name = "sessionValueclipboardToolStripMenuItem";
            this.sessionValueclipboardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.sessionValueclipboardToolStripMenuItem.Text = "Session Value (clipboard)";
            this.sessionValueclipboardToolStripMenuItem.Click += new System.EventHandler(this.sessionValueclipboardToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusMain});
            this.statusStrip1.Location = new System.Drawing.Point(0, 308);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(529, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(275, 16);
            // 
            // toolStripStatusMain
            // 
            this.toolStripStatusMain.Name = "toolStripStatusMain";
            this.toolStripStatusMain.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusMain.Text = "...";
            // 
            // tabAlerts
            // 
            this.tabAlerts.Controls.Add(this.tabPage2);
            this.tabAlerts.Controls.Add(this.tabPage4);
            this.tabAlerts.Controls.Add(this.tabPageMyAlerts);
            this.tabAlerts.Location = new System.Drawing.Point(12, 36);
            this.tabAlerts.Name = "tabAlerts";
            this.tabAlerts.SelectedIndex = 0;
            this.tabAlerts.Size = new System.Drawing.Size(513, 265);
            this.tabAlerts.TabIndex = 13;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.listProcesses);
            this.tabPage2.Controls.Add(this.lblCurrentProcess);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(505, 239);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Processes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSelectedProcessName);
            this.groupBox1.Location = new System.Drawing.Point(379, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(109, 154);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // lblSelectedProcessName
            // 
            this.lblSelectedProcessName.AutoSize = true;
            this.lblSelectedProcessName.Location = new System.Drawing.Point(7, 20);
            this.lblSelectedProcessName.Name = "lblSelectedProcessName";
            this.lblSelectedProcessName.Size = new System.Drawing.Size(73, 13);
            this.lblSelectedProcessName.TabIndex = 0;
            this.lblSelectedProcessName.Text = "ProcessName";
            // 
            // listProcesses
            // 
            this.listProcesses.FormattingEnabled = true;
            this.listProcesses.Location = new System.Drawing.Point(6, 31);
            this.listProcesses.MultiColumn = true;
            this.listProcesses.Name = "listProcesses";
            this.listProcesses.Size = new System.Drawing.Size(367, 147);
            this.listProcesses.TabIndex = 1;
            this.listProcesses.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listProcesses_MouseClick);
            // 
            // lblCurrentProcess
            // 
            this.lblCurrentProcess.AutoSize = true;
            this.lblCurrentProcess.Location = new System.Drawing.Point(6, 15);
            this.lblCurrentProcess.Name = "lblCurrentProcess";
            this.lblCurrentProcess.Size = new System.Drawing.Size(96, 13);
            this.lblCurrentProcess.TabIndex = 0;
            this.lblCurrentProcess.Text = "Session Processes";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lblMyInfoSessionID);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(505, 239);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "My Information";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lblMyInfoSessionID
            // 
            this.lblMyInfoSessionID.AutoSize = true;
            this.lblMyInfoSessionID.Location = new System.Drawing.Point(29, 17);
            this.lblMyInfoSessionID.Name = "lblMyInfoSessionID";
            this.lblMyInfoSessionID.Size = new System.Drawing.Size(124, 13);
            this.lblMyInfoSessionID.TabIndex = 0;
            this.lblMyInfoSessionID.Text = "Session ID: 1234567890";
            // 
            // tabPageMyAlerts
            // 
            this.tabPageMyAlerts.Controls.Add(this.button1);
            this.tabPageMyAlerts.Controls.Add(this.btnAlertRemove);
            this.tabPageMyAlerts.Controls.Add(this.listViewCurrentAlerts);
            this.tabPageMyAlerts.Controls.Add(this.comboBoxAlertTime);
            this.tabPageMyAlerts.Controls.Add(this.label2);
            this.tabPageMyAlerts.Controls.Add(this.comboBoxSelectableProcessesAlert);
            this.tabPageMyAlerts.Controls.Add(this.lblAlertProcessListTitle);
            this.tabPageMyAlerts.Controls.Add(this.btnAddAlert);
            this.tabPageMyAlerts.Controls.Add(this.lblActiveAlertsTitle);
            this.tabPageMyAlerts.Controls.Add(this.flowLayoutActiveAlerts);
            this.tabPageMyAlerts.Location = new System.Drawing.Point(4, 22);
            this.tabPageMyAlerts.Name = "tabPageMyAlerts";
            this.tabPageMyAlerts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMyAlerts.Size = new System.Drawing.Size(505, 239);
            this.tabPageMyAlerts.TabIndex = 4;
            this.tabPageMyAlerts.Text = "Alerts";
            this.tabPageMyAlerts.UseVisualStyleBackColor = true;
            // 
            // btnAlertRemove
            // 
            this.btnAlertRemove.Enabled = false;
            this.btnAlertRemove.Location = new System.Drawing.Point(206, 207);
            this.btnAlertRemove.Name = "btnAlertRemove";
            this.btnAlertRemove.Size = new System.Drawing.Size(90, 23);
            this.btnAlertRemove.TabIndex = 8;
            this.btnAlertRemove.Text = "Remove";
            this.btnAlertRemove.UseVisualStyleBackColor = true;
            this.btnAlertRemove.Click += new System.EventHandler(this.btnAlertRemove_Click);
            // 
            // listViewCurrentAlerts
            // 
            this.listViewCurrentAlerts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewCurrentAlerts.FullRowSelect = true;
            this.listViewCurrentAlerts.GridLines = true;
            this.listViewCurrentAlerts.Location = new System.Drawing.Point(206, 33);
            this.listViewCurrentAlerts.MultiSelect = false;
            this.listViewCurrentAlerts.Name = "listViewCurrentAlerts";
            this.listViewCurrentAlerts.Size = new System.Drawing.Size(121, 168);
            this.listViewCurrentAlerts.TabIndex = 7;
            this.listViewCurrentAlerts.UseCompatibleStateImageBehavior = false;
            this.listViewCurrentAlerts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewCurrentAlerts_MouseClick);
            // 
            // comboBoxAlertTime
            // 
            this.comboBoxAlertTime.FormattingEnabled = true;
            this.comboBoxAlertTime.Location = new System.Drawing.Point(141, 33);
            this.comboBoxAlertTime.Name = "comboBoxAlertTime";
            this.comboBoxAlertTime.Size = new System.Drawing.Size(59, 21);
            this.comboBoxAlertTime.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Alert Limit:";
            // 
            // comboBoxSelectableProcessesAlert
            // 
            this.comboBoxSelectableProcessesAlert.FormattingEnabled = true;
            this.comboBoxSelectableProcessesAlert.Location = new System.Drawing.Point(7, 33);
            this.comboBoxSelectableProcessesAlert.Name = "comboBoxSelectableProcessesAlert";
            this.comboBoxSelectableProcessesAlert.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSelectableProcessesAlert.TabIndex = 4;
            this.comboBoxSelectableProcessesAlert.MouseHover += new System.EventHandler(this.comboBox1_MouseHover);
            // 
            // lblAlertProcessListTitle
            // 
            this.lblAlertProcessListTitle.AutoSize = true;
            this.lblAlertProcessListTitle.Location = new System.Drawing.Point(6, 14);
            this.lblAlertProcessListTitle.Name = "lblAlertProcessListTitle";
            this.lblAlertProcessListTitle.Size = new System.Drawing.Size(112, 13);
            this.lblAlertProcessListTitle.TabIndex = 3;
            this.lblAlertProcessListTitle.Text = "Selectable Processes:";
            // 
            // btnAddAlert
            // 
            this.btnAddAlert.Location = new System.Drawing.Point(43, 60);
            this.btnAddAlert.Name = "btnAddAlert";
            this.btnAddAlert.Size = new System.Drawing.Size(75, 23);
            this.btnAddAlert.TabIndex = 2;
            this.btnAddAlert.Text = "Add Alert ->";
            this.btnAddAlert.UseVisualStyleBackColor = true;
            this.btnAddAlert.Click += new System.EventHandler(this.btnAddAlert_Click);
            // 
            // lblActiveAlertsTitle
            // 
            this.lblActiveAlertsTitle.AutoSize = true;
            this.lblActiveAlertsTitle.Location = new System.Drawing.Point(330, 14);
            this.lblActiveAlertsTitle.Name = "lblActiveAlertsTitle";
            this.lblActiveAlertsTitle.Size = new System.Drawing.Size(98, 13);
            this.lblActiveAlertsTitle.TabIndex = 1;
            this.lblActiveAlertsTitle.Text = "Active Alert Timers:";
            // 
            // flowLayoutActiveAlerts
            // 
            this.flowLayoutActiveAlerts.AutoScroll = true;
            this.flowLayoutActiveAlerts.BackColor = System.Drawing.Color.SeaShell;
            this.flowLayoutActiveAlerts.Location = new System.Drawing.Point(333, 33);
            this.flowLayoutActiveAlerts.Name = "flowLayoutActiveAlerts";
            this.flowLayoutActiveAlerts.Size = new System.Drawing.Size(152, 197);
            this.flowLayoutActiveAlerts.TabIndex = 0;
            // 
            // TimerMainUIComponents
            // 
            this.TimerMainUIComponents.Enabled = true;
            this.TimerMainUIComponents.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TimerProcesses
            // 
            this.TimerProcesses.Enabled = true;
            this.TimerProcesses.Interval = 1000;
            this.TimerProcesses.Tick += new System.EventHandler(this.TimerProcesses_Tick);
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.AutoSize = true;
            this.lblTimeElapsed.Location = new System.Drawing.Point(390, 9);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new System.Drawing.Size(74, 13);
            this.lblTimeElapsed.TabIndex = 1;
            this.lblTimeElapsed.Text = "Time Elapsed:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 207);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Clear All Alerts";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 330);
            this.Controls.Add(this.lblTimeElapsed);
            this.Controls.Add(this.tabAlerts);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabAlerts.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPageMyAlerts.ResumeLayout(false);
            this.tabPageMyAlerts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem getToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mACAddresssaveToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPAddressclipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionValueclipboardToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMain;
        private System.Windows.Forms.TabControl tabAlerts;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSelectedProcessName;
        private System.Windows.Forms.ListBox listProcesses;
        private System.Windows.Forms.Label lblCurrentProcess;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ToolStripMenuItem startSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.Timer TimerMainUIComponents;
        private System.Windows.Forms.Label lblMyInfoSessionID;
        private System.Windows.Forms.Timer TimerProcesses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Label lblTimeElapsed;
        private System.Windows.Forms.TabPage tabPageMyAlerts;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutActiveAlerts;
        private System.Windows.Forms.ComboBox comboBoxAlertTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSelectableProcessesAlert;
        private System.Windows.Forms.Label lblAlertProcessListTitle;
        private System.Windows.Forms.Button btnAddAlert;
        private System.Windows.Forms.Label lblActiveAlertsTitle;
        private System.Windows.Forms.Button btnAlertRemove;
        private System.Windows.Forms.ListView listViewCurrentAlerts;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem websiteToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}

