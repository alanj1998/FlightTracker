using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;

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

        public LoadFlightData(string code, string airline = null)
        {
            SplitData(GetData(code, airline));            
            //     AirportData data = JSON.GetJSONData<AirportData>("Assets/testData.json");
        }

        private void SplitData(AirportData data)
        {
            AddToLists(data.boardsResult.Arrivals.Flights, this.arrivals);
            AddToLists(data.boardsResult.EnRoute.Flights, this.arrivals);
            AddToLists(data.boardsResult.Departures.Flights, this.departures);
            AddToLists(data.boardsResult.Scheduled.Flights, this.departures);
            this.info = data.boardsResult.Info;
        }

        private void AddToLists<T>(List<T> data, List<FlightDetails> listToAddTo)
        {
            foreach (T f in data)
            {
                string t = "",
                       date = "",
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
                        date = arrival.ArrivalTime.Date;
                        airlineCode = arrival.AirlineCode;
                        town = SplitTownName(arrival.Origin.City);
                        flightCode = arrival.FlightID;
                    }
                    else if (f.GetType() == typeof(DepartureFlights))
                    {
                        DepartureFlights departure = f as DepartureFlights;
                        t = departure.DepartureTime.Time;
                        date = departure.DepartureTime.Date;
                        airlineCode = departure.AirlineCode;
                        town = SplitTownName(departure.Destination.City);
                        flightCode = departure.FlightID;
                    }
                }
                else if (typeOfFlight.Cancelled == true || typeOfFlight.ProgressPercentage == -1)
                {
                    if (f.GetType() == typeof(ArrivalFlights))
                    {
                        ArrivalFlights arrival = f as ArrivalFlights;
                        t = arrival.EstimatedArrivalTime.Time;
                        date = arrival.EstimatedArrivalTime.Date;
                        airlineCode = arrival.AirlineCode;
                        town = SplitTownName(arrival.Origin.City);
                        flightCode = arrival.FlightID;
                    }
                    else if (f.GetType() == typeof(DepartureFlights))
                    {
                        DepartureFlights departure = f as DepartureFlights;
                        t = departure.EstimatedDepartureTime.Time;
                        date = departure.EstimatedDepartureTime.Date;
                        airlineCode = departure.AirlineCode;
                        town = SplitTownName(departure.Destination.City);
                        flightCode = departure.FlightID;
                    }
                }

                if (t != "" && town != "" && flightCode != "" && date != "")
                {
                    Flight flight = new Flight(t, date, airlineCode + flightCode, town);
                    FlightDetails detail = new FlightDetails(flight);
                    listToAddTo.Add(detail);
                }
                else continue;
            }
        }

        private string SplitTownName(string town)
        {
            if (town.Contains(','))
                return town.Split(',')[0];
            else
                return town;
        }

        private AirportData GetData(string airport, string airline)//Dictionary<string, string> data)
        {
            AirportData data = new AirportData();

            Uri url = SetURL(airport, (airline != null) ? airline : null);
            string auth = SetAuthorization();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.Authorization, $"Basic {auth}");
            request.ContentType = "text/json";
            request.Method = "GET";

            string jsonResponse = "";

            using (WebResponse response = request.GetResponse())
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                jsonResponse = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<AirportData>(jsonResponse);
        }

        private Uri SetURL(string airport, string airline = null)
        {
            const string URL_STRING = "http://flightxml.flightaware.com/json/FlightXML3/AirportBoards";

            UriBuilder uriBuilder = new UriBuilder(URL_STRING);
            uriBuilder.Query += $"airport_code={airport}&filter=airline";
            Uri url = uriBuilder.Uri;

            return url;
        }

        private string SetAuthorization()
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1"); //set base64 authorization
            byte[] encoded = iso.GetBytes("alanj1998:4d5e8497d3ecc47709fce1db7980e4ebcb5fe740");

            return Convert.ToBase64String(encoded);
        }
    }
}
