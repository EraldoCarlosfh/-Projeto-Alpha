using System;
using System.Linq;
using System.Reflection;

namespace Alpha.Framework.MediatR.Resources.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsStringJsonTypeOf<T>(this string payload) where T : class
        {
            var modelType = typeof(T);

            var numberOfMissings = 0;

            var propertiesInfo = modelType.GetProperties();

            foreach (PropertyInfo propertyInfo in propertiesInfo)
            {
                if (!payload.ToUpper().Contains(propertyInfo.Name.ToUpper()))
                    numberOfMissings++;
            }

            if (numberOfMissings == 0)
                return true;

            // Consider 20% of allowed missing properties to handler null values
            double error = Math.Round((double)numberOfMissings / (double)propertiesInfo.Count(), 2);

            return error < 0.20;
        }
    }
}
