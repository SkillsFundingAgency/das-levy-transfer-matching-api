namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;

public class GetAccountQuery : IRequest<GetAccountQueryResult>
{
    public long AccountId { get; set; }
}