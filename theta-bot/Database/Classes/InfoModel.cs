using System;

namespace theta_bot
{
    public class InfoModel
    {
        public int Level;
        public bool? State = null;
        public DateTime CreateTime = DateTime.Now;
        public DateTime? AnswerTime = null;

        public InfoModel(int level) => Level = level;
    }
}