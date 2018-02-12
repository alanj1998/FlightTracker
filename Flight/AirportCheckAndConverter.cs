using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    class AirportCheckAndConverter
    {
        private string data;
        private string choice;
        private Dictionary<string, AirportData> airportData = JSON.GetJSONData<Dictionary<string, AirportData>>(AppPaths.Path.Airports);

        public AirportCheckAndConverter(string data, string choice)
        {
            this.data = data;
            this.choice = choice;
        }

        public string ReturnICAOCode()
        {
            string ICAO = "";

            if (choice == "airportID")
                ICAO = CheckICAO();
            else
                ICAO = ConvertFromName();

            return ICAO;     
        }

        public string ConvertFromName()
        {
            bool isAValidName = airportData.Any(x => x.Value.Name.ToUpper() == this.data.ToUpper() || x.Value.City.ToUpper() == this.data.ToUpper());

            if (isAValidName)
            {
                KeyValuePair<string, AirportData> index = airportData.FirstOrDefault(x => x.Value.Name.ToUpper() == this.data.ToUpper() || x.Value.City.ToUpper() == this.data.ToUpper());
                return index.Value.ICAO;
            }
            else
                return "";
        }

        public string CheckICAO()
        {
            if(this.data.Length == 4)
            {
                bool isValid = airportData.Any(x => x.Value.ICAO == this.data.ToUpper());

                if (isValid)
                    return airportData.FirstOrDefault(x => x.Value.ICAO == this.data).Value.ICAO;
                else
                    return "";                    
            }
            return "";
        }
    }
}
