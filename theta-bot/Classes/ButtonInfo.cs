namespace theta_bot
{
    public struct ButtonInfo
    {
        public readonly string Answer;
        public readonly int TaskId;

        public ButtonInfo(string answer, int taskId)
        {
            Answer = answer;
            TaskId = taskId;
        }
    }
}