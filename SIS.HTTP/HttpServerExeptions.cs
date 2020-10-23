using System;

namespace SIS.HTTP
{
    public class HttpServerExeptions : Exception
    {
        public HttpServerExeptions(string message):base(message)
        {

        }
    }
}
