using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE2_IMDB
{
    public static class Extensions
    {
        public static int ToInt(this object input)
        {
            int output;
            if (int.TryParse(input.ToString(), out output)) return output;
            else return -1;
        }
    }
}