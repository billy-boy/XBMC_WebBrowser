using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XBMC_WebBrowser
{
    public enum ContextMenuResult { Undefined, Navigate, Keyboard, Magnifier, Favourites, Shortcuts, DoubleClick, RightClick, Tab, Esc, F5 };

    public partial class FormContextMenu : Form
    {
        private ContextMenuResult _result = ContextMenuResult.Undefined;

        public FormContextMenu()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            InitButtons();
        }

        private void InitButtons()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("_btn"))
                {
                    Button btn = c as Button;
                    if (btn != null)
                    {
                        btn.MouseEnter += button_MouseEnter;
                        btn.MouseLeave += button_MouseLeave;
                        btn.Enter += button_Enter;
                        btn.Leave += button_Leave;
                        btn.Click += button_Click;
                        btn.KeyDown += button_KeyDown;
                    }
                }
            }
        }

        private void safeItemAndClose(Button btn)
        {
            switch (btn.Name)
            {
                case "_btn_Navigate": _result = ContextMenuResult.Navigate; break;
                case "_btn_Keyboard": _result = ContextMenuResult.Keyboard; break;
                case "_btn_Magnifier": _result = ContextMenuResult.Magnifier; break;
                case "_btn_Favourites": _result = ContextMenuResult.Favourites; break;
                case "_btn_Shortcuts": _result = ContextMenuResult.Shortcuts; break;
                case "_btn_DoubleClick": _result = ContextMenuResult.DoubleClick; break;
                case "_btn_Rightclick": _result = ContextMenuResult.RightClick; break;
                case "_btn_Tab": _result = ContextMenuResult.Tab; break;
                case "_btn_Esc": _result = ContextMenuResult.Esc; break;
                case "_btn_F5": _result = ContextMenuResult.F5; break;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void hover(Button btn, bool hover)
        {
            if (hover)
                btn.BackgroundImage = Properties.Resources.SubMenuBack_ButtonFocus;
            else
                btn.BackgroundImage = Properties.Resources.SubMenuBack_ButtonNoFocus;
        }

        public ContextMenuResult Result
        {
            get { return _result; }
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            btn.Focus();
            hover(btn, true);
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            if (!btn.Focused)
                hover(btn, false);
        }

        private void button_Enter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            hover(btn, true);
        }

        private void button_Leave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            hover(btn, false);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            safeItemAndClose(btn);
        }

        private void button_KeyDown(object sender, KeyEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            if (XWKeys.getInstance().keyMapUp.Contains(e.KeyCode))
            {
                switch (btn.Name)
                {
                    case "_btn_Navigate": _btn_F5.Focus(); break;
                    case "_btn_Keyboard": _btn_Navigate.Focus(); break;
                    case "_btn_Magnifier": _btn_Keyboard.Focus(); break;
                    case "_btn_Favourites": _btn_Magnifier.Focus(); break;
                    case "_btn_Shortcuts": _btn_Favourites.Focus(); break;
                    case "_btn_DoubleClick": _btn_Shortcuts.Focus(); break;
                    case "_btn_Tab": _btn_DoubleClick.Focus(); break;
                    case "_btn_Esc": _btn_Tab.Focus(); break;
                    case "_btn_F5": _btn_Esc.Focus();  break;
                }
                e.SuppressKeyPress = true;
            }
            else if (XWKeys.getInstance().keyMapDown.Contains(e.KeyCode))
            {
                switch (btn.Name)
                {
                    case "_btn_Navigate": _btn_Keyboard.Focus(); break;
                    case "_btn_Keyboard": _btn_Magnifier.Focus(); break;
                    case "_btn_Magnifier": _btn_Favourites.Focus(); break;
                    case "_btn_Favourites": _btn_Shortcuts.Focus(); break;
                    case "_btn_Shortcuts": _btn_DoubleClick.Focus(); break;
                    case "_btn_DoubleClick": _btn_Tab.Focus(); break;
                    case "_btn_Tab": _btn_Esc.Focus(); break;
                    case "_btn_Esc": _btn_F5.Focus(); break;
                    case "_btn_F5": _btn_Navigate.Focus(); break;
                }
                e.SuppressKeyPress = true;
            }
            else if (XWKeys.getInstance().keyMapClick.Contains(e.KeyCode))
            {
                safeItemAndClose(btn);
                e.SuppressKeyPress = true;
            }
        }

        private void FormContextMenu_Load(object sender, EventArgs e)
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = System.Drawing.Color.Transparent;
        }

        private void HandleSpecialKeys(Keys keyData)
        {
            //Close?
            if (XWKeys.getInstance().keyMapClose.Contains(keyData))
            {
                this.Close();
                return;
            }

            //Durchreichen an die Buttons
            if (this.ActiveControl.GetType() == typeof(Button))
            {
                button_KeyDown(this.ActiveControl, new KeyEventArgs(keyData));
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyDaya)
        {
            //Console.WriteLine("FormContextMenu PCommK | Msg: '" + msg.Msg.ToString() + "' | Key-Data: '" + keyDaya.ToString() + "' | W-Param: '" + msg.WParam.ToInt32().ToString() + "' | L-Param: '" + msg.LParam.ToInt32().ToString() + "'");
            if (XWKeys.getInstance().AllKeys.Contains(keyDaya))
            {
                HandleSpecialKeys(keyDaya);
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyDaya);
        }
    }
}
