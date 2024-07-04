using RussianDollCaching.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RussianDollCaching.Tests.Fixtures
{
    public class DollTest
    {
        [DollKey]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
