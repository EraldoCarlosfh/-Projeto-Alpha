using Alpha.Framework.MediatR.Data.Converters;
using Alpha.Framework.MediatR.Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Auditorship.Domain.Enums
{
    [JsonConverter(typeof(EnumToJsonConverter<OperationType>))]
    public class OperationType : Enumeration<OperationType, int>
    {
        public OperationType()
        {
        }

        public const int CreateId = 1;
        public const int UpdateId = 2;
        public const int InactiveId = 3;
        public const int OtherId = 4;
        public const int ScheduledJobId = 5;

        public readonly static OperationType Create = new OperationType(
            CreateId,
            nameof(Create),
            "Criação");

        public readonly static OperationType Update = new OperationType(
            UpdateId,
            nameof(Update),
            "Atualização");

        public readonly static OperationType Inactive = new OperationType(
            InactiveId,
            nameof(Inactive),
            "Inativação");

        public readonly static OperationType ScheduledJob = new OperationType(
            ScheduledJobId,
            nameof(ScheduledJobId),
            "Tarefa agendada");

        public readonly static OperationType Other = new OperationType(
            OtherId,
            nameof(Other),
            "Outro");

        public string FriendlyName { get; set; }

        public OperationType(
            int id,
            string name,
            string friendlyName
            ) : base(id, name)
        {
            FriendlyName = friendlyName;
        }

        public static OperationType GetByRequestName(string requestName)
        {
            var normalizedName = requestName.ToUpper();

            if (normalizedName.ToUpper().Contains("ADD") || normalizedName.ToUpper().Contains("CREATE") || normalizedName.ToUpper().Contains("INVITE"))
            {
                return Create;
            }

            if (normalizedName.ToUpper().Contains("UPDATE") || normalizedName.ToUpper().Contains("SET"))
            {
                return Update;
            }

            if (normalizedName.ToUpper().Contains("JOB"))
            {
                return ScheduledJob;
            }

            if (normalizedName.ToUpper().Contains("INACTIVATE"))
            {
                return Inactive;
            }

            return Other;
        }
    }
}
