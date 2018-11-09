using System;

namespace theta_bot.Database.Classes
{
    public class InfoModel
    {
        // ReSharper disable InconsistentNaming
        public int level;
        public bool? state = null;
        public DateTime createTime = DateTime.Now;
        public DateTime? answerTime = null;
        public int? timestamp;
        
        public InfoModel(int level) => this.level = level;
    }
}