using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.controllers.models
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Deleted { get; set; } = 0;
    }
}