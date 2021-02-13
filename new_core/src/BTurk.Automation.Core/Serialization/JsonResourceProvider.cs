using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// ReSharper disable ConvertToUsingDeclaration

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
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new ContractResolver()
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
            var bytes = memoryStream.ToArray();
            var text = Encoding.UTF8.GetString(bytes);
            var result = FromJsonString<T>(text);
            return result;
        }

        private T FromJsonString<T>(string content)
        {
            var settings = CreateSerializationSettings();
            var result = JsonConvert.DeserializeObject<T>(content, settings);
            return result;
        }

        private static JsonSerializerSettings CreateSerializationSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" });
            settings.Converters.Add(new TimeSpanConverter());

            return settings;
        }
    }
}