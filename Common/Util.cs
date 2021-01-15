using System;
using System.Collections.Generic;
using System.Text;

namespace CPUClocker.Common
{
    public class Util
    {
        public static string TrimInfluxColumn(string text)
        {
            return text.ToLower().Replace(' ', '_').Replace("#", String.Empty);
        }

        public static string TrimInfluxColumn(string name, string sensorName)
        {
            return TrimInfluxColumn(name + " " + sensorName);
        }
    }
}
