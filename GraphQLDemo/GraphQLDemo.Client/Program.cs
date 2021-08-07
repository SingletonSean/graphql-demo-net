using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrawberryShake;
using System;
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

            Console.ReadKey();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
