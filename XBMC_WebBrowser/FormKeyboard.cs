using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace XBMC_WebBrowser
{
    public partial class FormKeyboard : Form
    {
        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
    StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        private Dictionary<String, String> _keyboardLayout = new Dictionary<string, string>();
        private bool _shiftClicked = false;
        private String _startText;
        private String _userDataFolder = "";

        public FormKeyboard(String title, String startText, Boolean inputEnabled, String userDataFolder)
        {
            InitializeComponent();        
            _userDataFolder = userDataFolder;
            //load keyboard layout
            importKeyboardLayout();
            fillLayout(false);
            addKeyboardLayoutFunctions();
            //set defaults
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._lblTitle.Text = title;
            this._startText = startText;
            _txtText.Text = _startText;
            //read only?
            if (!inputEnabled)
            {
                _txtText.ReadOnly = true;
                _txtText.TabStop = false;
            }
            //cursor to last position
            _txtText.Select(_txtText.Text.Length, 0);
            //special buttons
            _btn_Shift.ForeColor = Color.FromArgb(85, 85, 85);
            _btn_Delete.ForeColor = Color.FromArgb(85, 85, 85);
            _btn_RemoveAll.ForeColor = Color.FromArgb(85, 85, 85);
            _btn_Space.ForeColor = Color.FromArgb(85, 85, 85);
            _btn_Enter.ForeColor = Color.FromArgb(85, 85, 85);
            _txtText.ForeColor = Color.FromArgb(85, 85, 85);

        }

        private void importKeyboardLayout()
        {
            _keyboardLayout.Clear();
            if (!File.Exists(_userDataFolder + "\\keyboardLayout"))
                return;
            StreamReader str = new StreamReader(_userDataFolder+"\\keyboardLayout",Encoding.UTF8);
            String line;
            while ((line = str.ReadLine()) != null)
            {
                if (line.Contains("=") && !line.StartsWith("["))
                {
                    String[] spl = line.Split('=');
                    _keyboardLayout.Add(spl[0], spl[1].Trim());
                }
            }
        }

        private void fillLayout(bool useUpper)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.Name.StartsWith("_btn_kb_"))
                {
                    Button btn = ctrl as Button;
                    if (btn != null)
                    {
                        String btnName = btn.Name;
                        btnName = btnName.Replace("_btn_kb_","");
                        if (useUpper)
                            btnName += "_u";
                        if (_keyboardLayout.ContainsKey(btnName))
                            btn.Text = _keyboardLayout[btnName];
                        else
                            btn.Text = btnName;
                    }
                }
            }
        }

        private void addKeyboardLayoutFunctions()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.Name.StartsWith("_btn_kb_"))
                {
                    Button btn = ctrl as Button;
                    if (btn != null)
                    {
                        btn.ForeColor = Color.FromArgb(85, 85, 85);
                        //removal is needed because of IDE inhomgency (some buttons have, some doesn't)
                        btn.Click -= button_Click;
                        btn.Click += button_Click;
                        btn.Enter -= button_Enter;
                        btn.Enter += button_Enter;
                        btn.Leave -= button_Leave;
                        btn.Leave += button_Leave;
                        btn.KeyDown -= button_KeyDown;
                        btn.KeyDown += button_KeyDown;
                        btn.PreviewKeyDown -= button_PreviewKeyDown;
                        btn.PreviewKeyDown += button_PreviewKeyDown;
                        btn.MouseEnter -= button_MouseEnter;
                        btn.MouseEnter += button_MouseEnter;
                        btn.MouseLeave -= button_MouseLeave;
                        btn.MouseLeave += button_MouseLeave;
                    }
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (XWKeys.getInstance().keyMapClick.Contains(e.KeyCode))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (XWKeys.getInstance().keyMapDown.Contains(e.KeyCode)
                || XWKeys.getInstance().keyMapUp.Contains(e.KeyCode)
                || XWKeys.getInstance().keyMapRight.Contains(e.KeyCode)
                || XWKeys.getInstance().keyMapLeft.Contains(e.KeyCode)
            )
            {
                _btn_kb_a_1.Focus();
                e.Handled = true;
            }
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Focused)
                return;

            button_Leave(sender, e);
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Focused)
                return;

            if (btn.Name == "_btn_Shift")
            {
                btn.BackgroundImage = Properties.Resources.button_shift_hover;
            }
            else if (btn.Name == "_btn_RemoveAll")
            {
                btn.BackgroundImage = Properties.Resources.button_hover_146;
            }
            else if (btn.Name == "_btn_Space")
            {
                btn.BackgroundImage = Properties.Resources.button_hover_449;
            }
            else if (btn.Name == "_btn_Delete")
            {
                btn.BackgroundImage = Properties.Resources.button_delete_hover;
            }
            else if (btn.Name == "_btn_Enter")
            {
                btn.BackgroundImage = Properties.Resources.button_hover_906;
            }
            else
            {
                btn.BackgroundImage = Properties.Resources.button_hover;
            }
            btn.ForeColor = Color.FromArgb(85, 85, 85);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            if (btn.Name == "_btn_Space")
            {
                _txtText.Text += " ";
                _txtText.SelectionStart = _txtText.Text.Length;
            }
            else if (btn.Name == "_btn_Shift")
            {
                shiftClicked();
            }
            else if (btn.Name == "_btn_Delete")
            {
                if (_txtText.Text.Length > 0)
                {
                    _txtText.Text = _txtText.Text.Substring(0, _txtText.Text.Length - 1);
                    _txtText.SelectionStart = _txtText.Text.Length;
                }
            }
            else if (btn.Name == "_btn_RemoveAll")
            {
                _txtText.Text = _startText;
                _txtText.SelectionStart = _txtText.Text.Length;
            }
            else if (btn.Name == "_btn_Enter")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                _txtText.Text += ((Button)sender).Text;
                _txtText.SelectionStart = _txtText.Text.Length;
                if (_shiftClicked) shiftClicked();
            }
        }

        private void button_Enter(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);
            if (btn == null) return;
            if (btn.Name == "_btn_Shift")
            {
                btn.BackgroundImage = Properties.Resources.button_shift_active;
            }
            else if (btn.Name == "_btn_RemoveAll")
            {
                btn.BackgroundImage = Properties.Resources.button_active_146;
            }
            else if (btn.Name == "_btn_Space")
            {
                btn.BackgroundImage = Properties.Resources.button_active_449;
            }
            else if (btn.Name == "_btn_Delete")
            {
                btn.BackgroundImage = Properties.Resources.button_delete_active;
            }
            else if (btn.Name == "_btn_Enter")
            {
                btn.BackgroundImage = Properties.Resources.button_active_906;
            }
            else
            {
                btn.BackgroundImage = Properties.Resources.button_active;
            }
            btn.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void button_Leave(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);
            if (btn == null) return;
            if ((btn.Name == "_btn_Shift") && _shiftClicked)
                return;
            if (btn.Name == "_btn_Shift")
            {
                btn.BackgroundImage = Properties.Resources.button_shift_normal;
            }
            else if (btn.Name == "_btn_RemoveAll")
            {
                btn.BackgroundImage = Properties.Resources.button_normal_146;
            }
            else if (btn.Name == "_btn_Space")
            {
                btn.BackgroundImage = Properties.Resources.button_normal_449;
            }
            else if (btn.Name == "_btn_Delete")
            {
                btn.BackgroundImage = Properties.Resources.button_delete_normal;
            }
            else if (btn.Name == "_btn_Enter")
            {
                btn.BackgroundImage = Properties.Resources.button_normal_906;
            }
            else
            {
                btn.BackgroundImage = Properties.Resources.button_normal;
            }
            btn.ForeColor = Color.FromArgb(85, 85, 85);
        }

        private void button_KeyDown(object sender, KeyEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            Boolean isKeyboardButton = false;
            if (btn.Name.StartsWith("_btn_kb_"))
                isKeyboardButton = true;

            String row = "", column = "";
            if (isKeyboardButton)
            {
                String[] spl = btn.Name.Replace("_btn_kb_", "").Split('_');
                row = spl[0];
                column = spl[1];
            }

            if (XWKeys.getInstance().keyMapDelete.Contains(e.KeyCode))
            {
                if (_txtText.Text.Length > 0)
                {
                    _txtText.Text = _txtText.Text.Substring(0, _txtText.Text.Length - 1);
                    _txtText.SelectionStart = _txtText.Text.Length;
                }
            }

            else if (XWKeys.getInstance().keyMapDown.Contains(e.KeyCode))
            {
                if (isKeyboardButton && row != "e")
                {
                    char row_c = (char)row[0];
                    row_c++;
                    Button next = this.Controls["_btn_kb_" + row_c.ToString() + "_" + column] as Button;
                    if (next != null)
                    {
                        next.Focus();
                    }
                }
                else if (isKeyboardButton && row == "e")
                {
                    if (Convert.ToInt32(column) <= 3)
                        _btn_Shift.Focus();
                    else if (Convert.ToInt32(column) <= 7)
                        _btn_Space.Focus();
                    else if (Convert.ToInt32(column) <= 10)
                        _btn_RemoveAll.Focus();
                    else
                        _btn_Delete.Focus();
                }
                else
                {
                    if (btn.Name != "_btn_Enter")
                        _btn_Enter.Focus();
                    else
                        _btn_kb_a_1.Focus();
                }
            }
            else if (XWKeys.getInstance().keyMapUp.Contains(e.KeyCode))
            {
                if (isKeyboardButton && row != "a")
                {
                    char row_c = (char)row[0];
                    row_c--;
                    Button next = this.Controls["_btn_kb_" + row_c.ToString() + "_" + column] as Button;
                    if (next != null)
                    {
                        next.Focus();
                    }
                }
                else if (isKeyboardButton && row == "a")
                {
                    _btn_Enter.Focus();
                }
                else
                {
                    switch (btn.Name)
                    {
                        case "_btn_Shift": _btn_kb_e_1.Focus(); break;
                        case "_btn_Space": _btn_kb_e_3.Focus(); break;
                        case "_btn_RemoveAll": _btn_kb_e_9.Focus(); break;
                        case "_btn_Delete": _btn_kb_e_11.Focus(); break;
                        case "_btn_Enter": _btn_Space.Focus(); break;
                    }
                }
            }
            else if (XWKeys.getInstance().keyMapRight.Contains(e.KeyCode))
            {
                if (isKeyboardButton && column != "12")
                {
                    int column_i = Convert.ToInt32(column);
                    column_i++;
                    Button next = this.Controls["_btn_kb_" + row + "_" + column_i.ToString()] as Button;
                    if (next != null)
                    {
                        next.Focus();
                    }
                }
                else if (isKeyboardButton && column == "12")
                {
                    Button next = this.Controls["_btn_kb_" + row + "_" + "1"] as Button;
                    if (next != null)
                    {
                        next.Focus();
                    }
                }
                else 
                {
                    switch (btn.Name)
                    {
                        case "_btn_Shift": _btn_Space.Focus(); break;
                        case "_btn_Space": _btn_RemoveAll.Focus(); break;
                        case "_btn_RemoveAll": _btn_Delete.Focus(); break;
                        case "_btn_Delete": _btn_Shift.Focus(); break;
                        case "_btn_Enter": _btn_Enter.Focus(); break;
                    }
                }
            }
            else if (XWKeys.getInstance().keyMapLeft.Contains(e.KeyCode))
            {
                if (isKeyboardButton && column != "1")
                {
                    int column_i = Convert.ToInt32(column);
                    column_i--;
                    Button next = this.Controls["_btn_kb_" + row + "_" + column_i.ToString()] as Button;
                    if (next != null)
                    {
                        next.Focus();
                    }
                }
                else if (isKeyboardButton && column == "1")
                {
                    Button next = this.Controls["_btn_kb_" + row + "_" + "12"] as Button;
                    if (next != null)
                    {
                        next.Focus();
                    }
                }
                else
                {
                    switch (btn.Name)
                    {
                        case "_btn_Shift": _btn_Delete.Focus(); break;
                        case "_btn_Space": _btn_Shift.Focus(); break;
                        case "_btn_RemoveAll": _btn_Space.Focus(); break;
                        case "_btn_Delete": _btn_RemoveAll.Focus(); break;
                        case "_btn_Enter": _btn_Enter.Focus(); break;
                    }
                }
            }
            else if (XWKeys.getInstance().keyMapClick.Contains(e.KeyCode))
            {
                button_Click(sender, e);
            }
        }

        private void button_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void shiftClicked()
        {
            if (_shiftClicked)
            {
                _shiftClicked = false;
                _btn_Shift.BackgroundImage = Properties.Resources.button_shift_normal;

                fillLayout(false);
            }
            else
            {
                _shiftClicked = true;
                _btn_Shift.BackgroundImage = Properties.Resources.button_shift_active;
                fillLayout(true);
            }
        }

        private void FormKeyboard_Load(object sender, EventArgs e)
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = System.Drawing.Color.Transparent;

            _btn_kb_a_1.Focus();
        }

        private void HandleSpecialKeys(Keys keyData)
        {
            //Close?
            if (XWKeys.getInstance().keyMapClose.Contains(keyData) || XWKeys.getInstance().keyMapKeyboard.Contains(keyData) || XWKeys.getInstance().keyMapNavigate.Contains(keyData))
            {
                this.Close();
                return;
            }

            //Durchreichen an die Buttons
            if (this.ActiveControl.GetType() == typeof(Button))
            {
                button_KeyDown(this.ActiveControl, new KeyEventArgs(keyData));
            }
            //Durchreichen an die Textbox
            else if (this.ActiveControl.GetType() == typeof(TextBox))
            {
                textBox1_KeyDown(this.ActiveControl, new KeyEventArgs(keyData));
            }
        }

        static string GetCharsFromKeys(Keys keys, bool shift, bool altGr)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
                keyboardState[(int)Keys.ShiftKey] = 0xff;
            if (altGr)
            {
                keyboardState[(int)Keys.ControlKey] = 0xff;
                keyboardState[(int)Keys.Menu] = 0xff;
            }
            ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyDaya)
        {
            //sConsole.WriteLine("FormContextMenu PCommK | Msg: '" + msg.Msg.ToString() + "' | Key-Data: '" + keyDaya.ToString() + "' | W-Param: '" + msg.WParam.ToInt32().ToString() + "' | L-Param: '" + msg.LParam.ToInt32().ToString() + "'");
            if (XWKeys.getInstance().AllKeys.Contains(keyDaya))
            {
                HandleSpecialKeys(keyDaya);
                return true;
            }
            //Kein Special-Char? Dann ab damit ins Textfeld!
            else
            {               
                //Delete
                if (keyDaya == Keys.Back)
                {
                    if (_txtText.Text.Length > 0)
                    {
                        _txtText.Text = _txtText.Text.Substring(0, _txtText.Text.Length - 1);
                        _txtText.SelectionStart = _txtText.Text.Length;
                        return true;
                    }
                }
                //Printbare
                else if ((byte)keyDaya >= 31)
                {
                    bool shift = false;
                    bool altgr = false;
                    if ((keyDaya & Keys.Shift) == Keys.Shift) shift = true;
                    if ((keyDaya & Keys.Alt) == Keys.Alt && (keyDaya & Keys.Control) == Keys.Control) altgr = true;
                    _txtText.AppendText(GetCharsFromKeys(keyDaya, shift, altgr));
                    return true;
                }    
                return base.ProcessCmdKey(ref msg, keyDaya);
            }
        }
    }
}
