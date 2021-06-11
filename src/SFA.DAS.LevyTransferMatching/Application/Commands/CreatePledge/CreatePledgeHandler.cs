using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using DataModels = SFA.DAS.LevyTransferMatching.Data.Models;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using FluentValidation;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly IValidator<CreatePledgeCommand> _validator;
        private readonly LevyTransferMatchingDbContext _dbContext;

        public CreatePledgeHandler(IValidator<CreatePledgeCommand> validator, LevyTransferMatchingDbContext dbContext)
        {
            _validator = validator;
            _dbContext = dbContext;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(command);

            var result = await _dbContext.AddAsync(new DataModels.Pledge
            {
                Amount = command.Amount,
                CreatedOn = DateTime.UtcNow,
                EmployerAccountId = command.AccountId,
                IsNamePublic = command.IsNamePublic,
                PledgeLevels = command.Levels.Select(x => new DataModels.PledgeLevel { LevelId = (byte)x }).ToList(),
                PledgeRoles = command.JobRoles.Select(x => new DataModels.PledgeRole { RoleId = (byte)x }).ToList(),
                PledgeSectors = command.Sectors.Select(x => new DataModels.PledgeSector { SectorId = (byte)x }).ToList(),
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