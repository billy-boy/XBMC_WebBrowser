using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XBMC_WebBrowser
{
    public enum InfoType { Info, Warning, Error }

    public partial class FormInfo : Form
    {
        private InfoType _infoType;

        public FormInfo(InfoType infoType)
        {
            InitializeComponent();
            _infoType = infoType;
        }
    }
}
