using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParserXml.Data;
using ParserXml.Model.EntitiesDbContext;
using File = ParserXml.Model.EntitiesDbContext.File;

namespace ParserXml.Service
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _context;

        public FileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFileAsync(File newFile)
        {
            _context.Files.Add(newFile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
            file.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        public string FindFilePath(string fileName)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            string[] files = Directory.GetFiles(basePath, fileName, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                return files[0];
            }

            return null;
        }
    }
}
