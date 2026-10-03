using System.Net;

namespace ReadMeter.Api.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string msg) : base(HttpStatusCode.NotFound, msg)
        {
        }
    }
}