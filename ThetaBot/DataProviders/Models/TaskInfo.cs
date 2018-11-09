using System;

namespace DataProviders.Models
{
    public class TaskInfo
    {
        public string Text;
        public string Answer;
        public string Level;
        public string GeneratorName;
        public DateTime? TimeCreated;
        public int? Timestamp;
        public DateTime? TimeAnswered;
        public bool? IsCorrectAnswer;
    }
}
