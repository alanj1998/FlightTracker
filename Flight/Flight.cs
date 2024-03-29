﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    /// <summary>
    /// Class used to hold Flight Information which is then displayed in the FlightsWindow
    /// </summary>
    public class Flight
    {
        private DateTime dateAndTime;
        private string flightCode;
        private Uri pathToLogo;
        private string town;


        public DateTime Time
        {
            get
            {
                return this.dateAndTime;
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

        public Flight(string time, string date, string flightCode, string town, Uri logo = null)
        {
            time.Replace(" ", "");
            this.dateAndTime = DateTime.Parse(date + " " + time);
            this.flightCode = flightCode;
            this.town = town;

            //If no logo is provided to a flight, set a default logo
            if (logo == null)
                this.pathToLogo = new Uri(AppPaths.Path.Images["Airline Name"], UriKind.Relative);
            else
                this.pathToLogo = logo;
        }
    }
}
