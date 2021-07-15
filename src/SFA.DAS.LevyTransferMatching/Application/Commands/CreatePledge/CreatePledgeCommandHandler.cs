using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeCommandHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public CreatePledgeCommandHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var employerAccount = await _dbContext.EmployerAccounts.FindAsync(command.AccountId);

                var pledge = employerAccount.CreatePledge(command.Amount,
                    command.IsNamePublic,
                    (Level)command.Levels.Cast<int>().Sum(),
                    (JobRole)command.JobRoles.Cast<int>().Sum(),
                    (Sector)command.Sectors.Cast<int>().Sum()
                );

                var result = await _dbContext.AddAsync(pledge, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                var pledgeId = result.Entity.Id;

                return new CreatePledgeResult
                {
                    Id = pledgeId
                };
            }
            catch (Exception e)
            {
                
                throw;
            }
            
        }
    }
}