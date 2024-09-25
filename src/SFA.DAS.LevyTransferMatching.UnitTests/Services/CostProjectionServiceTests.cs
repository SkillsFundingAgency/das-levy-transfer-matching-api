using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Services;

[TestFixture]
public class CostProjectionServiceTests
{
    private CostProjectionServiceTestsFixture _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new CostProjectionServiceTestsFixture();
    }

    [Test]
    public void GetCostProjections_Produces_Expected_Output()
    {
        _fixture
            .WithTotalAmount(17000)
            .WithStartDate(new DateTime(2023, 3, 1))
            .WithDuration(24)
            .Calculate();

        _fixture.Verify("2022/23", null);
        _fixture.Verify("2023/24", 6800);
        _fixture.Verify("2024/25", 6800);
        _fixture.Verify("2025/26", 3400);
        _fixture.Verify("2026/27", null);
    }

    private class CostProjectionServiceTestsFixture
    {
        private readonly CostProjectionService _costProjectionService;
        private int _amount;
        private DateTime _startDate;
        private int _duration;
        private List<CostProjection> _result;

        public CostProjectionServiceTestsFixture()
        {
            _costProjectionService = new CostProjectionService();
        }

        public CostProjectionServiceTestsFixture WithTotalAmount(int amount)
        {
            _amount = amount;
            return this;
        }

        public CostProjectionServiceTestsFixture WithStartDate(DateTime startDate)
        {
            _startDate = startDate;
            return this;
        }

        public CostProjectionServiceTestsFixture WithDuration(int duration)
        {
            _duration = duration;
            return this;
        }

        public void Calculate()
        {
            _result = _costProjectionService.GetCostProjections(_amount, _startDate, _duration);
        }

        public void Verify(string financialYear, decimal? amount)
        {
            if (amount.HasValue)
            {
                var yearValue = _result.Single(x => x.FinancialYear == financialYear);
                Assert.That(yearValue.Amount, Is.EqualTo(amount));
            }
            else
            {
                _result.Exists(x => x.FinancialYear == financialYear).Should().BeFalse();
            }
        }
    }
}