using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountCommandResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public CreateAccountCommandHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateAccountCommandResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var employerAccount = await _dbContext.EmployerAccounts.FindAsync(request.AccountId);

            var created = false;

            if (employerAccount == null)
            {
                await _dbContext.AddAsync(new EmployerAccount
                {
                    Id = request.AccountId,
                    Name = request.AccountName,
                }, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                created = true;
            }

            return new CreateAccountCommandResult
            {
                Created = created
            };
        }
    }
}