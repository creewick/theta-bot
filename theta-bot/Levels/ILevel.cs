using System;

namespace theta_bot
{
    public interface ILevel
    {
        bool IsFinished(IDataProvider data, long chatId);
        Exercise Generate(Random random);
    }
}