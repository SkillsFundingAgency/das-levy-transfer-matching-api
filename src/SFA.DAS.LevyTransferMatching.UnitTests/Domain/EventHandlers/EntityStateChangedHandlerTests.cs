using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers;

[TestFixture]
public class EntityStateChangedHandlerTests : LevyTransferMatchingDbContextFixture
{
    private EntityStateChangedHandler _handler;
    private Mock<IDiffService> _diffService;
    private readonly Fixture _fixture = new();
    private EntityStateChanged _event;
    private List<DiffItem> _diffResult;

    [SetUp]
    public void Setup()
    {
        _event = _fixture.Create<EntityStateChanged>();
        _event.InitialState = "{ }";
        _event.UpdatedState = "{ }";

        _diffResult = [new() { InitialValue = "initial", PropertyName = "myproperty", UpdatedValue = "updated" }];

        _diffService = new Mock<IDiffService>();
        _diffService.Setup(x =>
                x.GenerateDiff(It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<string, object>>()))
            .Returns(_diffResult);

        _handler = new EntityStateChangedHandler(DbContext, _diffService.Object);
    }

    [Test]
    public async Task Handle_Adds_Audit_Record()
    {
        await _handler.Handle(_event);

        await DbContext.SaveChangesAsync();

        var audit = await DbContext.Audits.FirstAsync();

        _event.EntityId.Should().Be(audit.EntityId);
        _event.EntityType.Should().Be(audit.EntityType);
        _event.UserId.Should().Be(audit.UserId);
        _event.UserDisplayName.Should().Be(audit.UserDisplayName);
        _event.UserAction.ToString().Should().Be(audit.UserAction);
        DateTime.UtcNow.Date.Should().Be(audit.AuditDate.Date);
        _event.InitialState.Should().Be(audit.InitialState);
        _event.UpdatedState.Should().Be(audit.UpdatedState);
        JsonConvert.SerializeObject(_diffResult).Should().Be(audit.Diff);
        _event.CorrelationId.Should().Be(audit.CorrelationId);
    }

    [Test]
    public async Task Handle_Does_Not_Add_Audit_Record_If_Diff_Is_Empty()
    {
        _diffResult.Clear();

        await _handler.Handle(_event);

        await DbContext.SaveChangesAsync();
        var audit = await DbContext.Audits.FirstOrDefaultAsync();
        audit.Should().BeNull();
    }
}