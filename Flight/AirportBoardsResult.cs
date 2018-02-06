using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightTracker
{
    class AirportData
    {
        [JsonProperty("AirportBoardsResult")]
        public FlightData boardsResult { get; set; }
    }

    class FlightData
    {
        [JsonProperty("airport")]
        public string Airport { get; set; }

        [JsonProperty("airport_info")]
        public AirportInfo Info { get; set; }

        [JsonProperty("arrivals")]
        public Arrivals Arrivals { get; set; }

        [JsonProperty("departures")]
        public Departures Departures { get; set; }

        [JsonProperty("enroute")]
        public EnRoute EnRoute { get; set; }

        [JsonProperty("scheduled")]
        public Scheduled Scheduled { get; set; }
    }

    class AirportInfo
    {
        [JsonProperty("airport_code")]
        public string AirportCode { get; set; }

        [JsonProperty("name")]
        public string AirportName { get; set; }

        [JsonProperty("elevation")]
        public double AirportElevation { get; set; }

        [JsonProperty("city")]
        public string AirportLocation { get; set; }

        [JsonProperty("timezone")]
        public string AirportTimeZone { get; set; }

        [JsonProperty("country_code")]
        public string AirportCountryCode { get; set; }
    }

    abstract class FlightType<T>
    {
        [JsonProperty("flights")]
        public List<T> Flights { get; set; }
    }
    class Arrivals : FlightType<ArrivalFlights> { }
    class Departures : FlightType<DepartureFlights> { }
    class EnRoute : FlightType<ArrivalFlights> { }
    class Scheduled : FlightType<DepartureFlights> { }

    abstract class Flights
    {
        [JsonProperty("flightnumber")]
        public string FlightID { get; set; }

        [JsonProperty("airline_iata")]
        public string AirlineCode { get; set; }

        [JsonProperty("cancelled")]
        public bool Cancelled { get; set; }

        [JsonProperty("aircrafttype")]
        public string AircraftType { get; set; }

        [JsonProperty("progress_percent")]
        public double ProgressPercentage { get; set; }

    }
    class ArrivalFlights : Flights
    {
        [JsonProperty("origin")]
        public Origin Origin { get; set; }

        [JsonProperty("actual_arrival_time")]
        public ArrivalTime ArrivalTime { get; set; }

        [JsonProperty("estimated_arrival_time")]
        public ArrivalTime EstimatedArrivalTime { get; set; }
    }
    class DepartureFlights : Flights
    {
        [JsonProperty("destination")]
        public Destination Destination { get; set; }

        [JsonProperty("actual_departure_time")]
        public DepartureTime DepartureTime { get; set; }

        [JsonProperty("estimated_departure_time")]
        public ArrivalTime EstimatedDepartureTime { get; set; }
    }

    abstract class FlightPlace
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("airport_name")]
        public string AirportName { get; set; }
    }
    class Origin : FlightPlace { }
    class Destination: FlightPlace { }

    abstract class FlightTime
    {
        [JsonProperty("dow")]
        public string Day { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; } 

        [JsonProperty("date")]
        public string Date { get; set; }

    }
    class DepartureTime: FlightTime { }
    class ArrivalTime: FlightTime { }
}
