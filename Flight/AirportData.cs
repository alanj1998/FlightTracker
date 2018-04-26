using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightTracker
{
    /// <summary>
    /// Class used to deserialise Airport Data
    /// </summary>
    class AirportData
    {
        [JsonProperty("icao")]
        public string ICAO { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string CountryCode { get; set; }
    }
}
