using GraphQLDemo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.DTOs
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public string CreatorId { get; set; }

        public Guid InstructorId { get; set; }
        public InstructorDTO Instructor { get; set; }

        public IEnumerable<StudentDTO> Students { get; set; }
    }
}
