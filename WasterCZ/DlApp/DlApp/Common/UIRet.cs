using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DlApp.Common
{
    public class UIRet<T>
    {
        public Boolean success { get; set; }
        public string msg { get; set; }
        public List<T> rs { get; set; }
    }
}