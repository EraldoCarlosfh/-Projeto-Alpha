using System.Globalization;

namespace Alpha.Framework.MediatR.Resources.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal ConvertToDecimal(this string value, bool isPercent)
        {
            var valueToConvert = isPercent ? value.Replace(".", ",") : value;

            var convertedValue = decimal.TryParse(valueToConvert, NumberStyles.Any, new CultureInfo("pt-BR"), out decimal decimalOutput) ?
            decimalOutput : decimal.Parse(valueToConvert, NumberStyles.Any, CultureInfo.InvariantCulture);

            return convertedValue;
        }

        public static decimal RoundLastDigit(this decimal value)
        {
            var rounded = (int)value;
            var rest = (rounded / 10m) % 1;

            if (rest <= 0.29m)
                return (((rest * 10) - 0) * -1) + rounded;
            else if (rest >= 0.3m && rest <= 0.75m)
                return (((rest * 10) - 5) * -1) + rounded;
            else
                return (((rest * 10) - 10) * -1) + rounded;
        }

        public static int ConvertToIntCurrency(this decimal amount)
        {
            var precision = 2;

            while (precision > 0)
            {
                amount *= 10;
                precision--;
            }

            return (int)Math.Floor(amount);
        }
    }
}
