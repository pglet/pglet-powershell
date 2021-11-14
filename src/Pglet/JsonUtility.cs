using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet
{
    public class JsonUtility
    {
        public static T Deserialize<T>(string json)
        {
            if (String.IsNullOrWhiteSpace(json))
                return default;

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Deserialize<T>(JObject jo)
        {
            if (jo == null)
                return default;

            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<T>(new JTokenReader(jo));
        }

        public static string Serialize(object obj, bool camelizeProperties = true, bool indented = false)
        {
            if (obj == null)
                return null;

            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }

            if (camelizeProperties)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
            }
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
