using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    public class UserType
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
    }
}
