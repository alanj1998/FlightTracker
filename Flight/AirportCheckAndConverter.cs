using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    /// <summary>
    /// Used to look up Airports when the search is being triggered
    /// 
    /// Methods: GetData(string choice, string query)
    /// </summary>
    class AirportLookUp
    {
        /// <summary>
        /// Dictionary used to hold airport data
        /// </summary>
        private Dictionary<string, AirportData> airportData = JSON.GetJSONData<Dictionary<string, AirportData>>(AppPaths.Path.Airports);

        /// <summary>
        /// Get Data method used to get airports from a JSON file. Utilises JSON class which
        /// loads the data and deserialises it into AirportData class
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ObservableCollection<Hint> GetData(string choice, string query)
        {
            //Collection of airports got when the airports are being read in
            ObservableCollection<Hint> hints = new ObservableCollection<Hint>();

            //Collection of airports is read in
            Dictionary<string, AirportData> airportDictionary = JSON.GetJSONData<Dictionary<string, AirportData>>(AppPaths.Path.Airports);

            //List of airports is declared
            List<AirportData> list = new List<AirportData>();

            //If the choice was airportID and there was more than 4 letters enetered
            if (choice == "airportID" && query.Length <= 4)
                list = airportDictionary.Where(x => x.Key.ToUpper().Contains(query.ToUpper())).Select(x => x.Value).ToList();

            //Else if the choice was to look by airport name
            else if(choice == "airportName")
                list = airportDictionary.Where(x => x.Value.City.ToUpper().Contains(query.ToUpper())).Select(x => x.Value).ToList();


            //Add each airport to the hints list
            foreach (AirportData ad in list)
            {
                Hint hint = new Hint(ad.City.Trim(new char[] { ' ' }), ad.ICAO, $"Assets/Flags/{ad.CountryCode.ToLower()}.png");
                hints.Add(hint);
            }

            return hints;
        }
    }
}
