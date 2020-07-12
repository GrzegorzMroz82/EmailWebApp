using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Infrastructure
{
    public class SmtpOptions
    {
        public const string Smtp = "Smtp";
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
