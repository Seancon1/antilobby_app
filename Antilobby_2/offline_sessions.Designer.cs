namespace Antilobby_2
{
    partial class offline_sessions
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox_OfflineSessions = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox_OfflineStorageDetail = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(224, 170);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(216, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Retry Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox_OfflineSessions
            // 
            this.listBox_OfflineSessions.FormattingEnabled = true;
            this.listBox_OfflineSessions.Location = new System.Drawing.Point(13, 26);
            this.listBox_OfflineSessions.Name = "listBox_OfflineSessions";
            this.listBox_OfflineSessions.ScrollAlwaysVisible = true;
            this.listBox_OfflineSessions.Size = new System.Drawing.Size(169, 134);
            this.listBox_OfflineSessions.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Saved Offline Storage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Details";
            // 
            // listBox_OfflineStorageDetail
            // 
            this.listBox_OfflineStorageDetail.FormattingEnabled = true;
            this.listBox_OfflineStorageDetail.Location = new System.Drawing.Point(188, 26);
            this.listBox_OfflineStorageDetail.Name = "listBox_OfflineStorageDetail";
            this.listBox_OfflineStorageDetail.Size = new System.Drawing.Size(243, 134);
            this.listBox_OfflineStorageDetail.TabIndex = 5;
            // 
            // offline_sessions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 198);
            this.Controls.Add(this.listBox_OfflineStorageDetail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_OfflineSessions);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "offline_sessions";
            this.Text = "offline_sessions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox_OfflineSessions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox_OfflineStorageDetail;
    }
}