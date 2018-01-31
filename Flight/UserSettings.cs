using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTracker
{
    class UserSettings
    {
        private string language,
                       timeType;
        private const string PATH_TO_SETTINGS = "Assets/UserSettings.json";

        public string Language
        {
            get
            {
                return this.language;
            }
            set
            {
                UpdateUserSettings(value, "language");
            }
        }

        public string Time
        {
            get
            {
                return this.timeType;
            }
            set
            {
                UpdateUserSettings(value, "timeType");
            }
        }

        public UserSettings()
        {
            Dictionary<string, string> data = JSON.GetJSONData<Dictionary<string, string>>(UserSettings.PATH_TO_SETTINGS);
            this.language = data["language"].ToString();
            this.timeType = data["timeType"].ToString();
        }

        public void UpdateUserSettings(string val, string settings)
        {
            this.language = val;
           // JSON.UpdateJSON(settings, val, UserSettings.PATH_TO_SETTINGS);
        }
    }
}
