using System.Net;

namespace ReadMeter.Api.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string msg) : base(HttpStatusCode.Conflict, msg)
        {
        }
    }
}