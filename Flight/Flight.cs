using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    public class Flight
    {
        private DateTime time;
        private string airlineCode;
        private Uri pathToLogo;
        private string town;

        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }
        public string AirlineCode
        {
            get
            {
                return this.airlineCode;
            }
        }
        public Uri PathToLogo
        {
            get
            {
                return this.pathToLogo;
            }
        }
        public string Town
        {
            get
            {
                return this.town;
            }
        }

        public Flight(DateTime time, string airlineCode, string town, Uri logo = null)
        {
            this.time = time;
            this.airlineCode = airlineCode;
            this.town = town;

            if (logo == null)
                this.pathToLogo = new Uri("Assets/Images/airlineLogo.png", UriKind.Relative);
            else
                this.pathToLogo = logo;
        }
    }
}
