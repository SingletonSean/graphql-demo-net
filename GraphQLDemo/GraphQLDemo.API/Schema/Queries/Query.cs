using GraphQLDemo.API.Services;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    public class Query
    {
        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<IEnumerable<ISearchResultType>> Search(string term, [ScopedService] SchoolDbContext context) 
        {
            IEnumerable<CourseType> courses = await context.Courses
                .Where(c => c.Name.Contains(term))
                .Select(c => new CourseType()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subject = c.Subject,
                    InstructorId = c.InstructorId,
                    CreatorId = c.CreatorId
                })
                .ToListAsync();

            IEnumerable<InstructorType> instructors = await context.Instructors
                .Where(i => i.FirstName.Contains(term) || i.LastName.Contains(term))
                .Select(i => new InstructorType()
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Salary = i.Salary,
                })
                .ToListAsync();

            return new List<ISearchResultType>()
                .Concat(courses)
                .Concat(instructors);
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string Instructions => "Smash that like button and subscribe to SingletonSean";
    }
}
