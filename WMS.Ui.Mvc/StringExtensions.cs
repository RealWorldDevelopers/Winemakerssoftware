
using System.Globalization;

namespace WMS.Ui.Mvc
{
    public static class StringExtensions
    {
        public static string TruncateForDisplay(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var returnValue = value;
            if (value.Length > length)
            {
                var tmp = value.Substring(0, length);
                returnValue = tmp.Substring(0, tmp.LastIndexOf(' ')) + " ...";
            }
            return returnValue;
        }

        public static string FormatAsPercent(this double value)
        {
            return value.ToString("P", CultureInfo.InvariantCulture);
        }

        public static string FormatTempDisplay(this int value)
        {
            var f = RWD.Toolbox.Conversion.Temperature.ConvertCelsiusToFahrenheit(value);
            var display = $"{f}°F ({value}°C)";
            return display;
        }



    }
}
