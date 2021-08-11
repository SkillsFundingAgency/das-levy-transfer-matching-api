using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers
{
    [TestFixture]
    public class EntityStateChangedHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private EntityStateChangedHandler _handler;
        private Mock<IDiffService> _diffService;
        private readonly Fixture _fixture = new Fixture();
        private EntityStateChanged _event;
        private List<DiffItem> _diffResult;

        [SetUp]
        public void Setup()
        {
            ResetDbContext();

            _event = _fixture.Create<EntityStateChanged>();
            _event.InitialState = "{ }";
            _event.UpdatedState = "{ }";

            _diffResult = new List<DiffItem>()
                {new DiffItem {InitialValue = "initial", PropertyName = "myproperty", UpdatedValue = "updated"}};

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

            Assert.AreEqual(audit.EntityId, _event.EntityId);
            Assert.AreEqual(audit.EntityType, _event.EntityType);
            Assert.AreEqual(audit.UserId, _event.UserId);
            Assert.AreEqual(audit.UserDisplayName, _event.UserDisplayName);
            Assert.AreEqual(audit.UserAction, _event.UserAction.ToString());
            Assert.AreEqual(audit.AuditDate.Date, DateTime.UtcNow.Date);
            Assert.AreEqual(audit.InitialState, _event.InitialState);
            Assert.AreEqual(audit.UpdatedState, _event.UpdatedState);
            CollectionAssert.AreEqual(audit.Diff, JsonConvert.SerializeObject(_diffResult));
            Assert.AreEqual(audit.CorrelationId, _event.CorrelationId);
        }

        [Test]
        public async Task Handle_Does_Not_Add_Audit_Record_If_Diff_Is_Empty()
        {
            _diffResult.Clear();

            await _handler.Handle(_event);
            
            await DbContext.SaveChangesAsync();
            var audit = await DbContext.Audits.FirstOrDefaultAsync();
            Assert.IsNull(audit);
        }
    }
}
