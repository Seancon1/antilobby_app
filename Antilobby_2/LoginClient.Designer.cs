namespace Antilobby_2
{
    partial class LoginClient
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.txtUrlStatus = new System.Windows.Forms.Label();
            this.lblLoginClientCancel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(12, 25);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(412, 262);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = new System.Uri("https://www.prestigecode.com/projects/antilobby/clientlogin.php", System.UriKind.Absolute);
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            // 
            // txtUrlStatus
            // 
            this.txtUrlStatus.AutoSize = true;
            this.txtUrlStatus.Location = new System.Drawing.Point(12, 9);
            this.txtUrlStatus.Name = "txtUrlStatus";
            this.txtUrlStatus.Size = new System.Drawing.Size(46, 13);
            this.txtUrlStatus.TabIndex = 1;
            this.txtUrlStatus.Text = "urlstatus";
            this.txtUrlStatus.Click += new System.EventHandler(this.txtUrlStatus_Click);
            // 
            // lblLoginClientCancel
            // 
            this.lblLoginClientCancel.AutoSize = true;
            this.lblLoginClientCancel.Location = new System.Drawing.Point(195, 294);
            this.lblLoginClientCancel.Name = "lblLoginClientCancel";
            this.lblLoginClientCancel.Size = new System.Drawing.Size(40, 13);
            this.lblLoginClientCancel.TabIndex = 2;
            this.lblLoginClientCancel.Text = "Cancel";
            this.lblLoginClientCancel.Click += new System.EventHandler(this.lblLoginClientCancel_Click);
            // 
            // LoginClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 313);
            this.Controls.Add(this.lblLoginClientCancel);
            this.Controls.Add(this.txtUrlStatus);
            this.Controls.Add(this.webBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginClient";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Label txtUrlStatus;
        private System.Windows.Forms.Label lblLoginClientCancel;
    }
}