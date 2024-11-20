using Explorer.BuildingBlocks.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Tests
{
    public class BaseEncountersIntegrationTest : BaseWebIntegrationTest<EncountersTestFactory>
    {
        BaseEncountersIntegrationTest(EncountersTestFactory factory) : base(factory) { }
    }
}
