using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLDemo.Client.Scripts
{
    public class GetCoursesScript
    {
        private readonly IGraphQLDemoClient _client;

        public GetCoursesScript(IGraphQLDemoClient client)
        {
            _client = client;
        }

        public async Task Run()
        {
            ConsoleKey key;

            int? first = 5;
            string after = null;

            int? last = null;
            string before = null;

            do
            {
                IOperationResult<IGetCoursesResult> coursesResult = await _client.GetCourses.ExecuteAsync(
                    first, after, last, before, null,
                    new List<CourseTypeSortInput>()
                    {
                        new CourseTypeSortInput()
                        {
                            Name = SortEnumType.Asc
                        }
                    });

                Console.WriteLine($"{"Name",-10} | {"Subject",-10}");
                Console.WriteLine();

                foreach (IGetCourses_Courses_Nodes course in coursesResult.Data.Courses.Nodes)
                {
                    Console.WriteLine($"{course.Name,-10} | {course.Subject,-10}");
                }

                IGetCourses_Courses_PageInfo pageInfo = coursesResult.Data.Courses.PageInfo;

                if(pageInfo.HasPreviousPage)
                {
                    Console.WriteLine("Press 'A' to move to the previous page.");
                }

                if(pageInfo.HasNextPage)
                {
                    Console.WriteLine("Press 'D' to move to the next page.");
                }

                Console.WriteLine("Press 'Enter' to exit.");

                key = Console.ReadKey().Key;

                if(key == ConsoleKey.A && pageInfo.HasPreviousPage)
                {
                    last = 5;
                    before = pageInfo.StartCursor;

                    first = null;
                    after = null;
                }
                else if (key == ConsoleKey.D && pageInfo.HasNextPage)
                {
                    first = 5;
                    after = pageInfo.EndCursor;

                    last = null;
                    before = null;
                }
            } while (key != ConsoleKey.Enter);
        }
    }
}
