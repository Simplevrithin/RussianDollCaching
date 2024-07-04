using RussianDollCaching.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RussianDollCaching.Core
{
    public class DollCachingBuilder<T> where T : class
    {
        private Doll _doll;
        private T _cachedObject;

        public DollCachingBuilder(string aggregateKey)
        {
            _doll = new Doll(aggregateKey, aggregateKey);
            _doll.AggregateKey = aggregateKey;
        }

        public DollCachingBuilder<T> Init(T cacheObject)
        {
            AssertObject(cacheObject);
            _cachedObject = cacheObject;
            return this;
        }

        public Doll Build()
        {
            return _doll;
        }

        private void AssertObject(T cacheObject)
        {
            if (cacheObject == null) 
            {
                throw new Exception("cached object cannot be null");
            }

            var idProps = cacheObject.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(DollKeyAttribute)) != null);
            if(idProps.Count() > 1)
            {
                throw new Exception("Every doll must have only one key");
            }

            if (idProps.Count() == 0) 
            {
                throw new Exception("Every doll must have key");
            }
        }
    }
}
