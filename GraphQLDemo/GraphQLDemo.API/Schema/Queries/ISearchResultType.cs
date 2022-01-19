using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    [UnionType("SearchResult")]
    public interface ISearchResultType
    {
    }
}
