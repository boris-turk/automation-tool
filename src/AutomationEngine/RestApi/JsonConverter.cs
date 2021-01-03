using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

namespace AutomationEngine.RestApi
{
    public static class JsonConverter
    {
        public static string ToJsonString(object value)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(value, Formatting.None, settings);
        }

        public static T FromJsonString<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
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

            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var result = (T)serializer.ReadObject(memoryStream);
                return result;
            }
            catch (Exception e)
            {
                memoryStream.Position = 0;
                var responseString = Encoding.UTF8.GetString(memoryStream.ToArray());

                try
                {
                    var result = FromJsonString<T>(responseString);
                    return result;
                }
                catch (Exception ex)
                {
                    e.Data.Add("newtonsoft_json_converter_exception", ex.Message);
                }

                e.Data.Add("response_string", responseString);
                throw;
            }
        }
    }

}