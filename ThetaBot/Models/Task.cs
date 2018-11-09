using System;
using System.Collections.Generic;

namespace Models
{
    public class Task
    {
        public string Text;
        public List<string> Variants;
        public string Answer;
        public string LevelId;
        public string TypeId;

        public DateTime? TimeCreated;
        public int? Timestamp;
        public DateTime? TimeAnswered;
        public bool? IsCorrectAnswer;
    }
}
