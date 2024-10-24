﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;

public class GetApplicationQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext)
    : IRequestHandler<GetApplicationQuery, GetApplicationResult>
{
    public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var applicationQuery = levyTransferMatchingDbContext.Applications
            .Include(x => x.ApplicationLocations)
            .Include(x => x.EmailAddresses)
            .Include(x => x.EmployerAccount)
            .Include(x => x.Pledge)
            .Include(x => x.Pledge.EmployerAccount)
            .Include(x => x.ApplicationCostProjections)
            .Include(x => x.StatusHistory)
            .Where(x => x.Id == request.ApplicationId)
            .AsQueryable();

        if (request.PledgeId.HasValue)
        {
            applicationQuery = applicationQuery
                .Where(x => x.PledgeId == request.PledgeId.Value);
        }

        var application = await applicationQuery.SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (application == null)
        {
            return null;
        }

        var result = new GetApplicationResult
        {
            BusinessWebsite = application.BusinessWebsite,
            Details = application.Details,
            EmailAddresses = application.EmailAddresses.Select(x => x.EmailAddress),
            CreatedOn = application.CreatedOn,
            FirstName = application.FirstName,
            HasTrainingProvider = application.HasTrainingProvider,
            LastName = application.LastName,
            NumberOfApprentices = application.NumberOfApprentices,
            Sectors = application.Sectors.ToList(),
            StandardId = application.StandardId,
            StandardTitle = application.StandardTitle,
            StandardLevel = application.StandardLevel,
            StandardDuration = application.StandardDuration,
            StandardMaxFunding = application.StandardMaxFunding,
            StandardRoute = application.StandardRoute,
            StartDate = application.StartDate,
            EmployerAccountName = application.EmployerAccount.Name,
            SenderEmployerAccountName = application.Pledge.EmployerAccount.Name,
            AmountUsed = application.AmountUsed,
            NumberOfApprenticesUsed = application.NumberOfApprenticesUsed,
            PledgeEmployerAccountName = application.Pledge.EmployerAccount.Name,
            Locations = application.ApplicationLocations.Select(x => new GetApplicationResult.ApplicationLocation { Id = x.Id, PledgeLocationId = x.PledgeLocationId }).ToList(),
            AdditionalLocation = application.AdditionalLocation,
            SpecificLocation = application.SpecificLocation,
            Amount = application.GetCost(),
            TotalAmount = application.TotalAmount,
            Status = application.Status,
            PledgeId = application.PledgeId,
            ReceiverEmployerAccountId = application.EmployerAccount.Id,
            SenderEmployerAccountId = application.Pledge.EmployerAccount.Id,
            AutomaticApproval = application.AutomaticApproval,
            CostProjections = application.ApplicationCostProjections.Select(p => new GetApplicationResult.CostProjection { FinancialYear = p.FinancialYear, Amount = p.Amount }).ToList(),
            MatchPercentage = application.MatchPercentage,
            MatchJobRole = application.MatchJobRole,
            MatchLevel = application.MatchLevel,
            MatchLocation = application.MatchLocation,
            MatchSector = application.MatchSector,
            CostingModel = application.CostingModel,
            PledgeRemainingAmount = application.Pledge.RemainingAmount,
            AutomaticApprovalOption = application.Pledge.AutomaticApprovalOption
        };

        return result;
    }
}