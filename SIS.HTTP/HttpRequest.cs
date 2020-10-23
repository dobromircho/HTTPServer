﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string httpRequestAsString)
        {
            this.Headers = new List<Header>();
            var lines = httpRequestAsString.Split(
                new string[] { HttpConstants.NEW_LINE }, StringSplitOptions.None);
            var httpInfoHeader = lines[0];
            var infoHeaderParts = httpInfoHeader.Split(' '); //Split first line to three parts
            if (infoHeaderParts.Length !=3)
            {
                throw new HttpServerExeptions("Invalid HTTP header line.");
            }
            var httpMethod = infoHeaderParts[0];
            this.Method = (httpMethod) switch
            {
                "POST" => HttpMethodType.Post,
                "GET" => HttpMethodType.Get,
                "PUT" => HttpMethodType.Put,
                "DELETE" => HttpMethodType.Delete,
                _=> HttpMethodType.Unknown
            };
            this.Path = infoHeaderParts[1];
            var httpVersion = infoHeaderParts[2];
            this.Version = httpVersion switch
            {
                "HTTP/1.0" => HttpVersionType.HTTP10,
                "HTTP/1.1" => HttpVersionType.HTTP11,
                "HTTP/2.0" => HttpVersionType.HTTP20,
                _=> HttpVersionType.HTTP11
            };

            bool isInHeader = true;
            StringBuilder bodyBuilder = new StringBuilder();
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeader = false;
                    continue;
                }
                if (isInHeader)
                {
                    var headerParts = line.Split(new string[] { ": " }, 2,
                                                 StringSplitOptions.None);
                    if (headerParts.Length != 2)
                    {
                        throw new HttpServerExeptions($"Invalid header: {line}");
                    }
                    var header = new Header(headerParts[0], headerParts[1]);
                    this.Headers.Add(header);
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }
            this.Body = bodyBuilder.ToString();

        }
        public HttpMethodType Method { get; set; } 
        public string Path { get; set; }
        public HttpVersionType Version { get; set; }
        public IList<Header> Headers { get; set; }
        public string Body { get; set; }

    }
}