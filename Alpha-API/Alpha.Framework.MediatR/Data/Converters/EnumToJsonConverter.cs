using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.Framework.MediatR.Resources.Enums;

namespace Alpha.Framework.MediatR.Data.Converters
{
    public class EnumToJsonConverter<T> : JsonConverter where T : Enumeration<T, int>
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject;
            try
            {
                jObject = JObject.Load(reader);
            }
            catch
            {
                return null;
            }
            var displayName = jObject["displayName"].ToString();

            var target = Enumeration<T, int>.Parse(displayName);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}