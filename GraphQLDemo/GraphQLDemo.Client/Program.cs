using Microsoft.Extensions.DependencyInjection;
using StrawberryShake;
using System;
using System.Threading.Tasks;

namespace GraphQLDemo.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection
                .AddGraphQLDemoClient()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5000/graphql"));

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IGraphQLDemoClient client = serviceProvider.GetRequiredService<IGraphQLDemoClient>();

            IOperationResult<IGetInstructionsResult> result = await client.GetInstructions.ExecuteAsync();

            if (result.IsErrorResult())
            {
                Console.WriteLine("Failed to get instructions");
            }
            else
            {
                Console.WriteLine(result.Data?.Instructions);
            }

            Console.ReadKey();
        }
    }
}
