using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParserXml.Model.EntitiesDbContext;

namespace ParserXml.Configurations
{
    public class LoggingEntityTypeConfiguration : IEntityTypeConfiguration<Logging>
    {
        public void Configure(EntityTypeBuilder<Logging> builder)
        {
            builder.HasKey(logging => logging.Id)
                .HasName("PK_Logging_Id");

            builder.Property(logging => logging.Date)
                .HasColumnType("datetime");

            builder.HasOne(logging => logging.User)
                .WithMany(user => user.Loggings)
                .HasForeignKey(logging => logging.UserId)
                .HasConstraintName("FK_Loggings_UserId_Users_Id")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(logging => logging.File)
                .WithMany(file => file.Loggings)
                .HasForeignKey(logging => logging.FileId)
                .HasConstraintName("FK_Loggings_FileId_Files_Id")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
