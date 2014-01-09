using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XBMC_WebBrowser
{
    public class XWShortcutList : System.Collections.CollectionBase
    {
        #region Singleton

        private static Dictionary<String,XWShortcutList> _instance = new Dictionary<string,XWShortcutList>();

        public static XWShortcutList getInstance(String mainURL)
        {
            if (!_instance.Keys.Contains(mainURL)) _instance[mainURL] = new XWShortcutList(mainURL);
            return _instance[mainURL];
        }

        #endregion

        private String _mainURL = "";

        public XWShortcutList(String mainURL)
        {
            _mainURL = mainURL;
        }

        public void Add(XWShortcut item)
        {
            base.InnerList.Add(item);
        }

        public void Remove(XWShortcut item)
        {
            base.InnerList.Remove(item);
        }

        public XWShortcut this[int index]
        {
            get
            {
                return (XWShortcut)base.InnerList[index];

            }
        }

        public XWShortcut this[String title]
        {
            get
            {
                XWShortcut item = null;

                foreach (XWShortcut search in base.InnerList)
                {
                    if (search.Title == title)
                    {
                        item = search;
                        break;
                    }
                }

                return item;
            }
        }

        public void loadShortcuts(String userDataFolder)
        {
            base.InnerList.Clear();
            if (Directory.Exists(userDataFolder))
            {
                if (File.Exists(userDataFolder + "\\shortcuts\\" + _mainURL + ".links"))
                {
                    FileInfo file = new FileInfo(userDataFolder + "\\shortcuts\\" + _mainURL + ".links");
                    if (file.FullName.EndsWith(".links"))
                    {
                        StreamReader str = new StreamReader(file.FullName);
                        String line;
                        String mainURL = file.Name.Replace(file.Extension,"");
                        String title = String.Empty;
                        String url = String.Empty;
                        while ((line = str.ReadLine()) != null)
                        {
                            if (line.Contains("="))
                            {
                                String entry = line.Substring(0, line.IndexOf("="));
                                String content = line.Substring(line.IndexOf("=") + 1);
                                title = entry.Trim();
                                url = content.Trim();
                                if (title != String.Empty && url != String.Empty)
                                {
                                    XWShortcut shortcut = new XWShortcut(title, mainURL, url);
                                    base.InnerList.Add(shortcut);
                                }
                            }
                        }
                        str.Close();
                    }
                }
            }
        }
    }
}