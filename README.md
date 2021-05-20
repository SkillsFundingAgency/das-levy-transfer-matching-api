# Levy Transfers Matching API

This repository contains the inner Levy Transfers Matching API

## Getting Started

To get the project running you will need to:

* Clone this repository
* Set the start up project to SFA.DAS.LevyTransferMatching.Api
* Restore packages
* Deploy the SFA.DAS.LevyTransferMatching.Database project
* Get the latest config for the project from [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) and deploy it to an instance of Azure Table Storage manually or by using the [das-employer-config-updater](https://github.com/SkillsFundingAgency/das-employer-config-updater)

> Both the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config) and the [das-employer-config-updater repository](https://github.com/SkillsFundingAgency/das-employer-config-updater) are private and only available to members of the SkillsFundingAgency GitHub Organisation

### Prerequisites

* An IDE supporting .NetCore 3.1
* The latest configuration from [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config)
* Azure Table Storage Emulator

## Usage

## Testing

Unit tests are available in tests folder and can be run with any NUnit test runner

## Known Issues

None

## License

[MIT License](https://choosealicense.com/licenses/mit/)
