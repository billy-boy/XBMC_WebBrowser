using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public XWKeyList keyMapUp = new XWKeyList(){"NumPad8"},
                        keyMapDown = new XWKeyList(){"NumPad2"},
                        keyMapLeft = new XWKeyList(){"NumPad4"},
                        keyMapRight = new XWKeyList(){"NumPad6"},
                        keyMapUpLeft = new XWKeyList(){"NumPad7"},
                        keyMapUpRight = new XWKeyList(){"NumPad9"},
                        keyMapDownLeft = new XWKeyList(){"NumPad1"},
                        keyMapDownRight = new XWKeyList(){"NumPad3"},
                        keyMapClick = new XWKeyList(){"NumPad5"},
                        keyMapDoubleClick = new XWKeyList(){},
                        keyMapZoomIn = new XWKeyList(){"Add"},
                        keyMapZoomOut = new XWKeyList(){"Subtract"},
                        keyMapMagnifier = new XWKeyList() { "Menu, Alt" },
                        keyMapNavigate = new XWKeyList(){},
                        keyMapClose = new XWKeyList(){"NumPad0"},
                        keyMapKeyboard = new XWKeyList(){},
                        keyMapFavourites = new XWKeyList(){},
                        keyMapShortCuts = new XWKeyList(){},
                        keyMapTAB = new XWKeyList(){},
                        keyMapESC = new XWKeyList(){},
                        keyMapToggleMouse = new XWKeyList(){"Multiply"},
                        keyMapContextMenu = new XWKeyList(){"Divide"},
                        keyMapF5 = new XWKeyList(){},
                        keyMapDelete = new XWKeyList(){"Decimal"};

        public XWKeys()
        {
            saveMappingList();
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
                        keyMapUp.Add(spl[1].Trim());
                    else if (spl[0] == "Down")
                        keyMapDown.Add(spl[1].Trim());
                    else if (spl[0] == "Left")
                        keyMapLeft.Add(spl[1].Trim());
                    else if (spl[0] == "Right")
                        keyMapRight.Add(spl[1].Trim());
                    else if (spl[0] == "UpLeft")
                        keyMapUpLeft.Add(spl[1].Trim());
                    else if (spl[0] == "UpRight")
                        keyMapUpRight.Add(spl[1].Trim());
                    else if (spl[0] == "DownLeft")
                        keyMapDownLeft.Add(spl[1].Trim());
                    else if (spl[0] == "DownRight")
                        keyMapDownRight.Add(spl[1].Trim());
                    else if (spl[0] == "Click")
                        keyMapClick.Add(spl[1].Trim());
                    else if (spl[0] == "DoubleClick")
                        keyMapDoubleClick.Add(spl[1].Trim());
                    else if (spl[0] == "ZoomIn")
                        keyMapZoomIn.Add(spl[1].Trim());
                    else if (spl[0] == "ZoomOut")
                        keyMapZoomOut.Add(spl[1].Trim());
                    else if (spl[0] == "EnterURL")
                        keyMapNavigate.Add(spl[1].Trim());
                    else if (spl[0] == "Magnifier")
                        keyMapMagnifier.Add(spl[1].Trim());
                    else if (spl[0] == "CloseWindow")
                        keyMapClose.Add(spl[1].Trim());
                    else if (spl[0] == "ShowKeyboard")
                        keyMapKeyboard.Add(spl[1].Trim());
                    else if (spl[0] == "ShowFavourites")
                        keyMapFavourites.Add(spl[1].Trim());
                    else if (spl[0] == "ShowShortcuts")
                        keyMapShortCuts.Add(spl[1].Trim());
                    else if (spl[0] == "PressTAB")
                        keyMapTAB.Add(spl[1].Trim());
                    else if (spl[0] == "PressESC")
                        keyMapESC.Add(spl[1].Trim());
                    else if (spl[0] == "ToggleMouse")
                        keyMapToggleMouse.Add(spl[1].Trim());
                    else if (spl[0] == "PressF5")
                        keyMapF5.Add(spl[1].Trim());
                    else if (spl[0] == "ShowContextMenu")
                        keyMapContextMenu.Add(spl[1].Trim());

                }
            }
            str.Close();

            saveMappingList();
        }

        private void saveMappingList()
        {
            _allKeys = new ArrayList();
            foreach (String key in keyMapUp) _allKeys.Add(key);
            foreach (String key in keyMapDown) _allKeys.Add(key);
            foreach (String key in keyMapLeft) _allKeys.Add(key);
            foreach (String key in keyMapRight) _allKeys.Add(key);
            foreach (String key in keyMapUpLeft) _allKeys.Add(key);
            foreach (String key in keyMapUpRight) _allKeys.Add(key);
            foreach (String key in keyMapDownLeft) _allKeys.Add(key);
            foreach (String key in keyMapDownRight) _allKeys.Add(key);
            foreach (String key in keyMapClose) _allKeys.Add(key);
            foreach (String key in keyMapMagnifier) _allKeys.Add(key);
            foreach (String key in keyMapNavigate) _allKeys.Add(key);
            foreach (String key in keyMapZoomIn) _allKeys.Add(key);
            foreach (String key in keyMapZoomOut) _allKeys.Add(key);
            foreach (String key in keyMapClick) _allKeys.Add(key);
            foreach (String key in keyMapDoubleClick) _allKeys.Add(key);
            foreach (String key in keyMapKeyboard) _allKeys.Add(key);
            foreach (String key in keyMapFavourites) _allKeys.Add(key);
            foreach (String key in keyMapShortCuts) _allKeys.Add(key);
            foreach (String key in keyMapTAB) _allKeys.Add(key);
            foreach (String key in keyMapESC) _allKeys.Add(key);
            foreach (String key in keyMapToggleMouse) _allKeys.Add(key);
            foreach (String key in keyMapContextMenu) _allKeys.Add(key);
            foreach (String key in keyMapF5) _allKeys.Add(key);
            foreach (String key in keyMapDelete) _allKeys.Add(key);
        }

        public ArrayList AllKeys { get { return _allKeys; } }
    }
}
