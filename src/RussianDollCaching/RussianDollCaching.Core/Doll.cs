namespace RussianDollCaching.Core
{
    public class Doll
    {
        public Doll()
        {
        }

        public Doll(string aggregateKey, string dollKey)
        {
            AggregateKey = aggregateKey;
            DollKey = dollKey;
            NestedDolls = new HashSet<Doll>();
            PropertyCached = new Dictionary<string, object>();
        }

        public string AggregateKey { get; set; }

        public string DollKey { get; set; }

        public string PropertyName { get; set; }

        public HashSet<Doll> NestedDolls { get; set; }

        public Dictionary<string, object> PropertyCached { get; set; }
    }
}
