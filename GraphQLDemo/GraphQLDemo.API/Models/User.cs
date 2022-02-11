using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool EmailVerified { get; set; }
    }
}
