using System;
using Telegram.Bot.Types;

namespace theta_bot
{
    public interface ILevel
    {
        bool IsFinished(Contact person);
        Exercise Generate(Random random);
    }
}