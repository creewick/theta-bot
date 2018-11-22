using System.Threading.Tasks;
using DataProviders;
using DataProviders.Models;

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
            var progress = await dataProvider.GetUserProgress(userId);
            var level = await dataProvider.GetLevel(progress.LevelName);


        }

        private TaskType GetTaskType(Progress progress, Level level)
        {

        }
    }
}
