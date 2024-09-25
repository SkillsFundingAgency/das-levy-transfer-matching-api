using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;

public class CreatePledgeCommandHandler(
    IEmployerAccountRepository employerAccountRepository,
    IPledgeRepository pledgeRepository)
    : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
{
    public async Task<CreatePledgeResult> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
    {
        var employerAccount = await employerAccountRepository.Get(request.AccountId);

        var properties = new CreatePledgeProperties
        {
            Amount = request.Amount,
            IsNamePublic = request.IsNamePublic,
            AutomaticApprovalOption = request.AutomaticApprovalOption,
            Levels = (Level)request.Levels.Cast<int>().Sum(),
            JobRoles = (JobRole)request.JobRoles.Cast<int>().Sum(),
            Sectors = (Sector)request.Sectors.Cast<int>().Sum(),
            Locations = request.Locations.Select(x =>
                new PledgeLocation
                {
                    Name = x.Name,
                    Latitude = x.Geopoint[0],
                    Longitude = x.Geopoint[1]
                }).ToList(),
        };

        var userInfo = new UserInfo(request.UserId, request.UserDisplayName);

        var pledge = employerAccount.CreatePledge(properties, userInfo);

        await pledgeRepository.Add(pledge);

        return new CreatePledgeResult
        {
            Id = pledge.Id
        };
    }
}