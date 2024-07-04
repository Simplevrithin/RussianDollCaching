using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RussianDollCaching.Core
{
    internal interface ICachingFactory
    {
        Doll SetSingleDollCaching<T>(string key, T cachedObject) where T : class;
        T Get<T>(string key) where T : class;
    }
}
