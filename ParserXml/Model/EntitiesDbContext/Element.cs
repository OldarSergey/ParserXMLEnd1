namespace ParserXml.Model.EntitiesDbContext
{
    public class Element
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int FileId { get; set; }
        public File File { get; set; }
    }
}
