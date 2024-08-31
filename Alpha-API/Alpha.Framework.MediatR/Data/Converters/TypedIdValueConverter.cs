using Alpha.Framework.MediatR.EventSourcing.Entity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Alpha.Framework.MediatR.Data.Converters
{
    public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
        where TTypedIdValue : TypedIdValueBase
    {
        public TypedIdValueConverter(ConverterMappingHints mappingHints = null)
            : base(id => id.Value, value => Create(value), mappingHints)
        {
        }

        private static TTypedIdValue Create(Guid id) => Activator.CreateInstance(typeof(TTypedIdValue), id.ToString()) as TTypedIdValue;
    }
}
