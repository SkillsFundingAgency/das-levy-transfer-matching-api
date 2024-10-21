using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;

public class CreateAccountCommandHandler(IEmployerAccountRepository accountRepository) : IRequestHandler<CreateAccountCommand, CreateAccountCommandResult>
{
    public async Task<CreateAccountCommandResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var employerAccount = await accountRepository.Get(request.AccountId);

        var created = false;

        if (employerAccount == null)
        {
            employerAccount = EmployerAccount.New(request.AccountId, request.AccountName);
            await accountRepository.Add(employerAccount);

            created = true;
        }

        return new CreateAccountCommandResult
        {
            Created = created
        };
    }
}