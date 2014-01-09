using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XBMC_WebBrowser
{
    public partial class FormMain : Form
    {
        #region Header

        #region Sub-Forms
        private FormZoom formZoom = null;
        private FormKeyboard formKeyboardNavi = null;
        private FormKeyboard formKeyboardSearch = null;
        private FormPopup formPopup = null;
        private FormCursor formCursor;
        private FormFavourites formFavourites = null;
        private FormShortcuts formShortcuts = null;
        private FormContextMenu formContextMenu = null;
        #endregion

        private String mainUrl = "http://www.google.de/";
        private String mainTitle = "";
        private String userAgent = "";
        private String userDataFolder = "";

        private bool showPopups = false;
        private bool showScrollBar = true;
        private bool useCustomCursor = true;
        private bool mouseEnabled = true;
        private bool urlKeyboardEnabled = false;
        private bool supressScriptWarnings = true;
        
        #region Magnifier
        private int magnifierWidth = 1280;
        private int magnifierHeigth = 720;
        private int magnifierZoom = 2;
        private int magnifierCaptureSize = 100;
        #endregion

        #region ActiveX zoom
        private int activexZoom = 100;
        private int activexZoomStep = 50;
        #endregion

        #region Cursor & mouse moving
        private int acceleration;
        private int minMouseSpeed = 10;
        private int maxMouseSpeed = 10;

        private int customCursorSize = 64;
        private int scrollSpeed = 20;

        private Point lastMousePosition;
        private long lastMousePositionChange = 0;
        #endregion

        private SHDocVw.WebBrowser nativeBrowser;

        #region DLL imports and constants
        private const UInt32 MOUSEEVENTF_MOVE = 0x0001;
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        private const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const UInt32 MOUSEEVENTF_RIGHTUP = 0x0010;
        private const UInt32 MOUSEEVENTF_WHEEL = 0x0800;
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("User32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, uint dwExtraInf);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        [DllImport("User32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        #endregion

        public FormMain(String[] args)
        {
            InitializeComponent();

            #region Detect and safe userdata folder
            
            if (args.Length > 0)
                userDataFolder = args[0].Replace("\"", "");

            #endregion
            
            #region Import from userdata config files

            // KeyMap
            String keymap_file = userDataFolder + "\\keymap";       
            if (File.Exists(keymap_file))
            {
                XWKeys.getInstance().importKeymap(keymap_file);
            }

            // Config
            String config_file = userDataFolder + "\\config";
            if (File.Exists(config_file))
            {
                importConfig(config_file);
            }

            #endregion

            //parse arguments
            parseCommandLineArgs(args);

            //set acceleration to minimum
            acceleration = minMouseSpeed;

            //disable the loading circle
            loadingCircleNavigation.Visible = false;

            //apply some colors
            txtNavigateURL.ForeColor = Color.FromArgb(90, 90, 90);
            pnlNavBar.BackColor = Color.FromArgb(210, 255, 255, 255);
            txtNavigateURL.BackColor = Color.FromArgb(255, 255, 255, 255);

            //Resize
            InitSize();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Bring the form to front (from behind XBMC)
            this.BringToFront();

            //Initialization of all things that should be done in loading event
            InitCursor();
            InitWebBrowser();

            //Set the cursor default position
            mouse_event(MOUSEEVENTF_MOVE, 1, 1, 0, 0);
        }

        #region Initializations & config importings/parsings

        private void InitSize()
        {
            //Set form size
            this.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.pnlNavBar.Width = this.ClientSize.Width - (this.pnlNavBar.Left * 2);
            //this.txtNavigateURL.Width = this.Size.Width - this.txtNavigateURL.Left;
            this.txtNavigateURL.Width = this.pnlNavBar.Width - (this.txtNavigateURL.Left * 2);
            this.loadingCircleNavigation.Left = this.pictNavProtocol.Left;
        }

        private void InitWebBrowser()
        {
            //Init the WebBrowser
            webBrowser1.ScrollBarsEnabled = showScrollBar;
            webBrowser1.ScriptErrorsSuppressed = supressScriptWarnings;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            //Navigate to 1st URL
            navigate(mainUrl);
            //Getting natvie webbrowser (ActiveX control) and safe for usage
            nativeBrowser = (SHDocVw.WebBrowser)webBrowser1.ActiveXInstance;
            nativeBrowser.NewWindow2 += nativeBrowser_NewWindow2;
            //webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
        }

        private void InitCursor()
        {
            //Show the cursor
            if (useCustomCursor)
            {
                Cursor.Hide();
                formCursor = new FormCursor();
                String cursorPath = userDataFolder + "\\cursor.png";
                Bitmap oImage = null;
                if (File.Exists(cursorPath))
                {
                    oImage = new Bitmap(cursorPath);
                }
                else
                {
                    oImage = new Bitmap(XBMC_WebBrowser.Properties.Resources.cursorBlue);
                }
                formCursor.BackgroundImage = oImage;
                formCursor.MinimumSize = new System.Drawing.Size(32, 32);
                formCursor.Size = new System.Drawing.Size(customCursorSize, customCursorSize);
                formCursor.Location = new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
                formCursor.Show();
            }
        }

        private void importConfig(String file)
        {
            StreamReader str = new StreamReader(file);
            String line;
            while ((line = str.ReadLine()) != null)
            {
                if (line.Contains("="))
                {
                    String[] spl = line.Split('=');
                    switch (spl[0])
                    {
                        case "mainTitle": mainTitle = spl[1].Trim(); break;
                        case "mainURL": mainUrl = spl[1].Trim(); break;
                        case "userAgent": userAgent = spl[1].Trim(); break;
                        case "showPopups": showPopups = Boolean.Parse(spl[1].Trim()); break;
                        case "showScrollBar": showScrollBar = Boolean.Parse(spl[1].Trim()); break;
                        case "useCustomCursor": useCustomCursor = Boolean.Parse(spl[1].Trim()); break;
                        case "customCursorSize": customCursorSize = Int32.Parse(spl[1].Trim()); break;
                        case "mouseEnabled": mouseEnabled = Boolean.Parse(spl[1].Trim()); break;
                        case "urlKeyboardEnabed": urlKeyboardEnabled = Boolean.Parse(spl[1].Trim()); break;
                        case "supressScriptWarnings": supressScriptWarnings = Boolean.Parse(spl[1].Trim()); break;
                        case "scrollSpeed": scrollSpeed = Int32.Parse(spl[1].Trim()); break;
                        case "magnifierWidth": magnifierWidth = Int32.Parse(spl[1].Trim()); break;
                        case "magnifierHeigth": magnifierHeigth = Int32.Parse(spl[1].Trim()); break;
                        case "magnifierZoom": magnifierZoom = Int32.Parse(spl[1].Trim()); break;
                        case "activexZoomStep": activexZoomStep = Int32.Parse(spl[1].Trim()); break;
                    }
                }
            }
            str.Close();
        }

        private void parseCommandLineArgs(String[] args)
        {
            //args[0] allready used
            if (args.Length > 1)
                mainTitle = args[1].Replace("\"", "");
            if (args.Length > 2)
                mainUrl = Uri.UnescapeDataString(args[2]);
            if (args.Length > 3)
                activexZoomStep = Convert.ToInt32(args[3]);
            if (args.Length > 4)
                showPopups = (args[4] == "yes");
            if (args.Length > 5)
                minMouseSpeed = Convert.ToInt32(args[5]);
            if (args.Length > 6)
                maxMouseSpeed = Convert.ToInt32(args[6]);
            if (args.Length > 7)
            {
                String[] spl = args[7].Split('x');
                magnifierWidth = Convert.ToInt32(spl[0]);
                magnifierHeigth = Convert.ToInt32(spl[1]);
            }
            if (args.Length > 8)
                useCustomCursor = (args[8] == "true");
            if (args.Length > 9)
                customCursorSize = Convert.ToInt32(args[9]);
            if (args.Length > 10)
                showScrollBar = (args[10] == "yes");
            if (args.Length > 11)
                scrollSpeed = Convert.ToInt32(args[11]);
            if (args.Length > 12)
                userAgent = args[12].Replace("\"", "");
        }

        #endregion
                
        private void navigate(String url)
        {
            if (String.IsNullOrEmpty(url)) return;
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "http://" + url;
            try
            {
                if (userAgent == "")
                    webBrowser1.Navigate(new Uri(url));
                else
                    webBrowser1.Navigate(new Uri(url), null, null, "User-Agent: " + userAgent);
                webBrowser1.Focus();
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }
        
        private void setAcceleration()
        {
            if ((DateTime.Now.Ticks - lastMousePositionChange) <= 1000000)
            {
                if (acceleration <= maxMouseSpeed)
                    acceleration++;
            }
            else
                acceleration = minMouseSpeed;
        }

        private void HandleSpecialKeys(Keys keyData)
        {
            try
            {
                String keys = keyData.ToString();
                //String keys = "";
                //foreach (int i in Enum.GetValues(typeof(Keys)))
                //{
                //    if (GetAsyncKeyState(i) == -32767)
                //    {
                //        keys += Enum.GetName(typeof(Keys), i) + " ";
                //    }
                //}
                keys = keys.Trim();
                if (keys.StartsWith("ShiftKey "))
                    keys = keys.Substring(9);
                if (keys.StartsWith("Menu "))
                    keys = keys.Substring(5);

                if (keys != "")
                {
                    if (XWKeys.getInstance().keyMapLeft.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.X > this.webBrowser1.Left)
                             Cursor.Position = new Point(Cursor.Position.X - acceleration, Cursor.Position.Y);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.X <= this.webBrowser1.Left)
                                webBrowser1.Navigate("javascript:window.scrollBy(-" + scrollSpeed + ", 0);");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(-" + scrollSpeed + ", 0);");
                    }
                    else if (XWKeys.getInstance().keyMapUp.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.Y > this.webBrowser1.Top)
                                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - acceleration);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.Y <= this.webBrowser1.Top)
                                webBrowser1.Navigate("javascript:window.scrollBy(0, -" + scrollSpeed + ");");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(0, -" + scrollSpeed + ");");
                    }
                    else if (XWKeys.getInstance().keyMapRight.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.X < (this.webBrowser1.Left + this.webBrowser1.Size.Width) - customCursorSize)
                                Cursor.Position = new Point(Cursor.Position.X + acceleration, Cursor.Position.Y);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.X >= (this.webBrowser1.Left + this.webBrowser1.Size.Width) - customCursorSize)
                                webBrowser1.Navigate("javascript:window.scrollBy(" + scrollSpeed + ", 0);");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(" + scrollSpeed + ", 0);");
                    }
                    else if (XWKeys.getInstance().keyMapDown.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.Y < (this.webBrowser1.Top + this.webBrowser1.Size.Height) - customCursorSize)
                                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + acceleration);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.Y >= (this.webBrowser1.Top + this.webBrowser1.Size.Height) - customCursorSize)
                                webBrowser1.Navigate("javascript:window.scrollBy(0, " + scrollSpeed + ");");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(0, " + scrollSpeed + ");");
                    }
                    else if (XWKeys.getInstance().keyMapUpLeft.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.X > this.webBrowser1.Left && Cursor.Position.Y > this.webBrowser1.Top)
                                Cursor.Position = new Point(Cursor.Position.X - acceleration, Cursor.Position.Y - acceleration);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.Y <= this.webBrowser1.Top)
                                webBrowser1.Navigate("javascript:window.scrollBy(0, -" + scrollSpeed + ");");
                            if (Cursor.Position.X <= this.webBrowser1.Left)
                                webBrowser1.Navigate("javascript:window.scrollBy(-" + scrollSpeed + ", 0);");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(-" + scrollSpeed + ", -" + scrollSpeed + ");");
                    }
                    else if (XWKeys.getInstance().keyMapUpRight.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.X < (this.webBrowser1.Left + this.webBrowser1.Size.Width - customCursorSize) && Cursor.Position.Y > this.webBrowser1.Top)
                                Cursor.Position = new Point(Cursor.Position.X + acceleration, Cursor.Position.Y - acceleration);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.Y <= this.webBrowser1.Top)
                                webBrowser1.Navigate("javascript:window.scrollBy(0, -" + scrollSpeed + ");");
                            if (Cursor.Position.X >= (this.webBrowser1.Left + this.webBrowser1.Size.Width - customCursorSize))
                                webBrowser1.Navigate("javascript:window.scrollBy(" + scrollSpeed + ", 0);");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(" + scrollSpeed + ", -" + scrollSpeed + ");");
                    }
                    else if (XWKeys.getInstance().keyMapDownLeft.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.X > this.webBrowser1.Left && Cursor.Position.Y < this.webBrowser1.Top + this.webBrowser1.Size.Height - customCursorSize)
                                Cursor.Position = new Point(Cursor.Position.X - acceleration, Cursor.Position.Y + acceleration);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.Y >= this.webBrowser1.Top + this.webBrowser1.Size.Height - customCursorSize)
                                webBrowser1.Navigate("javascript:window.scrollBy(0, " + scrollSpeed + ");");
                            if (Cursor.Position.X <= this.webBrowser1.Left)
                                webBrowser1.Navigate("javascript:window.scrollBy(-" + scrollSpeed + ", 0);");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(-" + scrollSpeed + ", " + scrollSpeed + ");");
                    }
                    else if (XWKeys.getInstance().keyMapDownRight.Contains(keys))
                    {
                        if (mouseEnabled)
                        {
                            setAcceleration();
                            if (Cursor.Position.Y < this.webBrowser1.Top+this.webBrowser1.Size.Height - customCursorSize && Cursor.Position.X < this.webBrowser1.Left + this.webBrowser1.Size.Width - customCursorSize)
                                Cursor.Position = new Point(Cursor.Position.X + acceleration, Cursor.Position.Y + acceleration);
                            lastMousePositionChange = DateTime.Now.Ticks;
                            if (Cursor.Position.Y >= this.webBrowser1.Top+this.webBrowser1.Size.Height - customCursorSize)
                                webBrowser1.Navigate("javascript:window.scrollBy(0, " + scrollSpeed + ");");
                            if (Cursor.Position.X >= this.webBrowser1.Left + this.webBrowser1.Size.Width - customCursorSize)
                                webBrowser1.Navigate("javascript:window.scrollBy(" + scrollSpeed + ", 0);");
                        }
                        else
                            webBrowser1.Navigate("javascript:window.scrollBy(" + scrollSpeed + ", " + scrollSpeed + ");");
                    }
                    else if (XWKeys.getInstance().keyMapClick.Contains(keys))
                    {
                        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    }
                    else if (XWKeys.getInstance().keyMapDoubleClick.Contains(keys))
                    {
                        doubleClick();
                    }
                    else if (XWKeys.getInstance().keyMapClose.Contains(keys))
                    {
                        if (formPopup != null)
                        {
                            formPopup.Close();
                            formPopup = null;
                        }
                        else if (formZoom != null)
                        {
                            formZoom.Close();
                            formZoom = null;
                        }
                        else if (formKeyboardNavi != null)
                        {
                            formKeyboardNavi.Close();
                            formKeyboardNavi = null;
                        }
                        else if (formKeyboardSearch != null)
                        {
                            formKeyboardSearch.Close();
                            formKeyboardSearch = null;
                        }
                        else if (formFavourites != null)
                        {
                            formFavourites.Close();
                            formFavourites = null;
                        }
                        else if (formShortcuts != null)
                        {
                            formShortcuts.Close();
                            formShortcuts = null;
                        }
                        else if (formContextMenu != null)
                        {
                            formContextMenu.Close();
                            formContextMenu = null;
                        }
                        else
                        {
                            Process[] p = Process.GetProcessesByName("xbmc");
                            if (p.Count() > 0)
                            {
                                ShowWindow(p[0].MainWindowHandle, SW_SHOWMAXIMIZED);
                                SetForegroundWindow(p[0].MainWindowHandle);
                            }
                            Application.Exit();
                        }
                    }
                    else if (XWKeys.getInstance().keyMapZoomIn.Contains(keys))
                    {
                        if (formZoom != null)
                        {
                            magnifierZoom++;
                            lastMousePosition = Cursor.Position;
                            formZoom.Hide();
                            updateMagnifier();
                        }
                        else
                        {
                            activexZoom += activexZoomStep;
                            Zoom(activexZoom);
                            //SendKeys.Send("^{ADD}");
                        }
                    }
                    else if (XWKeys.getInstance().keyMapZoomOut.Contains(keys))
                    {
                        if (formZoom != null && magnifierZoom > 2)
                        {
                            magnifierZoom--;
                            lastMousePosition = Cursor.Position;
                            formZoom.Hide();
                            updateMagnifier();
                        }
                        else
                        {
                            activexZoom -= activexZoomStep;
                            Zoom(activexZoom);
                            //SendKeys.Send("^{SUBTRACT}");
                        }
                    }
                    else if (XWKeys.getInstance().keyMapTAB.Contains(keys))
                    {
                        pressTab();
                    }
                    else if (XWKeys.getInstance().keyMapESC.Contains(keys))
                    {
                        pressEsc();
                    }
                    else if (XWKeys.getInstance().keyMapF5.Contains(keys))
                    {
                        pressF5();
                    }
                    else if (XWKeys.getInstance().keyMapMagnifier.Contains(keys))
                    {
                        showMagnifier();
                    }
                    else if (XWKeys.getInstance().keyMapNavigate.Contains(keys))
                    {
                        enterUrl();
                    }
                    else if (XWKeys.getInstance().keyMapKeyboard.Contains(keys))
                    {
                        showKeyboard();
                    }
                    else if (XWKeys.getInstance().keyMapFavourites.Contains(keys))
                    {
                        showFavourites();
                    }
                    else if (XWKeys.getInstance().keyMapShortCuts.Contains(keys))
                    {
                        showShortcuts();
                    }
                    else if (XWKeys.getInstance().keyMapToggleMouse.Contains(keys))
                        mouseEnabled = !mouseEnabled;
                    else if (XWKeys.getInstance().keyMapContextMenu.Contains(keys))
                    {
                        showContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        private void Zoom(int factor)
        {
            object pvaIn = factor;
            try
            {
                this.nativeBrowser.ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, ref pvaIn, IntPtr.Zero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Magnify handling

        private Timer _timerUpdateMagnifier = new Timer();

        private void updateMagnifier_Tick(object sender, EventArgs e)
        {
            Timer t = sender as Timer;
            if (t == null) return;

            t.Enabled = false;
            updateMagnifier();
        }

        private void showMagnifier()
        {
            if (formZoom == null)
            {
                formZoom = new FormZoom();
                formZoom.Size = new System.Drawing.Size(magnifierWidth, magnifierHeigth);
                updateMagnifier();
            }
            else
            {
                formZoom.Close();
                formZoom = null;
            }
        }

        private void updateMagnifier()
        {
            if (formZoom != null)
            {
                //Taking a screenshot
                Image img = XWMagnifier.CaptureMagnifier(this.Handle,Cursor.Position, new Size(magnifierCaptureSize,magnifierCaptureSize));
                //img.Save("E:\\xbmc_webbrowser\\orig_" + DateTime.Now.Ticks.ToString() + ".png");

                //Zooming the screenshot
                Bitmap magBmp = new Bitmap(magnifierWidth, magnifierHeigth);
                Graphics g = this.CreateGraphics();
                g = Graphics.FromImage(magBmp);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, magBmp.Width, magBmp.Height));
                //magBmp.Save("E:\\xbmc_webbrowser\\zoom_" + DateTime.Now.Ticks.ToString() + ".png");

                //Painting the screenshot
                //formZoom = new FormZoom();
                //formZoom.Size = new Size(magnifierWidth, magnifierHeigth);
                formZoom.Repaint(magBmp);

                //Clean up
                g.Dispose();
                g = null;
                img.Dispose();
                img = null;
                magBmp.Dispose();
                magBmp = null;

                //Show the magnifier if hidden
                if (!formZoom.Visible)
                    formZoom.Show();

                //Positioning the magnifier correctly
                formZoom.Location = new Point(Cursor.Position.X - formZoom.Width / 2, Cursor.Position.Y - formZoom.Height / 2);
            }
        }

        #endregion

        #region Menu button handling

        private void showContextMenu()
        {
            if (formContextMenu == null)
            {
                formContextMenu = new FormContextMenu();
                formContextMenu.ShowDialog();
                ContextMenuResult result = formContextMenu.Result;

                switch (result)
                {
                    case ContextMenuResult.Navigate: enterUrl(); break;
                    case ContextMenuResult.Keyboard: showKeyboard(); break;
                    case ContextMenuResult.Magnifier: showMagnifier(); break;
                    case ContextMenuResult.Favourites: showFavourites(); break;
                    case ContextMenuResult.Shortcuts: showShortcuts(); break;
                    case ContextMenuResult.DoubleClick: doubleClick(); break;
                    case ContextMenuResult.RightClick: rightClick(); break;
                    case ContextMenuResult.Tab: pressTab(); break;
                    case ContextMenuResult.Esc: pressEsc(); break;
                    case ContextMenuResult.F5: pressF5(); break;
                }
                //else if (entry == "Toggle Mouse/Scroll")
                //    mouseEnabled = !mouseEnabled;
                
                formContextMenu = null;
            }
            else
            {
                formContextMenu.Close();
                formContextMenu = null;
            }
        }

        private static void rightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        private static void doubleClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private static void pressEsc()
        {
            SendKeys.Send("{ESC}");
        }

        private static void pressTab()
        {
            SendKeys.Send("{TAB}");
        }

        private static void pressF5()
        {
            SendKeys.Send("{F5}");
        }

        private void enterUrl(String showingUrl = "http://")
        {
            if (formKeyboardNavi == null)
            {
                formKeyboardNavi = new FormKeyboard("Navigiere zu:", showingUrl, true, userDataFolder);
                formKeyboardNavi._txtText.SelectionStart = 7;
                formKeyboardNavi.ShowDialog();
                if (formKeyboardNavi._txtText.Text != "")
                {
                    String url = formKeyboardNavi._txtText.Text;
                    webBrowser1.Navigate(url);
                }
                formKeyboardNavi = null;
            }
            else
            {
                formKeyboardNavi.Close();
                formKeyboardNavi = null;
            }
        }

        private void showKeyboard()
        {
            if (formKeyboardSearch == null)
            {
                formKeyboardSearch = new FormKeyboard("Texteingabe", "", false, userDataFolder);
                formKeyboardSearch.ShowDialog();
                if (formKeyboardSearch._txtText.Text != "")
                {
                    Clipboard.SetText(formKeyboardSearch._txtText.Text);
                    SendKeys.Send("^v");
                }
                formKeyboardSearch = null;
            }
            else
            {
                formKeyboardSearch.Close();
                formKeyboardSearch = null;
            }
        }

        private void showFavourites()
        {
            if (formFavourites == null)
            {
                XWFavouriteList.getInstance().loadFavourites(userDataFolder);
                if (XWFavouriteList.getInstance().Count <= 0)
                {
                    //TODO: ERROR
                }
                else
                {
                    formFavourites = new FormFavourites(userDataFolder, webBrowser1.Url.ToString());
                    if (formFavourites.ShowDialog() == DialogResult.OK)
                    {
                        XWFavourite favorit = formFavourites.Favorit;
                        //mainTitle = ((ListBoxEntry)formFavourites.listBoxFavs.SelectedItem).title;
                        //importPageSettings(mainTitle);
                        navigate(favorit.URL);
                    }
                    formFavourites = null;
                }
            }
            else
            {
                formFavourites.Close();
                formFavourites = null;
            }
        }

        private void showShortcuts()
        {
            if (formShortcuts == null)
            {
                String mainURL = webBrowser1.Url.ToString().Replace("http://", "").Replace("https://", "");
                if (mainURL.IndexOf("/") >= 0)
                    mainURL = mainURL.Substring(0, mainURL.IndexOf("/"));
                XWShortcutList.getInstance(mainURL).loadShortcuts(userDataFolder);
                if (XWShortcutList.getInstance(mainURL).Count <= 0)
                {
                    //TODO: ERROR
                }
                else
                {
                    formShortcuts = new FormShortcuts(userDataFolder, mainURL, webBrowser1.Url.ToString());
                    if (formShortcuts.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        webBrowser1.Navigate(formShortcuts.Shortcut.URL);
                    formShortcuts = null;
                }
            }
            else
            {
                formShortcuts.Close();
                formShortcuts = null;
            }
        }

        #endregion

        #region WebBrowser handling

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //Show the URL and Info
            txtNavigateURL.Text = webBrowser1.Url.ToString();
            if (webBrowser1.Url.Scheme == "https")
            {
                pictNavProtocol.Image = Properties.Resources.https;
            }
            else
            {
                pictNavProtocol.Image = Properties.Resources.globe;
            }
            this.loadingCircleNavigation.Active = false;
            this.loadingCircleNavigation.Visible = false;
            this.pictNavProtocol.Visible = true;

            //Absolute finished loading
            if (e.Url.AbsolutePath == webBrowser1.Url.AbsolutePath)
            {
                System.Threading.Thread.Sleep(100);
                ((SHDocVw.WebBrowser)webBrowser1.ActiveXInstance).ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, activexZoom, IntPtr.Zero);
                //webBrowser1.DocumentCompleted -= webBrowser1_DocumentCompleted;
                webBrowser1.Navigate("javascript:window.scrollBy(0,-500);");
            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //do not circle on i.e. javascript: navigations
            if (e.Url.Scheme == "http" || e.Url.Scheme == "https")
            {
                this.pictNavProtocol.Visible = false;
                this.loadingCircleNavigation.Visible = true;
                this.loadingCircleNavigation.Active = true;
            }
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.loadingCircleNavigation.Active = false;
            this.loadingCircleNavigation.Visible = false;
            this.pictNavProtocol.Visible = true;
        }

        void nativeBrowser_NewWindow2(ref object ppDisp, ref bool Cancel)
        {
            if (showPopups)
            {
                formPopup = new FormPopup();
                formPopup.Location = new Point(0, 0);
                formPopup.Size = this.Size;
                formPopup.Show();
                ppDisp = formPopup.webBrowser1.ActiveXInstance;
            }
            else
                Cancel = true;
        }

        #endregion
                
        protected override bool ProcessCmdKey(ref Message msg, Keys keyDaya)
        {
            //Console.WriteLine("FormMain | Msg: '" + msg.Msg.ToString() + "' | Key-Data: '" + keyDaya.ToString() + "' | W-Param: '" + msg.WParam.ToInt32().ToString() + "' | L-Param: '" + msg.LParam.ToInt32().ToString() + "'");
            if (XWKeys.getInstance().AllKeys.Contains(keyDaya.ToString()))
            {
                HandleSpecialKeys(keyDaya);
                return true;
            }
            else              
                return base.ProcessCmdKey(ref msg, keyDaya);
        }

        #region GUI events

        private void timer_Tick(object sender, EventArgs e)
        {
            if (useCustomCursor && formCursor != null)
                formCursor.Location = new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);

            if ((Cursor.Position.X > lastMousePosition.X + 5 || Cursor.Position.X < lastMousePosition.X - 5
                || Cursor.Position.Y > lastMousePosition.Y + 5 || Cursor.Position.Y < lastMousePosition.Y - 5)
                && formZoom != null)
            {
                lastMousePosition = Cursor.Position;

                //formZoom.Hide();
                formZoom.Close();
                formZoom = null;

                //_timerUpdateMagnifier.Interval = 20;
                //_timerUpdateMagnifier.Tick += updateMagnifier_Tick;
                //_timerUpdateMagnifier.Enabled = false;
            }
        }

        private void txtNavigateURL_Enter(object sender, EventArgs e)
        {
            try
            {
                txtNavigateURL.ForeColor = Color.FromArgb(0, 0, 0);
            }
            catch (Exception ex) { }
            if (urlKeyboardEnabled)
            {
                enterUrl(txtNavigateURL.Text);
            }
        }

        private void txtNavigateURL_Leave(object sender, EventArgs e)
        {
            try
            {
                txtNavigateURL.ForeColor = Color.FromArgb(90, 90, 90);
            }
            catch (Exception ex) { }
        }

        private void txtNavigateURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.Enter)
            {
                navigate(txtNavigateURL.Text);
            }
        }

        #endregion

    }
}
