using Microsoft.AspNetCore.Identity;

namespace ParserXml.Model.EntitiesDbContext
{
    public class User : IdentityUser<int>
    {
        public bool IsDeleted { get; set; }

        public ICollection<File> Files { get; set; }
        public ICollection<Logging> Loggings { get; set; }

    }
}
