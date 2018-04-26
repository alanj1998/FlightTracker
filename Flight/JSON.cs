using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace FlightTracker
{
    /// <summary>
    /// JSON class used to get and post JSON data into a file.
    /// Methods: 
    /// GetJsonData(<typeparamref name="string"/> path)
    /// UpdateJson(<typeparamref name="TKey"/> key, <typeparamref name="TVal"/> value, <typeparamref name="string"/> path)    
    /// </summary>
    class JSON
    {
        /// <summary>
        /// Method used to get JSON data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static dynamic GetJSONData<T>(string path)

        {
            string data = "";
            using (StreamReader r = new StreamReader($"../../{path}"))
            {
                data = r.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Method used to update JSON data
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="path"></param>
        public static void UpdateJSON<TKey, TVal>(TKey key, TVal value, string path)
        {
            Dictionary<TKey, TVal> data = GetJSONData<Dictionary<TKey, TVal>>(path);
            data[key] = value;
            WriteToJSONData(data, path);
        }

        /// <summary>
        /// Write to JSON file
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void WriteToJSONData(object data, string path)
        {
            using (StreamWriter sw = new StreamWriter($"../../{path}"))
                sw.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
