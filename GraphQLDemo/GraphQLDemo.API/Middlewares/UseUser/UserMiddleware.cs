using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.Models;
using HotChocolate.Resolvers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Middlewares.UseUser
{
    public class UserMiddleware
    {
        public const string USER_CONTEXT_DATA_KEY = "User";

        private readonly FieldDelegate _next;

        public UserMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context)
        {
            if(context.ContextData.TryGetValue("ClaimsPrincipal", out object rawClaimsPrincipal) && 
                rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
            {
                bool emailVerified = bool.TryParse(claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED), out bool result) && result;
                
                User user = new User()
                {
                    Id = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID),
                    Email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL),
                    Username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME),
                    EmailVerified = emailVerified,
                };

                context.ContextData.Add(USER_CONTEXT_DATA_KEY, user);
            }

            await _next(context);
        }
    }
}
