using System;

namespace GraphQLDemo.API.Schema.Queries
{
    public class InstructorType : ISearchResultType
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Salary { get; set; }
    }
}
