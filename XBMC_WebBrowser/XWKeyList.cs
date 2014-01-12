using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        public void Add(Keys item)
		{
            if (!base.InnerList.Contains(item))
                base.InnerList.Add(item);
		}

        public void Remove(Keys item)
        {
            base.InnerList.Remove(item);
        }

        public bool Contains(Keys item)
        {
            foreach (Keys s in base.InnerList)
            {
                if (s == item)
                    return true;
            }
            return false;
        }

        public Keys this[int index]
		{
			get
			{
                return (Keys)base.InnerList[index];

			}
		}

    }
}
