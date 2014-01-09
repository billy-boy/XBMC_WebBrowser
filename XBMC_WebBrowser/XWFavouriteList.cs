using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XBMC_WebBrowser
{
    public class XWFavouriteList : System.Collections.CollectionBase
    {
        #region Singleton

        private static XWFavouriteList _instance = null;

        public static XWFavouriteList getInstance()
        {
            if (_instance == null) _instance = new XWFavouriteList();
            return _instance;
        }

        #endregion

        public XWFavouriteList()
		{
		}

        public void Add(XWFavourite item)
		{
            base.InnerList.Add(item);
		}

        public void Remove(XWFavourite item)
        {
            base.InnerList.Remove(item);
        }

        public XWFavourite this[int index]
		{
			get
			{
                return (XWFavourite)base.InnerList[index];

			}
		}

        public XWFavourite this[String title]
		{
			get
			{
                XWFavourite item = null;

                foreach (XWFavourite search in base.InnerList)
				{
					if (search.Title ==title)
					{
                        item = search;
						break;
					}
				}

                return item;
			}
		}

        public void loadFavourites(String userDataFolder)
        {
            base.InnerList.Clear();
            if (Directory.Exists(userDataFolder))
            {
                DirectoryInfo dir = new DirectoryInfo(userDataFolder + "\\sites");
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.FullName.EndsWith(".link"))
                    {
                        StreamReader str = new StreamReader(file.FullName);
                        String line;
                        String title = String.Empty;
                        String url = String.Empty;
                        Int32 zoom = 0;
                        Boolean showPopups = false;
                        Boolean showScrollBar = true;
                        String userAgent = "";
                        while ((line = str.ReadLine()) != null)
                        {
                            if (line.Contains("="))
                            {
                                String entry = line.Substring(0, line.IndexOf("="));
                                String content = line.Substring(line.IndexOf("=")+1);
                                if (entry == "title")
                                    title = content.Trim();
                                else if (entry == "url")
                                    url = content.Trim();
                                else if (entry == "zoom")
                                    zoom = Convert.ToInt32(content.Trim());
                                else if (entry == "showPopups")
                                    showPopups = (content.Trim() == "yes");
                                else if (entry == "showScrollbar")
                                    showScrollBar = (content.Trim() == "yes");
                                else if (entry == "userAgent")
                                    userAgent = content.Trim();
                            }
                        }
                        str.Close();
                        if (title != String.Empty && url != String.Empty)
                        {
                            XWFavourite favorit = new XWFavourite(title, url);
                            favorit.ShowPopups = showPopups;
                            favorit.ShowScrollbar = showScrollBar;
                            favorit.Zoom = zoom;
                            favorit.UserAgent = userAgent;
                            base.InnerList.Add(favorit);
                        }
                    }
                }
        }
    }

    }
}
