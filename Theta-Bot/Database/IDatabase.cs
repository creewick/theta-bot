using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theta_Bot.Database
{
    public interface IDatabase
    {
        Task<List<string>> GetCompletedLevelsAsync(string userId);
        Task SetLevelCompletedAsync(string userId, string levelId);

        Task<string> GetCurrentLevelAsync(string userId);
        Task SetCurrentLevelAsync(string userId, string levelId);

//        Task<Exercise> GetCurrentExerciseAsync(string userId);
//        Task SetCurrentExerciseAsync(string userId, Exercise exercise);
//
//        Task<Dictionary<string, GeneratorState>> GetGeneratorStatesAsync(string userId);
//        Task SetGeneratorStateAsync(string userId, string generatorId, GeneratorState state);
//
//        Task<GeneratorHistory> GetGeneratorHistoryAsync(string generatorId);
//        Task RaiseShowsCountAsync(string generatorsId);
//        Task RaiseCorrectCountAsync(string generatorsId);
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
        generators:
          <id>:
            template: <string>
            max-points: <int>
*/
     
    
}
