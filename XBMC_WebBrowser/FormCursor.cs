using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XBMC_WebBrowser
{
    public partial class FormCursor : Form
    {
        public FormCursor()
        {
            InitializeComponent();
        }

        private void FormCursor_Load(object sender, EventArgs e)
        {
            //this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            //this.BackColor = System.Drawing.Color.Transparent;
            this.TransparencyKey = Color.White;
            this.DoubleBuffered = true;
        }
    }
}
