using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Models
{
    public class Messages
    {
        public string token;
    }

    public class LoginResponse
    {
        public string status;
        public Messages message;
    }
}