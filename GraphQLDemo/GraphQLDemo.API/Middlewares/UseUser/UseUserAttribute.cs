using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Middlewares.UseUser
{
    public class UseUserAttribute : ObjectFieldDescriptorAttribute
    {
        public UseUserAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }

        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.Use<UserMiddleware>();
        }
    }
}
