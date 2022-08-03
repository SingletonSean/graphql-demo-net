using System;
using System.Collections.Generic;

namespace GraphQLDemo.API.DTOs
{
    public class InstructorDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Salary { get; set; }

        public IEnumerable<CourseDTO> Courses { get; set; }
    }
}
