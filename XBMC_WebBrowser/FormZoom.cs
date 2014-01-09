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
    public partial class FormZoom : Form
    {
        public FormZoom()
        {
            InitializeComponent();
        }

        public void Repaint(Bitmap bmp)
        {
            Bitmap bmpCopy = (Bitmap)bmp.Clone();
            _pictZoom.Image = bmpCopy;
        }

        public PictureBox Picture { get { return _pictZoom; } }

        private void HandleSpecialKeys(Keys keyData)
        {
            String keys = keyData.ToString();

            //Close?
            if (XWKeys.getInstance().keyMapClose.Contains(keys) || XWKeys.getInstance().keyMapMagnifier.Contains(keys))
            {
                this.Close();
                return;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyDaya)
        {
            //Console.WriteLine("FormContextMenu PCommK | Msg: '" + msg.Msg.ToString() + "' | Key-Data: '" + keyDaya.ToString() + "' | W-Param: '" + msg.WParam.ToInt32().ToString() + "' | L-Param: '" + msg.LParam.ToInt32().ToString() + "'");
            if (XWKeys.getInstance().AllKeys.Contains(keyDaya.ToString()))
            {
                HandleSpecialKeys(keyDaya);
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyDaya);
        }
    }
}
