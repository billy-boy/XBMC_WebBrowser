using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XBMC_WebBrowser
{
    public class XWShortcut
    {
        private String _title = "";
        private String _url = "";
        private String _mainURL = "";

        public XWShortcut(String title, String mainURL, String url)
        {
            _title = title;
            _mainURL = mainURL;
            _url = url;
        }

        public String Title { get { return _title; } }
        public String URL { get { return _url; } }
        public String mainURL { get { return _mainURL; } }

        public void Save(String folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            File.AppendAllText(folder + "\\"+ _mainURL + ".links", "\r\n"+_title + "=" +_url + "\r\n");
        }

        public void Delete(String folder)
        {
            if (!Directory.Exists(folder))
                return;
            if (!File.Exists(folder + "\\" + _mainURL + ".links"))
                return;
            String[] lines = File.ReadAllLines(folder + "\\" + _mainURL + ".links");
            List<String> newLines = new List<String>();
            foreach (String line in lines)
            {
                if (line.Replace("\r", "").Replace("\n", "").Trim() != ""
                    && line.Replace("\r", "").Replace("\n", "").Trim() != _title + "=" + _url)
                {
                    newLines.Add(line);
                }
            }
            File.WriteAllLines(folder + "\\" + _mainURL + ".links", newLines);
        }
    }
}
