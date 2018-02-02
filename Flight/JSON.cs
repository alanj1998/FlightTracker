using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace FlightTracker
{
    static class JSON
    {
        public static dynamic GetJSONData<T>(string path)
        {
            string data = "";
            using (StreamReader r = new StreamReader($"../../{path}"))
            {
                data = r.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void UpdateJSON<TKey,TVal>(TKey key, TVal value, string path)
        {
            Dictionary<TKey,TVal> data = GetJSONData<Dictionary<TKey,TVal>>(path);
            data[key] = value;
            WriteToJSONData(data, path);
        }

        public static void WriteToJSONData(object data, string path)
        {
            using (StreamWriter sw = new StreamWriter($"../../{path}"))
                sw.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
