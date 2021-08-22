using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrawberryShake;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    string graphqlApiUrl = context.Configuration.GetValue<string>("GRAPHQL_API_URL");

                    services
                        .AddGraphQLDemoClient()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(graphqlApiUrl));

                    services.AddHostedService<Startup>();
                })
                .Build()
                .Run();
        }
    }

    public class Startup : IHostedService
    {
        private readonly IGraphQLDemoClient _client;

        public Startup(IGraphQLDemoClient client)
        {
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IOperationResult<IGetCoursesResult> result = await _client.GetCourses.ExecuteAsync();

            if (result.IsErrorResult())
            {
                Console.WriteLine("Failed to get courses.");
            }
            else
            {
                foreach (IGetCourses_Courses course in result.Data?.Courses)
                {
                    Console.WriteLine($"{course.Name} is taught by {course.Instructor.FirstName} and has {course.Students.Count} students.");
                }
            }

            Console.WriteLine();

            IOperationResult<IGetCourseByIdResult> courseByIdResult = await _client.GetCourseById.ExecuteAsync(Guid.NewGuid());

            if (result.IsErrorResult())
            {
                Console.WriteLine("Failed to get course.");
            }
            else
            {
                IGetCourseById_CourseById course = courseByIdResult.Data?.CourseById;
                Console.WriteLine($"{course.Name} is taught by {course.Instructor.FirstName} and has {course.Students.Count} students.");
            }

            IOperationResult<ICreateCourseResult> createCourseResult = await _client.CreateCourse.ExecuteAsync(new CourseTypeInput()
            {
                Name = "GraphQL 101",
                Subject = Subject.Science,
                InstructorId = Guid.NewGuid()
            });
            Guid courseId = createCourseResult.Data.CreateCourse.Id;
            string createdCourseName = createCourseResult.Data.CreateCourse.Name;
            Console.WriteLine($"Successfully created course {createdCourseName}.");

            IOperationResult<IUpdateCourseResult> updateCourseResult = await _client.UpdateCourse.ExecuteAsync(courseId, new CourseTypeInput()
            {
                Name = "GraphQL 102",
                Subject = Subject.Science,
                InstructorId = Guid.NewGuid()
            });

            if(updateCourseResult.IsErrorResult())
            {
                IClientError error = updateCourseResult.Errors.First();
                if(error.Code == "COURSE_NOT_FOUND")
                {
                    Console.WriteLine("Course was not found.");
                }
                else
                {
                    Console.WriteLine("Unknown course update error.");
                }
            }
            else
            {
                string updatedCourseName = updateCourseResult.Data.UpdateCourse.Name;
                Console.WriteLine($"Successfully updated course to {updatedCourseName}.");
            }

            IOperationResult<IDeleteCourseResult> deleteCourseResult = await _client.DeleteCourse.ExecuteAsync(courseId);
            bool deleteCourseSuccessful = deleteCourseResult.Data.DeleteCourse;
            if(deleteCourseSuccessful)
            {
                Console.WriteLine("Successfully deleted course.");
            }

            Console.ReadKey();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
