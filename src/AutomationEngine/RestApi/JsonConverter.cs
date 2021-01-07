using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace AutomationEngine.RestApi
{
    public static class JsonConverter
    {
        public static string ToJsonString(object value)
        {
            var settings = CreateSerializationSettings();
            settings.ContractResolver = new ContractResolver();
            return JsonConvert.SerializeObject(value, Formatting.None, settings);
        }

        public static T FromJsonString<T>(string content)
        {
            var settings = CreateSerializationSettings();
            var result = JsonConvert.DeserializeObject<T>(content, settings);
            return result;
        }

        public static T DeserializeJson<T>(Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                var result = DeserializeJson<T>(memoryStream);
                return result;
            }
        }

        private static T DeserializeJson<T>(MemoryStream memoryStream)
        {
            memoryStream.Position = 0;
            var bytes = memoryStream.ToArray();
            var text = Encoding.UTF8.GetString(bytes);
            var result = FromJsonString<T>(text);
            return result;
        }

        private static JsonSerializerSettings CreateSerializationSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            settings.Converters.Add(new DateFormatConverter("yyyy-MM-ddTHH:mm:ss.fffZ"));
            settings.Converters.Add(new TimeSpanConverter());

            return settings;
        }
    }
}