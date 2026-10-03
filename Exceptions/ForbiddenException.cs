using System.Net;

namespace ReadMeter.Api.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string msg) : base(HttpStatusCode.Forbidden, msg)
        {
        }
    }
}