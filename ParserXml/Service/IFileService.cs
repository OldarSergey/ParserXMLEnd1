using ParserXml.Model.EntitiesDbContext;
using File = ParserXml.Model.EntitiesDbContext.File;
namespace ParserXml.Service
{
    public interface IFileService
    {
        public Task AddFileAsync(File newFile);
        public Task DeleteFileAsync(int fileId);
        public string FindFilePath(string fileName);



    }
}
