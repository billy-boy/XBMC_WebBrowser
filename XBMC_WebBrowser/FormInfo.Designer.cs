namespace XBMC_WebBrowser
{
    partial class FormInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this._btnOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // _btnOK
            // 
            this._btnOK.BackgroundImage = global::XBMC_WebBrowser.Properties.Resources.button_normal_906;
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnOK.FlatAppearance.BorderSize = 0;
            this._btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this._btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this._btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnOK.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnOK.Location = new System.Drawing.Point(59, 597);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(906, 64);
            this._btnOK.TabIndex = 1;
            this._btnOK.Text = "Ok";
            this._btnOK.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(53, 50);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // _lblTitle
            // 
            this._lblTitle.BackColor = System.Drawing.Color.Transparent;
            this._lblTitle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTitle.ForeColor = System.Drawing.Color.White;
            this._lblTitle.Image = global::XBMC_WebBrowser.Properties.Resources.InfoMessagePanel_Down;
            this._lblTitle.Location = new System.Drawing.Point(388, 5);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(249, 61);
            this._lblTitle.TabIndex = 46;
            this._lblTitle.Text = "Info";
            this._lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::XBMC_WebBrowser.Properties.Resources.ContentPanel;
            this.CancelButton = this._btnOK;
            this.ClientSize = new System.Drawing.Size(1024, 720);
            this.Controls.Add(this._lblTitle);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._btnOK);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormInfo";
            this.Text = "FormInfo";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label _lblTitle;
    }
}