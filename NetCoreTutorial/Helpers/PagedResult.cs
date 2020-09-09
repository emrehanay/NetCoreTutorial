using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetCoreTutorial.Helpers
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        [JsonIgnore] public IEnumerable<T> TempResult { get; set; }
        public IEnumerable Results { get; set; }

        public PagedResult()
        {
            TempResult = new List<T>();
        }
    }
}