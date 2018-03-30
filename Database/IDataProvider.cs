namespace theta_bot
{
    public interface IDataProvider
    {
        int AddTask(int chatId, string answer);
        string GetAnswer(int taskId);
    }
}