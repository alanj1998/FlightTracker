using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    class Hint
    {
        public string TownName { get; set; }
        public string ICAOCode { get; set; }
        public string PathToImage { get; set; }

        public Hint(string TownName, string ICAOCode, string PathToImage)
        {
            this.TownName = TownName;
            this.ICAOCode = ICAOCode;
            this.PathToImage = PathToImage;
        }
    }
}
