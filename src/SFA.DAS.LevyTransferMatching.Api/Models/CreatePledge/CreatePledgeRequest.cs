using Newtonsoft.Json;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Api.Models.Base;

namespace SFA.DAS.LevyTransferMatching.Api.Models.CreatePledge
{
    public class CreatePledgeRequest: StateChangeRequest
    {
        [JsonProperty(Required = Required.Always)]
        public int Amount { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool IsNamePublic { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string DasAccountName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<Sector> Sectors { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<JobRole> JobRoles { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<Level> Levels { get; set; }

        [JsonProperty(Required = Required.Always)]
        public List<LocationInformation> Locations { get; set; }
    }
}