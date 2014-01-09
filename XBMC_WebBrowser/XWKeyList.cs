using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBMC_WebBrowser
{
    public class XWKeyList : System.Collections.CollectionBase
    {
        #region Singleton

        private static XWKeyList _instance = null;

        public static XWKeyList getInstance()
        {
            if (_instance == null) _instance = new XWKeyList();
            return _instance;
        }

        #endregion

        public XWKeyList()
		{
		}

        public void Add(String item)
		{
            if (!base.InnerList.Contains(item))
                base.InnerList.Add(item);
		}

        public void Remove(String item)
        {
            base.InnerList.Remove(item);
        }

        public bool Contains(String item)
        {
            foreach (String s in base.InnerList)
            {
                if (s == item)
                    return true;
            }
            return false;
        }

        public String this[int index]
		{
			get
			{
                return (String)base.InnerList[index];

			}
		}

    }
}
