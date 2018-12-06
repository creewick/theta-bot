using System.Configuration;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Theta_Bot.Database;

namespace Theta_Bot
{
    [TestFixture]
    public class FirebaseDataProviderTests
    {
        [Test]
        public async Task GetCompletedLevels_ShouldReturnDictionary()
        {
            var data = new FirebaseDataProvider(
                ConfigurationManager.ConnectionStrings["DatabaseAddress"].ToString(),
                ConfigurationManager.ConnectionStrings["DatabaseToken"].ToString());

            var levels = await data.GetCompletedLevelsAsync("1");
            levels.Should().ContainKey("level1");
            levels["level1"].Should().BeTrue();
        }
    }
}
