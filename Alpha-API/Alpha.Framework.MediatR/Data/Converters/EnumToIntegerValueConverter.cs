using Alpha.Framework.MediatR.Resources.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Data.Converters
{
    public class EnumToIntegerValueConverter<T> : ValueConverter<T, int?> where T : Enumeration<T, int>
    {
        public EnumToIntegerValueConverter(ConverterMappingHints mappingHints = null)
            : base(
                v => v != null ? v.Value : (int?)null,
                v => v.HasValue ? Enumeration<T, int>.FromValue(v.Value) : null,
                mappingHints)
        {
        }

        public static ValueConverterInfo DefaultInfo { get; }
            = new ValueConverterInfo(typeof(T), typeof(int), i => new EnumToIntegerValueConverter<T>(i.MappingHints));
    }
}
