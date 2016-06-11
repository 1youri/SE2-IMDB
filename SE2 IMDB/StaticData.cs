using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE2_IMDB
{
    public static class StaticData
    {
        public static string Message { get; set; }

        public static string GetAndResetMessage()
        {
            string temp = "" + Message + "";
            Message = "";
            return temp;
        }
    }
}