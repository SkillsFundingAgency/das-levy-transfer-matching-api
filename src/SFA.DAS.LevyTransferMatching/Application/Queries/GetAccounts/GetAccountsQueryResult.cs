using SFA.DAS.LevyTransferMatching.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAccounts
{
    public class GetAccountsQueryResult
    {
        public List<EmployerAccount> EmployerAccounts { get; set; }

        public class EmployerAccount
        {
            public long Id { get; set; }
            public string Name { get; set; }

            public static implicit operator EmployerAccount(Data.Models.EmployerAccount employerAccount)
            {
                return new EmployerAccount 
                { 
                    Id = employerAccount.Id,
                    Name = employerAccount.Name
                };
            }
        }
    }
}
