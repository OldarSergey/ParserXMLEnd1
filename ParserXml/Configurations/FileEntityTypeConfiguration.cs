using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParserXml.Model.EntitiesDbContext;
using File = ParserXml.Model.EntitiesDbContext.File;

namespace ParserXml.Configurations
{
    public class FileEntityTypeConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasKey(fail => fail.Id)
                .HasName("PK_Files_Id");

            builder.Property(file => file.FileName)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar");

            builder.Property(file => file.FilePath)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar");

            builder.Property(file => file.VolumeFile)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar");

            builder.Property(file => file.FileExtension)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar");

            builder.HasOne(file => file.User)
                .WithMany(user => user.Files)
                .HasForeignKey(file => file.UserId)
                .HasConstraintName("FK_Users_FileId_Files_Id")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
