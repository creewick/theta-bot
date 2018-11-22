using System.Collections.Generic;

namespace DataProviders.Models
{
    public class ProgressInfo
    {
        public string LevelId;
        public ExerciseInfo CurrentExercise;
        public Dictionary<string, GeneratorInfo> Generators;
    }
}
