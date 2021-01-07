using Newtonsoft.Json.Converters;

namespace AutomationEngine.RestApi
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}