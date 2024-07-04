using RussianDollCaching.Attributes;

namespace RussianDollCaching.Tests.Fixtures
{
    public class DollTestSample
    {
        [DollKey]
        public string Id { get; set; }

        public string Name { get; set; }

        [NestedDoll(_SingleObject: true)]
        public DollTest NestedDoll { get; set; }
    }
}
