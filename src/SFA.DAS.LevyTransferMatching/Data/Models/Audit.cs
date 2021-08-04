using System;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Audit
    {
        public long Id { get; set; }
        public string EntityType { get; set; }
        public long EntityId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string UserAction { get; set; }
        public DateTime AuditDate { get; set; }
        public string InitialState { get; set; }
        public string UpdatedState { get; set; }
        public string Diff { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
