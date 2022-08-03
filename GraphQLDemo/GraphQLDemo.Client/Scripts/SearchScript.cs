using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Client.Scripts
{
    public class SearchScript
    {
        private readonly IGraphQLDemoClient _client;

        public SearchScript(IGraphQLDemoClient client)
        {
            _client = client;
        }

        public async Task Run()
        {
            Console.WriteLine("Enter a search term:");
            string term = Console.ReadLine();
            IOperationResult<ISearchResult> searchResult = await _client.Search.ExecuteAsync(term);

            IEnumerable<ISearch_Search_CourseType> courses = searchResult.Data.Search.OfType<ISearch_Search_CourseType>();
            Console.WriteLine("COURSES");
            foreach (ISearch_Search_CourseType course in courses)
            {
                Console.WriteLine(course.Name);
            }
            
            IEnumerable<ISearch_Search_InstructorType> instructors = searchResult.Data.Search.OfType<ISearch_Search_InstructorType>();
            Console.WriteLine();
            Console.WriteLine("INSTRUCTORS");
            foreach (ISearch_Search_InstructorType instructor in instructors)
            {
                Console.WriteLine($"{instructor.FirstName} {instructor.LastName}");
            }

            Console.WriteLine();
        }
    }
}
