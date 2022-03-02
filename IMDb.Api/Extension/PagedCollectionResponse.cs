using System;
using System.Collections.Generic;

namespace IMDb.Api.Extension
{
    public class PagedCollectionResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
    }
}
