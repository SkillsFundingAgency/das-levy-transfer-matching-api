using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;

public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, GetAccountQueryResult>
{
    private readonly LevyTransferMatchingDbContext _dbContext;

    public GetAccountQueryHandler(LevyTransferMatchingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAccountQueryResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.EmployerAccounts
            .SingleOrDefaultAsync(x => x.Id == request.AccountId,
                cancellationToken: cancellationToken);

        if (account == null) return null;

        return new GetAccountQueryResult
        {
            AccountId = account.Id,
            AccountName = account.Name
        };
    }
}