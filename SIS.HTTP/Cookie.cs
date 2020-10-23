using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class Cookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ResponseCookie : Cookie
    {
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime? Expires { get; set; }
        public long MaxAge { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public SameSiteType SameSite { get; set; }

    }

}
