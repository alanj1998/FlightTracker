using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FlightTracker
{
    class LoadFlightData
    {
        private List<FlightDetails> arrivals = new List<FlightDetails>();
        private List<FlightDetails> departures = new List<FlightDetails>();
        private AirportInfo info;

        public List<FlightDetails> Arrivals
        {
            get
            {
                return this.arrivals;
            }
        }

        public List<FlightDetails> Departures
        {
            get
            {
                return this.departures;
            }
        }

        public AirportInfo AirportInfo
        {
            get
            {
                return this.info;
            }
        }

        public LoadFlightData()
        {
            //     AirportData data = JSON.GetJSONData<AirportData>("Assets/testData.json");
            AirportData data = GetData();
            //For testing purposes
            SplitData(data);
        }

        private void SplitData(AirportData data)
        {
            FlightData f = data.boardsResult;
            AddToLists(f.Arrivals.Flights, this.arrivals);
            AddToLists(f.EnRoute.Flights, this.arrivals);
            AddToLists(f.Departures.Flights, this.departures);
            AddToLists(f.Scheduled.Flights, this.departures);
            this.info = f.Info;
        }

        private void AddToLists<T>(List<T> data, List<FlightDetails> listToAddTo)
        {
            foreach(T f in data)
            {
                string t = "",
                       airlineCode = "",
                       town = "",
                       flightCode = "";
                Flights typeOfFlight = f as Flights;

                if (typeOfFlight.Cancelled == false && typeOfFlight.ProgressPercentage == 100)
                {
                    if (f.GetType() == typeof(ArrivalFlights))
                    {
                        ArrivalFlights arrival = f as ArrivalFlights;
                        t = arrival.ArrivalTime.Time;
                        airlineCode = arrival.AirlineCode;
                        town = SplitTownName(arrival.Origin.City);
                        flightCode = arrival.FlightID;
                    }
                    else if (f.GetType() == typeof(DepartureFlights))
                    {
                        DepartureFlights departure = f as DepartureFlights;
                        t = departure.DepartureTime.Time;
                        airlineCode = departure.AirlineCode;
                        town = SplitTownName(departure.Destination.City);
                        flightCode = departure.FlightID;
                    }
                }
                else if(typeOfFlight.Cancelled == true || typeOfFlight.ProgressPercentage == -1)
                {
                    if (f.GetType() == typeof(ArrivalFlights))
                    {
                        ArrivalFlights arrival = f as ArrivalFlights;
                        t = arrival.EstimatedArrivalTime.Time;
                        airlineCode = arrival.AirlineCode;
                        town = SplitTownName(arrival.Origin.City);
                        flightCode = arrival.FlightID;
                    }
                    else if (f.GetType() == typeof(DepartureFlights))
                    {
                        DepartureFlights departure = f as DepartureFlights;
                        t = departure.EstimatedDepartureTime.Time;
                        airlineCode = departure.AirlineCode;
                        town = SplitTownName(departure.Destination.City);
                        flightCode = departure.FlightID;
                    }
                }

                Flight flight = new Flight(t, flightCode, town);
                FlightDetails detail = new FlightDetails(flight);
                listToAddTo.Add(detail);
            }
        }

        private string SplitTownName(string town)
        {
            if (town.Contains(','))
                return town.Split(',')[0];
            else
                return town;
        }

        private AirportData GetData()//Dictionary<string, string> data)
        {
            AirportData results = new AirportData();

            const string URL_STRING = "http://http://flightxml.flightaware.com/json/FlightXML3/AirportBoards?";

            UriBuilder uriBuilder = new UriBuilder(URL_STRING);
            uriBuilder.Query += "airport_code=EIDW";
            uriBuilder.UserName = "alanj1998";
            uriBuilder.Password = "4d5e8497d3ecc47709fce1db7980e4ebcb5fe740";
            Uri url = uriBuilder.Uri;

            using (WebClient web = new WebClient())
            {
                string json = web.DownloadString(url);
                results = JsonConvert.DeserializeObject<AirportData>(json);
            }

            return results;
        }
    }
}
