using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    public class Flight
    {
        private string time;
        private string flightCode;
        private Uri pathToLogo;
        private string town;

        public string Time
        {
            get
            {
                return this.time;
            }
        }
        public string FlightCode
        {
            get
            {
                return this.flightCode;
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

        public Flight(string time, string flightCode, string town, Uri logo = null)
        {
            time.Replace(" ", "");
            this.time = DateTime.Parse(time).ToString("HH:mm");
            this.flightCode = flightCode;
            this.town = town;

            if (logo == null)
                this.pathToLogo = new Uri(AppPaths.Path.Images["Airline Name"], UriKind.Relative);
            else
                this.pathToLogo = logo;
        }
    }
}
