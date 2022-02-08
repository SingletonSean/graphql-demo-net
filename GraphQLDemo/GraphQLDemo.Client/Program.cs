using Firebase.Auth;
using GraphQLDemo.Client.Scripts;
using GraphQLDemo.Client.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrawberryShake;
using System;
using System.Linq;
using System.Net.Http.Headers;
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
                        .ConfigureHttpClient((services, c) => {
                            c.BaseAddress = new Uri(httpGraphQLApiUrl);

                            TokenStore tokenStore = services.GetRequiredService<TokenStore>();
                            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStore.AccessToken);
                        })
                        .ConfigureWebSocketClient(c => c.Uri = new Uri(webSocketsGraphQLApiUrl));

                    services.AddHostedService<Startup>();

                    services.AddSingleton<TokenStore>();

                    string firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");
                    services.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey)));

                    services.AddTransient<GetCoursesScript>();
                    services.AddTransient<CreateCourseScript>();
                    services.AddTransient<LoginScript>();
                })
                .Build()
                .Run();
        }
    }

    public class Startup : IHostedService
    {
        private readonly CreateCourseScript _createCourseScript;
        private readonly LoginScript _loginScript;

        public Startup(CreateCourseScript createCourseScript, LoginScript loginScript)
        {
            _createCourseScript = createCourseScript;
            _loginScript = loginScript;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _loginScript.Run();
            await _createCourseScript.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
