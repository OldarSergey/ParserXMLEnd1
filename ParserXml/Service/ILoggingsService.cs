using ParserXml.Model.EntitiesDbContext;

namespace ParserXml.Service
{
    public interface ILoggingsService
    {
        public Task<List<Logging>> GetLoggingsByUserName(string userName);
        public Task AddLoggingAsync(Logging newLog);

    }
}
