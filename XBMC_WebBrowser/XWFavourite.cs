using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XBMC_WebBrowser
{
    public class XWFavourite
    {
        private String _title = "";
        private String _url = "";
        private Int32 _zoom = 0;
        private Boolean _showPopups = false;
        private Boolean _showScrollbar = true;
        private String _userAgent = "";

        public XWFavourite(String title, String url)
        {
            _title = title;
            _url = url;
        }

        public String Title { get { return _title; } }
        public String URL { get { return _url; } }
        public Int32 Zoom { get { return _zoom; } set { _zoom = value; } }
        public Boolean ShowPopups { get { return _showPopups; } set { _showPopups = value; } }
        public Boolean ShowScrollbar { get { return _showScrollbar; } set { _showScrollbar = value; } }
        public String UserAgent { get { return _userAgent; } set { _userAgent = value; } }

        public void Save(String folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            File.AppendAllText(folder + "\\" + _title + ".link", "title="+_title+"\r\n"+"url="+ _url + "\r\n");
        }
        public void Delete(String folder)
        {
             if (!Directory.Exists(folder))
                return;
             File.Delete(folder + "\\" + _title + ".link");
        }
    }
}
