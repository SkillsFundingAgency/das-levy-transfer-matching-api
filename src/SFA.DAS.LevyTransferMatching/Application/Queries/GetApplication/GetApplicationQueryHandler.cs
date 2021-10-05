﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly LevyTransferMatchingDbContext _levyTransferMatchingDbContext;

        public GetApplicationQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext)
        {
            _levyTransferMatchingDbContext = levyTransferMatchingDbContext;
        }

        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var applicationQuery = _levyTransferMatchingDbContext.Applications
                .Include(x => x.EmailAddresses)
                .Include(x => x.EmployerAccount)
                .Include(x => x.Pledge)
                .Include(x => x.Pledge.EmployerAccount)
                .Include(x => x.Pledge.Locations)
                .Where(x => x.Id == request.ApplicationId)
                .AsQueryable();

            if (request.PledgeId.HasValue)
            {
                applicationQuery = applicationQuery
                    .Where(x => x.PledgeId == request.PledgeId.Value);
            }
            
            var application = await applicationQuery.SingleOrDefaultAsync();

            if (application == null)
            {
                return null;
            }

            var result = new GetApplicationResult()
            {
                BusinessWebsite = application.BusinessWebsite,
                Details = application.Details,
                EmailAddresses = application.EmailAddresses.Select(x => x.EmailAddress),
                FirstName = application.FirstName,
                HasTrainingProvider = application.HasTrainingProvider,
                LastName = application.LastName,
                NumberOfApprentices = application.NumberOfApprentices,
                Postcode = application.Postcode,
                Sectors = application.Sectors.ToList(),
                StandardId = application.StandardId,
                StartDate = application.StartDate,
                EmployerAccountName = application.EmployerAccount.Name,
                PledgeLocations = application.Pledge.Locations.Select(x => x.Name).ToList(),
                PledgeSectors = application.Pledge.Sectors.ToList(),
                PledgeLevels = application.Pledge.Levels.ToList(),
                PledgeJobRoles = application.Pledge.JobRoles.ToList(),
                Amount = application.Amount,
                PledgeRemainingAmount = application.Pledge.RemainingAmount,
                Status = application.Status,
                PledgeIsNamePublic = application.Pledge.IsNamePublic,
                PledgeId = application.PledgeId,
                ReceiverEmployerAccountId = application.EmployerAccount.Id,
                SenderEmployerAccountId = application.Pledge.EmployerAccount.Id,
            };

            return result;
        }
    }
}