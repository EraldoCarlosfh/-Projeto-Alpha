using Alpha.Framework.MediatR.EventSourcing.Entity;

namespace Alpha.Framework.MediatR.Resources.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidString(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (value.ToUpper() == "NULL")
                return false;

            return true;
        }

        public static EntityId ToEntityId(this string value)
        {
            if (!value.IsValidString() || !value.IsGuid()) return null;
            return new EntityId(value);
        }

        public static bool IsGuid(this string value)
        {
            System.Guid parsedValue;
            return System.Guid.TryParse(value, out parsedValue);
        }
    }
}
