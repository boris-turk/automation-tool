using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace BTurk.Automation.Core.Serialization
{
    public class JsonResourceProvider : IResourceProvider
    {
        public void Save(object instance, string filePath)
        {
            using (var file = File.CreateText(filePath))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };
                serializer.Serialize(file, instance);
            }
        }

        public T Load<T>(string resourceName)
        {
            var filePath = Path.Combine("Configuration", resourceName);

            if (!filePath.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
                filePath = $"{filePath}.json";

            if (!File.Exists(filePath))
                throw new InvalidOperationException($"Missing file: {filePath}");

            using (var sourceStream = new FileStream(filePath, FileMode.Open))
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                var result = Deserialize<T>(memoryStream);
                return result;
            }
        }

        private T Deserialize<T>(MemoryStream memoryStream)
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

        private T FromJsonString<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}