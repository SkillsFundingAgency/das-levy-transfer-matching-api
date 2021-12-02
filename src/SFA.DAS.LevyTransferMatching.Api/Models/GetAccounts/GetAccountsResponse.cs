using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccounts;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetAccounts
{
    public class GetAccountsResponse
    {
        public List<EmployerAccount> EmployerAccounts { get; set; }

        public class EmployerAccount
        {
            public long Id { get; set; }
            public string Name { get; set; }

            public static implicit operator EmployerAccount(GetAccountsQueryResult.EmployerAccount employerAccount)
            {
                return new EmployerAccount
                {
                    Id = employerAccount.Id,
                    Name = employerAccount.Name
                };
            }
        }

        public static implicit operator GetAccountsResponse(GetAccountsQueryResult getAccountsQueryResult)
        {
            return new GetAccountsResponse
            {
                EmployerAccounts = getAccountsQueryResult.EmployerAccounts.Select(x => (EmployerAccount)x).ToList()
            };
        }
    }
}
