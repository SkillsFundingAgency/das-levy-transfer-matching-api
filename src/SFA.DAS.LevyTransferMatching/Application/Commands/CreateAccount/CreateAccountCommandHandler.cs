using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountCommandResult>
    {
        private readonly IEmployerAccountRepository _accountRepository;

        public CreateAccountCommandHandler(IEmployerAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<CreateAccountCommandResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var employerAccount = await _accountRepository.Get(request.AccountId);

            var created = false;

            if (employerAccount == null)
            {
                await _accountRepository.Add(new EmployerAccount
                {
                    Id = request.AccountId,
                    Name = request.AccountName,
                });

                created = true;
            }

            return new CreateAccountCommandResult
            {
                Created = created
            };
        }
    }
}