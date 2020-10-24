using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(HttpResponseCode statusCode, byte[] body)
        {
            this.StatusCode = statusCode;
            this.Version = HttpVersionType.HTTP11;
            this.Headers = new List<Header>();
            this.Cookies = new List<ResponseCookie>();
            this.Body = body;
            if (body?.Length > 0)
            {
                this.Headers.Add(new Header("Content-Length", body.Length.ToString()));
            }

        }

        public HttpVersionType Version { get; set; }
        public HttpResponseCode StatusCode { get; set; }
        public IList<Header> Headers { get; set; }
        public IList<ResponseCookie> Cookies { get; set; }
        public byte[] Body { get; set; }
        public override string ToString()
        {
            var responseToString = new StringBuilder();
            var httpVersionAsString = this.Version switch
            {
                HttpVersionType.HTTP10 => "HTTP/1.0",
                HttpVersionType.HTTP11 => "HTTP/1.1",
                HttpVersionType.HTTP20 => "HTTP/2.0",
                _=> "HTTP/1.1"
            };
            responseToString.Append($"{httpVersionAsString}" +
                $" {(int)this.StatusCode} {this.StatusCode}" + HttpConstants.NEW_LINE);
            foreach (var header in Headers)
            {
                responseToString.Append(header.ToString() + HttpConstants.NEW_LINE);
            }
            foreach (var cookie in Cookies)
            {
                responseToString.Append(
                    "Set-Cookie: " + cookie.ToString() + HttpConstants.NEW_LINE);
            }

            responseToString.Append(HttpConstants.NEW_LINE);

            return responseToString.ToString();

        }
    }

    
    //HTTP/1.1 200 OK
    //Cache-Control: private
    //Content-Type: text/html; charset=utf-8
    //Content-Encoding: gzip
    //Vary: Accept-Encoding
    //X-Frame-Options: SAMEORIGIN
    //Set-Cookie: language=bg; path=/; secure
    //Date: Fri, 23 Oct 2020 11:23:10 GMT
    //Content-Length: 18859
}
