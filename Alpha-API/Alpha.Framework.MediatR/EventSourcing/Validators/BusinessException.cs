using System.Net;

namespace Alpha.Framework.MediatR.EventSourcing.Validators
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; }

        public BusinessException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            StatusCode = (int)statusCode;
        }
    }
}
