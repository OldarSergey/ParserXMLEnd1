namespace ParserXml.Model.EntitiesDbContext
{
    public class Logging
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public int FileId { get; set; }

        public User User { get; set; }
        public File File { get; set; }
    }
}
