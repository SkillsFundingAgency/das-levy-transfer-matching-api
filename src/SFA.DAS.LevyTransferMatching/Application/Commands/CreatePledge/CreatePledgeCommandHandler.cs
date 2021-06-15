using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using DataModels = SFA.DAS.LevyTransferMatching.Data.Models;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

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
            var result = await _dbContext.AddAsync(new DataModels.Pledge
            {
                Amount = command.Amount,
                CreatedOn = DateTime.UtcNow,
                EmployerAccountId = command.AccountId,
                IsNamePublic = command.IsNamePublic,
                Levels = command.Levels.Sum(x => (int)x),
                JobRoles = command.JobRoles.Sum(x => (int)x),
                Sectors = command.Sectors.Sum(x => (int)x),
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            command.Id = result.Entity.Id;

            return new CreatePledgeResult
            {
                Id = command.Id.Value,
            };
        }
    }
}