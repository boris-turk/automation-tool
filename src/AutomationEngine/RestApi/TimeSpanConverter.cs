using System;
using System.Xml;
using Newtonsoft.Json;

namespace AutomationEngine.RestApi
{
    public class TimeSpanConverter : Newtonsoft.Json.JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var timeSpan = (TimeSpan)value;
            var text = XmlConvert.ToString(timeSpan);
            serializer.Serialize(writer, text);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var value = serializer.Deserialize<string>(reader);

            if (value == null)
                return null;

            var timeSpan = XmlConvert.ToTimeSpan(value);

            return timeSpan;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }
    }
}