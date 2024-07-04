using MessagePack;
using Microsoft.Extensions.Caching.Memory;
using RussianDollCaching.Attributes;
using System.Collections.Concurrent;
using System.Reflection;

namespace RussianDollCaching.Core
{
    public class MemoryCaching : ICachingFactory
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCaching()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions()
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(1)
            });
        }

        public Doll SetSingleDollCaching<T>(string key, T cachedObject) where T : class
        {
            var dollCaching = new DollCachingBuilder<T>(key)
                .Init(cachedObject)
                .Build();

            var props = cachedObject.GetType().GetProperties();
            foreach (var prop in props)
            {
                var nestedDollAttribute = prop.GetCustomAttribute(typeof(NestedDollAttribute)) as NestedDollAttribute;
                if (nestedDollAttribute != null && nestedDollAttribute.SingleObject)
                {
                    var nestedDollValue = prop.GetValue(cachedObject, null);
                    if (nestedDollValue == null)
                    {
                        continue;
                    }
                    
                    if (nestedDollValue is IEnumerable<object>)
                    {
                        dollCaching.PropertyCached.Add(prop.Name, nestedDollValue);
                        continue;
                    }
                    var nestedProps = nestedDollValue.GetType().GetProperties();
                    foreach (var nestedProp in nestedProps)
                    {
                        
                    }
                    var nestedDoll = SetSingleDollCaching(key, nestedDollValue);
                    dollCaching.NestedDolls.Add(nestedDoll);
                }
            }
            var existed = _memoryCache.Get(key);
            if (existed != null)
            {
                _memoryCache.Remove(key);
            }

            _memoryCache.Set(key, dollCaching);
            return dollCaching;
        }

        public T Get<T>(string key) where T : class
        {
            var cachedObject = _memoryCache.Get<Doll>(key);
            if (cachedObject == null)
            {
                return null;
            }

            var instance = Activator.CreateInstance(typeof(T));
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var propNestedDollAttribute = prop.GetCustomAttribute(typeof(NestedDollAttribute)) as NestedDollAttribute;
                if (propNestedDollAttribute != null && propNestedDollAttribute.SingleObject)
                {
                    var childInstance = Activator.CreateInstance(prop.GetType());
                    if (childInstance == null)
                    {
                        continue;
                    }

                    var nestedProps = childInstance.GetType().GetProperties();
                    foreach (var nestedProp in nestedProps)
                    {
                        var cachedNested = cachedObject.NestedDolls.SingleOrDefault(x => x.PropertyName == nestedProp.Name);
                        if (cachedNested != null)
                        {
                            var nestedObject = _memoryCache.Get<Doll>(cachedNested.DollKey);
                            if (nestedObject != null)
                            {
                                var val = nestedObject.PropertyCached.Where(x => x.Key ==  nestedProp.Name);
                                if(val != null)
                                {
                                    nestedProp.SetValue(childInstance, val);
                                }
                            }
                        }
                    }
                    prop.SetValue(instance, childInstance);
                }
            }

            return instance as T;
        }
    }
}
