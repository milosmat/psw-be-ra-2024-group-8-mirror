using Explorer.BuildingBlocks.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.Tests
{
    public class BaseGamesIntegrationTest : BaseWebIntegrationTest<GamesTestFactory>
    {
        BaseGamesIntegrationTest(GamesTestFactory factory) : base(factory) { } 
    }
}
