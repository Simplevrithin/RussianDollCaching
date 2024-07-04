using RussianDollCaching.Core;
using RussianDollCaching.Tests.Fixtures;

namespace RussianDollCaching.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var cacheKey = Guid.NewGuid().ToString();
            var cachedObject = new DollTestSample
            {
                Id = cacheKey,
                Name = "test"
            };

            var cachingFactory = new MemoryCaching();
            var dollCaching = cachingFactory.SetSingleDollCaching(cacheKey, cachedObject);
            Assert.Equal(dollCaching.AggregateKey, cacheKey);

            var cached = cachingFactory.Get<DollTestSample>(cacheKey);
            Assert.Equal(cachedObject, cached);
        }
    }
}