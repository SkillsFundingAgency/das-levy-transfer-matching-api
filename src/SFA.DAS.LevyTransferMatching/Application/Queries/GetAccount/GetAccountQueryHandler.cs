using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;

public class GetAccountQueryHandler(LevyTransferMatchingDbContext dbContext) : IRequestHandler<GetAccountQuery, GetAccountQueryResult>
{
    public async Task<GetAccountQueryResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await dbContext.EmployerAccounts
            .SingleOrDefaultAsync(x => x.Id == request.AccountId,
                cancellationToken: cancellationToken);

        if (account == null)
        {
            return null;
        }

        return new GetAccountQueryResult
        {
            AccountId = account.Id,
            AccountName = account.Name
        };
    }
}