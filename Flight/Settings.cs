using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;

namespace FlightTracker
{
    static class UserSettings
    {
        private static string language,
                              timeType;
        private const string PATH_TO_SETTINGS = "Assets/UserSettings.json";

        public static string Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
            }
        }

        public static string Time
        {
            get
            {
                return timeType;
            }
            set
            {
                timeType = value;
            }
        }

        public static void SetUserSettings()
        {
            Dictionary<string, string> data = JSON.GetJSONData<Dictionary<string, string>>(UserSettings.PATH_TO_SETTINGS);
            language = data["Language"].ToString();
            timeType = data["Time"].ToString();
        }

        public static bool UpdateUserSettings(string value, string setting)
        {
            if (typeof(UserSettings).GetProperties().Select(x => x.Name.ToUpper()).Contains(setting.ToUpper()))
            {
                if (setting.ToUpper() == "LANGUAGE")
                {
                    Language = value;
                }
                else
                {
                    Time = value;
                }
                JSON.UpdateJSON(setting, value, UserSettings.PATH_TO_SETTINGS);
                return true;
            }
            else
                return false;
        }
    }

    enum Settings
    {
        Language, Time
    }

    static class AppPaths
    {
        private static Paths path;

        static AppPaths()
        {
            path = JSON.GetJSONData<Paths>("Assets/Paths.json");
        }

        public static Paths Path
        {
            get
            {
                return path;
            }
        }
    }

    class Paths
    {
        private string languageList;
        private Dictionary<string, string> languages;
        private Dictionary<string, string> images;

        public string LanguageList
        {
            get
            {
                return this.languageList;
            }
        }

        public Dictionary<string, string> Languages
        {
            get
            {
                return this.languages;
            }
        }

        public Dictionary<string, string> Images
        {
            get
            {
                return this.images;
            }
        }

        public Paths(string languageList, Dictionary<string,string> languages, Dictionary<string, string> images)
        {
            this.languageList = languageList;
            this.languages = languages;
            this.images = images;
        }
    }

    static class AppSettings
    {
        public static void SetResourceFile(string Language)
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            ResourceDictionary lang = new ResourceDictionary();
            lang.Source = new Uri(AppPaths.Path.Languages[Language], UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(lang);
        }
    }
}
