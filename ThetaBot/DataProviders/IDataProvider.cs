using System.Collections.Generic;
using System.Threading.Tasks;
using DataProviders.Models;

namespace DataProviders
{
    public interface IDataProvider
    {
        Task<IEnumerable<string>> GetCompletedLevels(string userId);
        Task AddCompletedLevels(string userId, IEnumerable<string> levels);
        Task<ProgressInfo> GetCurrentProgress(string userId);
        Task SetCurrentProgress(string userId, ProgressInfo progress);
    }
}
