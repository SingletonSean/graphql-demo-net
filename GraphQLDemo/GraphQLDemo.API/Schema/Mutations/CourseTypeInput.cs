using GraphQLDemo.API.Models;
using System;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class CourseTypeInput
    {
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
