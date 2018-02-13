using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    class AirportLookUp
    {
        private Dictionary<string, AirportData> airportData = JSON.GetJSONData<Dictionary<string, AirportData>>(AppPaths.Path.Airports);

        public ObservableCollection<Hint> GetData(string choice, string query)
        {
            ObservableCollection<Hint> hints = new ObservableCollection<Hint>();
            Dictionary<string, AirportData> airportDictionary = JSON.GetJSONData<Dictionary<string, AirportData>>(AppPaths.Path.Airports);
            List<AirportData> list = new List<AirportData>();

            if (choice == "airportID" && query.Length <= 4)
                list = airportDictionary.Where(x => x.Key.ToUpper().Contains(query.ToUpper())).Select(x => x.Value).ToList();
            else if(choice == "airportName")
                list = airportDictionary.Where(x => x.Value.City.ToUpper().Contains(query.ToUpper())).Select(x => x.Value).ToList();

            foreach (AirportData ad in list)
            {
                Hint hint = new Hint(ad.City.Trim(new char[] { ' ' }), ad.ICAO, $"Assets/Flags/{ad.CountryCode.ToLower()}.png");
                hints.Add(hint);
            }

            return hints;

        }
    }
}
