using System;

namespace SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

public class LevyTransferMatchingApi
{
    public string DatabaseConnectionString { get; set; }
    public string NServiceBusConnectionString { get; set; }
    public string NServiceBusLicense { get; set; }
    public string RedisConnectionString { get; set; }
    public string DataProtectionKeysDatabase { get; set; }
    public string UtcNowOverride { get; set; }
    public DateTime AutoDeclineImplementationDate { get; set; }
    public DateTime AutoExpireApplicationsImplementationDate { get; set; }
    public FeatureToggles FeatureToggles { get; set; }
}

public class FeatureToggles
{
    public bool ToggleNewCostingModel { get; set; }
}