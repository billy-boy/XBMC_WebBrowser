namespace XBMC_WebBrowser
{
    partial class FormShortcuts
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
            this._pictDown = new System.Windows.Forms.PictureBox();
            this._pictUp = new System.Windows.Forms.PictureBox();
            this._btn_AddCurrent = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._pictDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictUp)).BeginInit();
            this.SuspendLayout();
            // 
            // _pictDown
            // 
            this._pictDown.BackgroundImage = global::XBMC_WebBrowser.Properties.Resources.SubMenuBack_Footer2;
            this._pictDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._pictDown.Location = new System.Drawing.Point(0, 96);
            this._pictDown.Name = "_pictDown";
            this._pictDown.Size = new System.Drawing.Size(256, 32);
            this._pictDown.TabIndex = 7;
            this._pictDown.TabStop = false;
            // 
            // _pictUp
            // 
            this._pictUp.BackgroundImage = global::XBMC_WebBrowser.Properties.Resources.SubMenuBack_Header;
            this._pictUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._pictUp.Location = new System.Drawing.Point(0, 0);
            this._pictUp.Name = "_pictUp";
            this._pictUp.Size = new System.Drawing.Size(256, 32);
            this._pictUp.TabIndex = 6;
            this._pictUp.TabStop = false;
            // 
            // _btn_AddCurrent
            // 
            this._btn_AddCurrent.BackgroundImage = global::XBMC_WebBrowser.Properties.Resources.SubMenuBack_ButtonNoFocus;
            this._btn_AddCurrent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._btn_AddCurrent.FlatAppearance.BorderSize = 0;
            this._btn_AddCurrent.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this._btn_AddCurrent.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this._btn_AddCurrent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btn_AddCurrent.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btn_AddCurrent.ForeColor = System.Drawing.Color.White;
            this._btn_AddCurrent.Location = new System.Drawing.Point(0, 32);
            this._btn_AddCurrent.Name = "_btn_AddCurrent";
            this._btn_AddCurrent.Size = new System.Drawing.Size(256, 64);
            this._btn_AddCurrent.TabIndex = 8;
            this._btn_AddCurrent.Text = "+ Aktuelle";
            this._btn_AddCurrent.UseVisualStyleBackColor = true;
            this._btn_AddCurrent.Click += new System.EventHandler(this.button_Click);
            this._btn_AddCurrent.Enter += new System.EventHandler(this.button_Enter);
            this._btn_AddCurrent.Leave += new System.EventHandler(this.button_Leave);
            this._btn_AddCurrent.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this._btn_AddCurrent.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // FormShortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(256, 128);
            this.Controls.Add(this._btn_AddCurrent);
            this.Controls.Add(this._pictDown);
            this.Controls.Add(this._pictUp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormShortcuts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormShortcuts";
            this.TransparencyKey = System.Drawing.Color.Snow;
            this.Load += new System.EventHandler(this.FormShortcuts_Load);
            ((System.ComponentModel.ISupportInitialize)(this._pictDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictUp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictDown;
        private System.Windows.Forms.PictureBox _pictUp;
        private System.Windows.Forms.Button _btn_AddCurrent;

    }
}