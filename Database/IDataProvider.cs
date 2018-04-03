namespace theta_bot
{
    public interface IDataProvider
    {
        int AddTask(long chatId, string answer);
        string GetAnswer(int taskId);
        void SetSolved(int chatId, bool solved);
    }
}