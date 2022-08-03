using StrawberryShake;
using System;
using System.Threading.Tasks;

namespace GraphQLDemo.Client.Scripts
{
    public class CreateCourseScript
    {
        private readonly IGraphQLDemoClient _client;

        public CreateCourseScript(IGraphQLDemoClient client)
        {
            _client = client;
        }

        public async Task Run()
        {
            CourseTypeInput courseInput = new CourseTypeInput()
            { 
                Name = "Algebra",
                Subject = Subject.Mathematics,
                InstructorId = Guid.Parse("b0920fe9a1a54468b9faa5fe295dfc2d")
            };

            IOperationResult<ICreateCourseResult> createCourseResult = await _client.CreateCourse.ExecuteAsync(courseInput);

            Console.WriteLine(createCourseResult.Errors);
        }
    }
}
