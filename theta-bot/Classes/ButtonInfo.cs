namespace theta_bot.Classes
{
    public struct ButtonInfo
    {
        public readonly string Answer;
        public readonly string TaskKey;

        public ButtonInfo(string answer, string taskKey)
        {
            Answer = answer;
            TaskKey = taskKey;
        }
    }
}