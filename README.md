# Levy Transfer Matching Api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

Holds information on Pledges and Applications for Levy Transfer Matching.

## How It Works

This web api serves as the inner api for the Levy Transfer Matching stack of services:

* [Levy Transfer Matching Web](https://github.com/SkillsFundingAgency/das-levy-transfer-matching-web)
* [Levy Transfer Matching Functions](https://github.com/SkillsFundingAgency/das-levy-transfer-matching-functions)
* LevyTransferMatching Outer Api in [Apim endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints)

## 🚀 Installation

### Pre-Requisites

The Azure Functions component is not necessary for the website to function but can be found [here](https://github.com/SkillsFundingAgency/das-levy-transfer-matching-functions)

* A clone of this repository
* A code editor that supports .NetCore 3.1
* A Redis instance
* The latest [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) for:
  *  `SFA.DAS.LevyTransferMatchingApi_1.0`

### Config


This utility uses the standard Apprenticeship Service configuration. All configuration can be found in the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config) which may be more up-to-date than what is described here.

Azure Table Storage config

Row Key: SFA.DAS.LevyTransferMatching.Web_1.0

Partition Key: LOCAL

Data:

```json
{
  "LevyTransferMatchingApi": {
    "DatabaseConnectionString": "Data Source=.;Initial Catalog=SFA.DAS.LevyTransferMatching.Database;Integrated Security=True",
    "NServiceBusConnectionString": "UseDevelopmentStorage=true",
    "NServiceBusLicense": "",
    "RedisConnectionString": "",
    "DataProtectionKeysDatabase": ""
   },
"AzureAd": {
    "tenant": "citizenazuresfabisgov.onmicrosoft.com",
    "identifier": "https://citizenazuresfabisgov.onmicrosoft.com/das-at-ltmapi-as-ar"
  }
}
```

## Technologies

* .NetCore 3.1
* REDIS
* NLog
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions

## 🐛 Known Issues

* This web api must be run under the Kestrel web server
