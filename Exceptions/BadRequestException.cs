using System.Net;

namespace ReadMeter.Api.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string msg) : base(HttpStatusCode.BadRequest, msg)
        {
        }
    }
}
