using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetAccount;

[TestFixture]
public class GetAccountQueryHandlerTests : LevyTransferMatchingDbContextFixture
{
    private Fixture _fixture;
    private EmployerAccount _account;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _account = _fixture.Create<EmployerAccount>();
        DbContext.EmployerAccounts.Add(_account);
        DbContext.SaveChanges();
    }

    [Test]
    public async Task Handle_Account_Is_Returned()
    {
        var handler = new GetAccountQueryHandler(DbContext);
        var query = new GetAccountQuery { AccountId = _account.Id };

        var result = await handler.Handle(query, CancellationToken.None);

        result.AccountId.Should().Be(_account.Id);
        result.AccountName.Should().Be(_account.Name);
    }

    [Test]
    public async Task Handle_Returns_Null_If_Account_Is_Not_Found()
    {
        var handler = new GetAccountQueryHandler(DbContext);
        var query = new GetAccountQuery { AccountId = _account.Id + 1 };

        var result = await handler.Handle(query, CancellationToken.None);
        result.Should().BeNull();
    }
}