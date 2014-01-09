namespace XBMC_WebBrowser
{
    partial class FormMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.pnlNavBar = new System.Windows.Forms.Panel();
            this.txtNavigateURL = new ZBobb.AlphaBlendTextBox();
            this.loadingCircleNavigation = new MRG.Controls.UI.LoadingCircle();
            this.pictNavProtocol = new System.Windows.Forms.PictureBox();
            this.pnlNavBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictNavProtocol)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(0, 53);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(800, 543);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // timerMain
            // 
            this.timerMain.Enabled = true;
            this.timerMain.Interval = 20;
            this.timerMain.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // pnlNavBar
            // 
            this.pnlNavBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNavBar.Controls.Add(this.txtNavigateURL);
            this.pnlNavBar.Controls.Add(this.loadingCircleNavigation);
            this.pnlNavBar.Controls.Add(this.pictNavProtocol);
            this.pnlNavBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlNavBar.Location = new System.Drawing.Point(3, 3);
            this.pnlNavBar.Name = "pnlNavBar";
            this.pnlNavBar.Size = new System.Drawing.Size(785, 46);
            this.pnlNavBar.TabIndex = 37;
            // 
            // txtNavigateURL
            // 
            this.txtNavigateURL.BackAlpha = 0;
            this.txtNavigateURL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtNavigateURL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNavigateURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNavigateURL.Location = new System.Drawing.Point(44, 6);
            this.txtNavigateURL.Name = "txtNavigateURL";
            this.txtNavigateURL.Size = new System.Drawing.Size(445, 31);
            this.txtNavigateURL.TabIndex = 38;
            this.txtNavigateURL.Enter += new System.EventHandler(this.txtNavigateURL_Enter);
            this.txtNavigateURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNavigateURL_KeyPress);
            this.txtNavigateURL.Leave += new System.EventHandler(this.txtNavigateURL_Leave);
            // 
            // loadingCircleNavigation
            // 
            this.loadingCircleNavigation.Active = false;
            this.loadingCircleNavigation.BackColor = System.Drawing.Color.Transparent;
            this.loadingCircleNavigation.Color = System.Drawing.Color.DarkGray;
            this.loadingCircleNavigation.InnerCircleRadius = 11;
            this.loadingCircleNavigation.Location = new System.Drawing.Point(732, 6);
            this.loadingCircleNavigation.Name = "loadingCircleNavigation";
            this.loadingCircleNavigation.NumberSpoke = 12;
            this.loadingCircleNavigation.OuterCircleRadius = 14;
            this.loadingCircleNavigation.RotationSpeed = 100;
            this.loadingCircleNavigation.Size = new System.Drawing.Size(32, 32);
            this.loadingCircleNavigation.SpokeThickness = 5;
            this.loadingCircleNavigation.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            this.loadingCircleNavigation.TabIndex = 39;
            this.loadingCircleNavigation.Text = "Loading";
            // 
            // pictNavProtocol
            // 
            this.pictNavProtocol.BackColor = System.Drawing.Color.Transparent;
            this.pictNavProtocol.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictNavProtocol.Image = global::XBMC_WebBrowser.Properties.Resources.globe;
            this.pictNavProtocol.Location = new System.Drawing.Point(6, 6);
            this.pictNavProtocol.Name = "pictNavProtocol";
            this.pictNavProtocol.Size = new System.Drawing.Size(32, 32);
            this.pictNavProtocol.TabIndex = 38;
            this.pictNavProtocol.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::XBMC_WebBrowser.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pnlNavBar);
            this.Controls.Add(this.webBrowser1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XBMC Browser";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.pnlNavBar.ResumeLayout(false);
            this.pnlNavBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictNavProtocol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Panel pnlNavBar;
        private System.Windows.Forms.PictureBox pictNavProtocol;
        private MRG.Controls.UI.LoadingCircle loadingCircleNavigation;
        private ZBobb.AlphaBlendTextBox txtNavigateURL;
    }
}

