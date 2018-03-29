namespace theta_bot
{
    public interface IDataProvider
    {
        string GetAnswer(long chatId);
        void StoreAnswer(long chatId, string answer);
    }
}