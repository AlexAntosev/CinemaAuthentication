using System;

namespace AuthenticationManager.Business.Exceptions
{
    public class HttpBadRequestException : HttpStatusCodeException
    {
        private const int BAD_REQUEST = 400;

        public HttpBadRequestException(int statusCode = BAD_REQUEST) : base(statusCode)
        {
        }

        public HttpBadRequestException(string message, int statusCode = BAD_REQUEST) : base(statusCode, message)
        {
        }

        public HttpBadRequestException(Exception inner, int statusCode = BAD_REQUEST) : base(statusCode, inner)
        {
        }
    }

}
