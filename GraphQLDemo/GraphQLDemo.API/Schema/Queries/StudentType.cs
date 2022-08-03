using HotChocolate;
using System;

namespace GraphQLDemo.API.Schema.Queries
{
    public class StudentType
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [GraphQLName("gpa")]
        public double GPA { get; set; }
    }
}
