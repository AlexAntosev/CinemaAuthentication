using System;

namespace AuthenticationManager.Business.Exceptions
{
    public class HttpNotFoundException : HttpStatusCodeException
    {
        private const int NOT_FOUND = 404;

        public HttpNotFoundException(int statusCode = NOT_FOUND) : base(statusCode)
        {
        }

        public HttpNotFoundException(string message, int statusCode = NOT_FOUND) : base(statusCode, message)
        {
        }

        public HttpNotFoundException(Exception inner, int statusCode = NOT_FOUND) : base(statusCode, inner)
        {
        }
    }

}
