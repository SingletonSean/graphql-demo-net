using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Middlewares.UseUser
{
    public class UserAttribute : GlobalStateAttribute
    {
        public UserAttribute() : base(UserMiddleware.USER_CONTEXT_DATA_KEY)
        {
        }
    }
}
