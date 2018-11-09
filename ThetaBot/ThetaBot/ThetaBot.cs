using System;
using System.Threading.Tasks;
using DataProviders;

namespace ThetaBot
{
    public class ThetaBot
    {
        private readonly IDataProvider dataProvider;

        public ThetaBot(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public async Task<string> GetNewTask(string userId)
        {
            var level = await dataProvider.GetUserLevel(userId);
            
            throw new NotImplementedException();
        }
    }
}
