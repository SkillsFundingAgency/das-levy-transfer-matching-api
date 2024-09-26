using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Services.Audit;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Services.Audit;

[TestFixture]
public class DiffServiceTests
{
    private DiffServiceTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new DiffServiceTestsFixture();
    }

    [Test]
    public void IdenticalItemsProducesEmptyDiff()
    {
        _fixture.WithIdenticalItems().GenerateDiff();
        _fixture.Result.Should().BeEmpty();
    }

    [Test]
    public void DifferentItemsAreReturned()
    {
        _fixture.WithRandomInitialItem().WithDifferentUpdatedValues().GenerateDiff();

        foreach (var item in _fixture.InitialItem)
        {
            var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
            resultItem.InitialValue.Should().Be(item.Value);
            resultItem.UpdatedValue.Should().Be(_fixture.UpdatedItem[item.Key]);
        }
    }

    [Test]
    public void ComparisonToNullInitialStateReturnsAllItemsInUpdated()
    {
        _fixture.WithNullInitialItem().WithRandomUpdatedItem().GenerateDiff();

        _fixture.Result.Count.Should().Be(_fixture.UpdatedItem.Count);
        
        foreach (var item in _fixture.UpdatedItem)
        {
            var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
            resultItem.InitialValue.Should().BeNull();
            resultItem.UpdatedValue.Should().Be(item.Value);
        }
    }

    [Test]
    public void ComparisonToInitialItemWithNullValuesReturnsAllItemsInUpdated()
    {
        _fixture.WithInitialItemsWithNullValues().WithDifferentUpdatedValues().GenerateDiff();

        _fixture.Result.Count.Should().Be(_fixture.UpdatedItem.Count);
        
        foreach (var item in _fixture.UpdatedItem)
        {
            var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
            resultItem.InitialValue.Should().BeNull();
            resultItem.UpdatedValue.Should().Be(item.Value);
        }
    }

    [Test]
    public void ComparisonToNullUpdatedStateReturnsAllItemsInInitial()
    {
        _fixture.WithRandomInitialItem().WithNullUpdatedItem().GenerateDiff();

        _fixture.Result.Count.Should().Be(_fixture.InitialItem.Count);
        
        foreach (var item in _fixture.InitialItem)
        {
            var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
            resultItem.UpdatedValue.Should().BeNull();
            resultItem.InitialValue.Should().Be(item.Value);
        }
    }

    private class DiffServiceTestsFixture
    {
        private readonly Fixture _autoFixture;
        private readonly DiffService _diffService;
        public IReadOnlyList<DiffItem> Result { get; private set; }
        public Dictionary<string, object> InitialItem;
        public Dictionary<string, object> UpdatedItem;

        public DiffServiceTestsFixture()
        {
            _autoFixture = new Fixture();
            _diffService = new DiffService();

            InitialItem = null;
            UpdatedItem = null;
        }

        public DiffServiceTestsFixture WithNullInitialItem()
        {
            InitialItem = null;
            return this;
        }

        public DiffServiceTestsFixture WithInitialItemsWithNullValues()
        {
            InitialItem = GenerateRandomDataWithNullValues();
            return this;
        }

        public DiffServiceTestsFixture WithNullUpdatedItem()
        {
            UpdatedItem = null;
            return this;
        }

        public DiffServiceTestsFixture WithRandomInitialItem()
        {
            InitialItem = GenerateRandomData();
            return this;
        }

        public DiffServiceTestsFixture WithRandomUpdatedItem()
        {
            UpdatedItem = GenerateRandomData();
            return this;
        }

        public DiffServiceTestsFixture WithDifferentUpdatedValues()
        {
            UpdatedItem = GenerateModifiedData(InitialItem);
            return this;
        }

        public DiffServiceTestsFixture WithIdenticalItems()
        {
            InitialItem = GenerateRandomData();
            UpdatedItem = InitialItem;
            return this;
        }

        public void GenerateDiff()
        {
            Result = _diffService.GenerateDiff(InitialItem, UpdatedItem);
        }

        private Dictionary<string, object> GenerateRandomData()
        {
            var result = new Dictionary<string, object>();
            for (var i = 0; i < 10; i++)
            {
                result.Add(_autoFixture.Create<string>(), _autoFixture.Create<string>());
                result.Add(_autoFixture.Create<string>(), _autoFixture.Create<long>());
                result.Add(_autoFixture.Create<string>(), _autoFixture.Create<DateTime>());
            }
            return result;
        }

        private Dictionary<string, object> GenerateRandomDataWithNullValues()
        {
            var result = new Dictionary<string, object>();
            for (var index = 0; index < 10; index++)
            {
                result.Add(_autoFixture.Create<string>(), null);
            }
            return result;
        }

        private static Dictionary<string, object> GenerateModifiedData(Dictionary<string, object> source)
        {
            var result = new Dictionary<string, object>();

            foreach (var sourceItem in source)
            {
                switch (sourceItem.Value)
                {
                    case null:
                        result.Add(sourceItem.Key, "modified");
                        continue;
                    case string stringValue:
                        result.Add(sourceItem.Key, stringValue + "_modified");
                        continue;
                    case long longValue:
                        result.Add(sourceItem.Key, longValue + 1);
                        continue;
                    case DateTime dateTimeValue:
                        result.Add(sourceItem.Key, dateTimeValue.AddDays(1));
                        continue;
                }
            }

            return result;
        }
    }
}