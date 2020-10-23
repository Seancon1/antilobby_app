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
            this.components = new System.ComponentModel.Container();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.txtUrlStatus = new System.Windows.Forms.Label();
            this.lblLoginClientCancel = new System.Windows.Forms.Label();
            this.btnLoginClientLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.panLogin = new System.Windows.Forms.Panel();
            this.btnLoginClientRegister = new System.Windows.Forms.Button();
            this.lblLoginClientReturnToLogin = new System.Windows.Forms.Label();
            this.timerLoginClient = new System.Windows.Forms.Timer(this.components);
            this.panLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(12, 25);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(412, 255);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = new System.Uri("https://www.prestigecode.com/api2/register", System.UriKind.Absolute);
            this.webBrowser.Visible = false;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            // 
            // txtUrlStatus
            // 
            this.txtUrlStatus.AutoSize = true;
            this.txtUrlStatus.Location = new System.Drawing.Point(12, 9);
            this.txtUrlStatus.Name = "txtUrlStatus";
            this.txtUrlStatus.Size = new System.Drawing.Size(46, 13);
            this.txtUrlStatus.TabIndex = 1;
            this.txtUrlStatus.Text = "urlstatus";
            this.txtUrlStatus.Visible = false;
            this.txtUrlStatus.Click += new System.EventHandler(this.txtUrlStatus_Click);
            // 
            // lblLoginClientCancel
            // 
            this.lblLoginClientCancel.AutoSize = true;
            this.lblLoginClientCancel.Location = new System.Drawing.Point(198, 291);
            this.lblLoginClientCancel.Name = "lblLoginClientCancel";
            this.lblLoginClientCancel.Size = new System.Drawing.Size(40, 13);
            this.lblLoginClientCancel.TabIndex = 2;
            this.lblLoginClientCancel.Text = "Cancel";
            this.lblLoginClientCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoginClientCancel.Click += new System.EventHandler(this.lblLoginClientCancel_Click);
            // 
            // btnLoginClientLogin
            // 
            this.btnLoginClientLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoginClientLogin.Location = new System.Drawing.Point(142, 131);
            this.btnLoginClientLogin.Name = "btnLoginClientLogin";
            this.btnLoginClientLogin.Size = new System.Drawing.Size(137, 47);
            this.btnLoginClientLogin.TabIndex = 3;
            this.btnLoginClientLogin.Text = "Login";
            this.btnLoginClientLogin.UseVisualStyleBackColor = true;
            this.btnLoginClientLogin.Click += new System.EventHandler(this.btnLoginClientLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Email:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password:";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(128, 57);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(275, 31);
            this.txtEmail.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(128, 94);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(275, 31);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // panLogin
            // 
            this.panLogin.Controls.Add(this.btnLoginClientRegister);
            this.panLogin.Controls.Add(this.txtEmail);
            this.panLogin.Controls.Add(this.btnLoginClientLogin);
            this.panLogin.Controls.Add(this.label1);
            this.panLogin.Controls.Add(this.txtPassword);
            this.panLogin.Controls.Add(this.label2);
            this.panLogin.Location = new System.Drawing.Point(12, 25);
            this.panLogin.Name = "panLogin";
            this.panLogin.Size = new System.Drawing.Size(412, 255);
            this.panLogin.TabIndex = 8;
            // 
            // btnLoginClientRegister
            // 
            this.btnLoginClientRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoginClientRegister.Location = new System.Drawing.Point(142, 180);
            this.btnLoginClientRegister.Name = "btnLoginClientRegister";
            this.btnLoginClientRegister.Size = new System.Drawing.Size(137, 47);
            this.btnLoginClientRegister.TabIndex = 8;
            this.btnLoginClientRegister.Text = "Register";
            this.btnLoginClientRegister.UseVisualStyleBackColor = true;
            this.btnLoginClientRegister.Click += new System.EventHandler(this.btnLoginClientRegister_Click);
            // 
            // lblLoginClientReturnToLogin
            // 
            this.lblLoginClientReturnToLogin.AutoSize = true;
            this.lblLoginClientReturnToLogin.Location = new System.Drawing.Point(335, 291);
            this.lblLoginClientReturnToLogin.Name = "lblLoginClientReturnToLogin";
            this.lblLoginClientReturnToLogin.Size = new System.Drawing.Size(80, 13);
            this.lblLoginClientReturnToLogin.TabIndex = 9;
            this.lblLoginClientReturnToLogin.Text = "Return to Login";
            this.lblLoginClientReturnToLogin.Visible = false;
            this.lblLoginClientReturnToLogin.Click += new System.EventHandler(this.lblLoginClientReturnToLogin_Click);
            // 
            // timerLoginClient
            // 
            this.timerLoginClient.Enabled = true;
            this.timerLoginClient.Interval = 1000;
            this.timerLoginClient.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LoginClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 313);
            this.Controls.Add(this.lblLoginClientReturnToLogin);
            this.Controls.Add(this.panLogin);
            this.Controls.Add(this.lblLoginClientCancel);
            this.Controls.Add(this.txtUrlStatus);
            this.Controls.Add(this.webBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginClient";
            this.TopMost = true;
            this.panLogin.ResumeLayout(false);
            this.panLogin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Label txtUrlStatus;
        private System.Windows.Forms.Label lblLoginClientCancel;
        private System.Windows.Forms.Button btnLoginClientLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Panel panLogin;
        private System.Windows.Forms.Button btnLoginClientRegister;
        private System.Windows.Forms.Label lblLoginClientReturnToLogin;
        private System.Windows.Forms.Timer timerLoginClient;
    }
}