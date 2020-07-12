using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Services.Models
{
    
    public class EmailAddress
    {

        public EmailAddress(string address, string name)
        {
            Address = address;
            Name = name;
        }

        public string Address { get; }
        public string Name { get;  }

        

    }
}
