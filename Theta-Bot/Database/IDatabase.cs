using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theta_Bot.Database
{
    public interface IDatabase
    {
        Task<HashSet<string>> GetCompletedLevelsAsync(string userId);
        Task SetLevelCompletedAsync(string userId, string levelId);

        Task<string> GetCurrentLevelAsync(string userId);
        Task SetCurrentLevelAsync(string userId, string levelId);
//        
//        Task<List<Level>> 
    }
    
/*
    users:
      <id>:
        completed-levels:
          [0]: <id>
          ...
        current-level:
          id: <id>
          current-task:
            id: <id>
            generator-id: <id>
            answer: <string>
          generators:
            <id>:
              template: <string>
              max-points: <int>
              points: <int>
    
    stats:
      <generator-id>:
        shows: <int>
        correct: <int>
        answers:
          [0]:
            answer: <string>
            shows: <int>
            correct: <int>
            is-correct: <bool>
            
    levels:
      <id>:
        name: <string>
        requires:
          [0]: <level-id>
          ...

    tasks:
      <level-id>:
        generators:
          <id>:
            template: <string>
            max-points: <int>
*/
     
    
}
