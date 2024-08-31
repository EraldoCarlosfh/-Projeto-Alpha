using Alpha.Framework.MediatR.Notifications;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Alpha.Framework.MediatR.Api.Controllers
{
    public class ApiResult
    {
        public ApiResult()
        {

        }

        [JsonProperty(PropertyName = "isSuccess")]
        public bool IsSuccess { get; private set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; private set; }

        [JsonProperty(PropertyName = "errors")]
        public List<Notification> Errors { get; private set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; private set; }


        public ApiResult(bool isSuccess, string message, object data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public ApiResult(bool isSuccess, string message, List<Notification> erros)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = erros;
        }

        public ApiResult(bool isSuccess, string message, object data, List<Notification> erros)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Errors = erros;
        }
    }
}
