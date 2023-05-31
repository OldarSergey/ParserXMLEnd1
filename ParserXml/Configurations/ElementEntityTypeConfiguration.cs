using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParserXml.Model.EntitiesDbContext;

namespace ParserXml.Configurations
{
    public class ElementEntityTypeConfiguration : IEntityTypeConfiguration<Element>
    {
        public void Configure(EntityTypeBuilder<Element> builder)
        {
            builder.HasKey(element => element.Id)
                .HasName("PK_Elements_Id");

            builder.Property(element => element.Name)
                .HasColumnType("nvarchar");

            builder.Property(element => element.Description)
               .HasColumnType("nvarchar");

            builder.HasOne(element => element.File)
                .WithMany(file => file.Elements)
                .HasForeignKey(element => element.FileId)
                .HasConstraintName("FK_Elements_FileId_Files_Id")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
