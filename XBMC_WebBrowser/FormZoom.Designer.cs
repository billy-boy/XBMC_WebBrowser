namespace XBMC_WebBrowser
{
    partial class FormZoom
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
            this._pictZoom = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._pictZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // _pictZoom
            // 
            this._pictZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pictZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this._pictZoom.Location = new System.Drawing.Point(1, 1);
            this._pictZoom.Name = "_pictZoom";
            this._pictZoom.Size = new System.Drawing.Size(598, 398);
            this._pictZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._pictZoom.TabIndex = 0;
            this._pictZoom.TabStop = false;
            // 
            // FormZoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this._pictZoom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormZoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormZoom";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this._pictZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictZoom;

    }
}