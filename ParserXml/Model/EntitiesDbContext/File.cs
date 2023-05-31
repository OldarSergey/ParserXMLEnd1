using System.ComponentModel.DataAnnotations.Schema;

namespace ParserXml.Model.EntitiesDbContext
{
    public class File
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public string VolumeFile { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        [NotMapped]
        public ICollection<Element> Elements { get; set; }
        [NotMapped]
        public ICollection<Logging> Loggings { get; set; }
    }
}
