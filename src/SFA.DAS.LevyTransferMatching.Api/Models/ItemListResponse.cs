using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class ItemListResponse<TResult>
    {
        public IEnumerable<TResult> Items { get; set; }
        public int TotalItems { get; set; }
    }
}