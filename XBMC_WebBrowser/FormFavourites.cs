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
    public partial class FormFavourites : Form
    {
        private FormKeyboard formKeyboard;
        private XWFavourite _favorit = null;
        private int _firstItem = 0;
        private int _buttonCount = 7;
        private String _userDataFolder = "";
        private String _currentURL = "";

        public FormFavourites(String userdataFolder, String currentUrl)
        {
            InitializeComponent();
            _userDataFolder = userdataFolder;
            _currentURL = currentUrl;
            this.DialogResult = DialogResult.Cancel;
            InitButtons();
        }

        private void InitButtons()
        {
            int startPosition = _pictUp.Top + _btn_AddCurrent.Height + _pictUp.Height;

            for (int i = 1; i <= _buttonCount; i++)
            {
                Button btn = new Button();
                btn.Name = "_btn_list_" + i;
                btn.Location = new Point(0, startPosition);
                btn.Size = new Size(this.Width, 64);
                btn.BackColor = Color.Transparent;
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackgroundImage = Properties.Resources.SubMenuBack_ButtonNoFocus;
                btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btn.FlatAppearance.CheckedBackColor = Color.Transparent;
                btn.Text = "";
                btn.Enabled = false;
                btn.ForeColor = Color.White;
                btn.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn.MouseEnter += button_MouseEnter;
                btn.MouseLeave += button_MouseLeave;
                btn.Enter += button_Enter;
                btn.Leave += button_Leave;
                btn.Click += button_Click;
                btn.KeyDown += button_KeyDown;
                this.Height += btn.Height;
                this._pictDown.Top = btn.Top + btn.Height;

                this.SuspendLayout();
                this.Controls.Add(btn);
                this.ResumeLayout(false); 
                
                startPosition += btn.Height;
            }

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

            if (btn.Name == "_btn_AddCurrent")
                addItemAndClose();
            else
                safeItemAndClose(btn);
        }

        private void button_KeyDown(object sender, KeyEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            if (XWKeys.getInstance().keyMapUp.Contains(e.KeyCode.ToString()))
            {
                if (btn.Name.StartsWith("_btn_list_"))
                {
                    int btnNumber = Convert.ToInt32(btn.Name.Replace("_btn_list_", ""));
                    if (btnNumber == 1)
                    {
                        if (_firstItem != 0)
                        {
                            int start = _firstItem -1;
                            if (start < 0) start = 0;
                            fillFavorites(start);
                        }
                        else
                            this._btn_AddCurrent.Focus();
                    }
                    else
                    {
                        btnNumber--;
                        this.Controls["_btn_list_" + btnNumber].Focus();
                    }
                }
                e.SuppressKeyPress = true;
            }
            else if (XWKeys.getInstance().keyMapDown.Contains(e.KeyCode.ToString()))
            {
                if (btn.Name.StartsWith("_btn_list_"))
                {
                    int btnNumber = Convert.ToInt32(btn.Name.Replace("_btn_list_", ""));
                    if (btnNumber == _buttonCount)
                    {
                        if (_firstItem + _buttonCount < XWFavouriteList.getInstance().Count)
                        {
                            int start = _firstItem + 1;
                            fillFavorites(start);
                        }
                    }
                    else
                    {
                        btnNumber++;
                        this.Controls["_btn_list_" + btnNumber].Focus();
                    }
                }
                else if (btn.Name == "_btn_AddCurrent")
                {
                    this.Controls["_btn_list_1"].Focus();
                }
                e.SuppressKeyPress = true;
            }
            else if (XWKeys.getInstance().keyMapClick.Contains(e.KeyCode.ToString()))
            {
                if (btn.Name == "_btn_AddCurrent")
                    addItemAndClose();
                else
                    safeItemAndClose(btn);
            }
            else if (XWKeys.getInstance().keyMapDelete.Contains(e.KeyCode.ToString()))
            {
                if (btn.Name.StartsWith("_btn_list_"))
                {
                    deleteItemAndClose(btn);
                }
                e.SuppressKeyPress = true;
            }
        }

        private void addItemAndClose()
        {
            formKeyboard = new FormKeyboard("Name eingeben", "", true, _userDataFolder);
            if (formKeyboard.ShowDialog() == DialogResult.OK)
            {
                String title = formKeyboard._txtText.Text;
                XWFavourite favorit = new XWFavourite(title, _currentURL);
                favorit.Save(_userDataFolder + "\\sites");
                //XWFavouriteList.getInstance().loadFavourites(_userDataFolder);

                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void deleteItemAndClose(Button btn)
        {
            XWFavouriteList.getInstance()[btn.Text].Delete(_userDataFolder+"\\sites");

            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.Close();
        }

        private void safeItemAndClose(Button btn)
        {
            this._favorit = XWFavouriteList.getInstance()[btn.Text];

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

        public XWFavourite Favorit { get { return _favorit; } }

        private void fillFavorites(int start)
        {
            XWFavouriteList list = XWFavouriteList.getInstance();
            int btnCounter = 1;
            for (int i = start; start < _buttonCount; i++)
            {
                if (i >= list.Count) break; //Abbruch wenn nichts mehr geladen weerden kann
                _firstItem = start;

                Button btn = this.Controls["_btn_list_" + btnCounter] as Button;
                if (btn == null) break; //Abbruch wenn max. Anzahl Buttons erfüllt
                XWFavourite favorit = list[i];
                btn.Text = favorit.Title;
                btn.Enabled = true;     

                btnCounter++;
            }
        }

        private void clearFavorites()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Name.StartsWith("_btn_list_"))
                {
                    Button btn = c as Button;
                    if (btn != null)
                    {
                        btn.Text = "";
                        btn.Enabled = false;
                    }
                }
            }
        }

        private void FormFavourites_Load(object sender, EventArgs e)
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = System.Drawing.Color.Transparent;

            clearFavorites();
            fillFavorites(0);

            this.Controls["_btn_list_1"].Focus();
        }

        private void HandleSpecialKeys(Keys keyData)
        {
            String keys = keyData.ToString();
            
            //Close?
            if (XWKeys.getInstance().keyMapClose.Contains(keys))
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
