﻿using AutoFixture;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    [TestFixture]
    public class CreatePledgeHandlerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Pledge_Created_Id_Returned()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase("SFA.DAS.LevyTransferMatching.Database")
                .Options;

            var mockValidator = new Mock<IValidator<CreatePledgeCommand>>();
            var dbContext = new LevyTransferMatchingDbContext(options);
            var createPledgeHandler = new CreatePledgeHandler(mockValidator.Object, dbContext);

            var command = _fixture.Create<CreatePledgeCommand>();
            var expectedId = 1;

            // Act
            var result = await createPledgeHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, expectedId);
        }
    }
}