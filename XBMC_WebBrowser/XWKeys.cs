using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XBMC_WebBrowser
{
    public class XWKeys
    {

        public const UInt32 WM_KEYDOWN = 0x100;
        public const UInt32 WM_KEYUP = 0x101;
        public const UInt32 WM_CHAR = 0x102;
        public const UInt32 WM_SYSKEYDOWN = 0x104;
        public const UInt32 WM_SYSKEYUP = 0x105;
        #region Singleton

        private static XWKeys _instance = null;

        public static XWKeys getInstance()
        {
            if (_instance == null) _instance = new XWKeys();
            return _instance;
        }

        #endregion
        
        private ArrayList _allKeys = new ArrayList();
        public XWKeyList keyMapUp = new XWKeyList(){ Keys.NumPad8},
                        keyMapDown = new XWKeyList(){ Keys.NumPad2},
                        keyMapLeft = new XWKeyList(){ Keys.NumPad4},
                        keyMapRight = new XWKeyList(){ Keys.NumPad6},
                        keyMapUpLeft = new XWKeyList(){ Keys.NumPad7},
                        keyMapUpRight = new XWKeyList(){ Keys.NumPad9},
                        keyMapDownLeft = new XWKeyList(){ Keys.NumPad1},
                        keyMapDownRight = new XWKeyList(){ Keys.NumPad3},
                        keyMapClick = new XWKeyList(){ Keys.NumPad5},
                        keyMapDoubleClick = new XWKeyList(){},
                        keyMapZoomIn = new XWKeyList(){ Keys.Add},
                        keyMapZoomOut = new XWKeyList(){ Keys.Subtract},
                        keyMapMagnifier = new XWKeyList() { Keys.Menu | Keys.Alt },
                        keyMapNavigate = new XWKeyList(){},
                        keyMapClose = new XWKeyList(){ Keys.NumPad0},
                        keyMapKeyboard = new XWKeyList(){},
                        keyMapFavourites = new XWKeyList(){},
                        keyMapShortCuts = new XWKeyList(){},
                        keyMapTAB = new XWKeyList(){},
                        keyMapESC = new XWKeyList(){},
                        keyMapToggleMouse = new XWKeyList(){ Keys.Multiply },
                        keyMapContextMenu = new XWKeyList(){ Keys.Divide },
                        keyMapF5 = new XWKeyList(){},
                        keyMapDelete = new XWKeyList(){ Keys.Decimal };

        public XWKeys()
        {
            saveMappingList();
        }

        private void clearKeymap()
        {
            _allKeys.Clear();
            keyMapUp = new XWKeyList(){};
            keyMapDown = new XWKeyList(){};
            keyMapLeft = new XWKeyList(){};
            keyMapRight = new XWKeyList(){};
            keyMapUpLeft = new XWKeyList(){};
            keyMapUpRight = new XWKeyList(){};
            keyMapDownLeft = new XWKeyList(){};
            keyMapDownRight = new XWKeyList(){};
            keyMapClick = new XWKeyList(){};
            keyMapDoubleClick = new XWKeyList(){};
            keyMapZoomIn = new XWKeyList(){};
            keyMapZoomOut = new XWKeyList(){};
            keyMapMagnifier = new XWKeyList() {};
            keyMapNavigate = new XWKeyList(){};
            keyMapClose = new XWKeyList(){};
            keyMapKeyboard = new XWKeyList(){};
            keyMapFavourites = new XWKeyList(){};
            keyMapShortCuts = new XWKeyList(){};
            keyMapTAB = new XWKeyList(){};
            keyMapESC = new XWKeyList(){};
            keyMapToggleMouse = new XWKeyList(){};
            keyMapContextMenu = new XWKeyList(){};
            keyMapF5 = new XWKeyList(){};
            keyMapDelete = new XWKeyList(){};
        }

        private void importKeyLine(String line, XWKeyList list)
        {
            String[] spl = line.Split(',');
            Keys key = (Keys)Convert.ToByte(spl[0]);
            if (line.ToLower().Contains("shift"))
                key = key | Keys.Shift;
            if (line.ToLower().Contains("alt"))
                key = key | Keys.Alt;
            if (line.ToLower().Contains("control"))
                key = key | Keys.Control;
            list.Add(key);
        }

        public void importKeymap(String file)
        {
            StreamReader str = new StreamReader(file);
            String line;
            while ((line = str.ReadLine()) != null)
            {
                if (line.Contains("="))
                {
                    String[] spl = line.Split('=');
                    if (spl[0] == "Up")
                        importKeyLine(spl[1].Trim(), keyMapUp);
                    else if (spl[0] == "Down")
                        importKeyLine(spl[1].Trim(), keyMapDown);
                    else if (spl[0] == "Left")
                        importKeyLine(spl[1].Trim(), keyMapLeft);
                    else if (spl[0] == "Right")
                        importKeyLine(spl[1].Trim(), keyMapRight);
                    else if (spl[0] == "UpLeft")
                        importKeyLine(spl[1].Trim(), keyMapUpLeft);
                    else if (spl[0] == "UpRight")
                        importKeyLine(spl[1].Trim(), keyMapUpRight);
                    else if (spl[0] == "DownLeft")
                        importKeyLine(spl[1].Trim(), keyMapDownLeft);
                    else if (spl[0] == "DownRight")
                        importKeyLine(spl[1].Trim(), keyMapDownRight);
                    else if (spl[0] == "Click")
                        importKeyLine(spl[1].Trim(), keyMapClick);
                    else if (spl[0] == "DoubleClick")
                        importKeyLine(spl[1].Trim(), keyMapDoubleClick);
                    else if (spl[0] == "ZoomIn")
                        importKeyLine(spl[1].Trim(), keyMapZoomIn);
                    else if (spl[0] == "ZoomOut")
                        importKeyLine(spl[1].Trim(), keyMapZoomOut);
                    else if (spl[0] == "EnterURL")
                        importKeyLine(spl[1].Trim(), keyMapNavigate);
                    else if (spl[0] == "Magnifier")
                        importKeyLine(spl[1].Trim(), keyMapMagnifier);
                    else if (spl[0] == "CloseWindow")
                        importKeyLine(spl[1].Trim(), keyMapClose);
                    else if (spl[0] == "ShowKeyboard")
                        importKeyLine(spl[1].Trim(), keyMapKeyboard);
                    else if (spl[0] == "ShowFavourites")
                        importKeyLine(spl[1].Trim(), keyMapFavourites);
                    else if (spl[0] == "ShowShortcuts")
                        importKeyLine(spl[1].Trim(), keyMapShortCuts);
                    else if (spl[0] == "PressTAB")
                        importKeyLine(spl[1].Trim(), keyMapTAB);
                    else if (spl[0] == "PressESC")
                        importKeyLine(spl[1].Trim(), keyMapESC);
                    else if (spl[0] == "ToggleMouse")
                        importKeyLine(spl[1].Trim(), keyMapToggleMouse);
                    else if (spl[0] == "PressF5")
                        importKeyLine(spl[1].Trim(), keyMapF5);
                    else if (spl[0] == "ShowContextMenu")
                        importKeyLine(spl[1].Trim(), keyMapContextMenu);

                }
            }
            str.Close();

            saveMappingList();
        }

        private void saveMappingList()
        {
            _allKeys = new ArrayList();
            foreach (Keys key in keyMapUp) _allKeys.Add(key);
            foreach (Keys key in keyMapDown) _allKeys.Add(key);
            foreach (Keys key in keyMapLeft) _allKeys.Add(key);
            foreach (Keys key in keyMapRight) _allKeys.Add(key);
            foreach (Keys key in keyMapUpLeft) _allKeys.Add(key);
            foreach (Keys key in keyMapUpRight) _allKeys.Add(key);
            foreach (Keys key in keyMapDownLeft) _allKeys.Add(key);
            foreach (Keys key in keyMapDownRight) _allKeys.Add(key);
            foreach (Keys key in keyMapClose) _allKeys.Add(key);
            foreach (Keys key in keyMapMagnifier) _allKeys.Add(key);
            foreach (Keys key in keyMapNavigate) _allKeys.Add(key);
            foreach (Keys key in keyMapZoomIn) _allKeys.Add(key);
            foreach (Keys key in keyMapZoomOut) _allKeys.Add(key);
            foreach (Keys key in keyMapClick) _allKeys.Add(key);
            foreach (Keys key in keyMapDoubleClick) _allKeys.Add(key);
            foreach (Keys key in keyMapKeyboard) _allKeys.Add(key);
            foreach (Keys key in keyMapFavourites) _allKeys.Add(key);
            foreach (Keys key in keyMapShortCuts) _allKeys.Add(key);
            foreach (Keys key in keyMapTAB) _allKeys.Add(key);
            foreach (Keys key in keyMapESC) _allKeys.Add(key);
            foreach (Keys key in keyMapToggleMouse) _allKeys.Add(key);
            foreach (Keys key in keyMapContextMenu) _allKeys.Add(key);
            foreach (Keys key in keyMapF5) _allKeys.Add(key);
            foreach (Keys key in keyMapDelete) _allKeys.Add(key);
        }

        public ArrayList AllKeys { get { return _allKeys; } }
    }
}
