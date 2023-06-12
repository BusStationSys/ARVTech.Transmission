using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    [Serializable]
    public class Result
    {
        public Result()
        { }

        public Result(string line)
        {
            var split = line.Split('|');

            LineName = split[0];
            SP = Convert.ToInt32(split[1]);
            CMP = Convert.ToInt32(split[2]);
            Latitude = Convert.ToDecimal(split[3].ToString(CultureInfo.InvariantCulture).Replace(".", ","));
            Longitude = Convert.ToDecimal(split[4].ToString(CultureInfo.InvariantCulture).Replace(".", ","));
            Cruisename = split[5];
        }

        public string LineName { get; set; }

        public int SP { get; set; }

        public int CMP { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Cruisename { get; set; }
    }
}