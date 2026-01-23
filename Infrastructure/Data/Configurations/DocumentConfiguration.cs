using Domain.Entities.Documents;

namespace Infrastructure.Data.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd()
            .ConfigureGuid(isRequired: true);

        builder.Property(d => d.UserId)
            .ConfigureGuid(isRequired: true);

        builder.Property(d => d.Type)
            .ConfigureString(100, isRequired: true);

        builder.Property(d => d.Status)
            .IsRequired();

        builder.Property(d => d.FilePath)
            .ConfigureString(500, isRequired: true);

        builder.Property(d => d.FileName)
            .ConfigureString(255, isRequired: true);

        builder.Property(d => d.ContentType)
            .ConfigureString(120, isRequired: true);

        builder.Property(d => d.Size)
            .IsRequired();

        builder.HasIndex(d => d.UserId)
            .HasDatabaseName("ix_documents_user_id");
    }
}

