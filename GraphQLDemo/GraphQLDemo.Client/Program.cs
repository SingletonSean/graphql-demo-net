using GraphQLDemo.Client.Scripts;
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

                    string httpGraphQLApiUrl = $"http://{graphqlApiUrl}";
                    string webSocketsGraphQLApiUrl = $"ws://{graphqlApiUrl}";

                    services
                        .AddGraphQLDemoClient()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(httpGraphQLApiUrl))
                        .ConfigureWebSocketClient(c => c.Uri = new Uri(webSocketsGraphQLApiUrl));

                    services.AddHostedService<Startup>();

                    services.AddTransient<GetCoursesScript>();
                })
                .Build()
                .Run();
        }
    }

    public class Startup : IHostedService
    {
        private readonly GetCoursesScript _getCoursesScript;

        public Startup(GetCoursesScript getCoursesScript)
        {
            _getCoursesScript = getCoursesScript;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _getCoursesScript.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
