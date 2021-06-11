using Newtonsoft.Json;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class CreatePledgeRequest
    {
        [JsonProperty(Required = Required.Always)]
        public int Amount { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool IsNamePublic { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<Sector> Sectors { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<JobRole> JobRoles { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<Level> Levels { get; set; }
    }
}