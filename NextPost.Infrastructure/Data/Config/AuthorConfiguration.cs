using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextPost.Core.Models;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .UseIdentityColumn(1, 1);

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.Property(p => p.FirstName)
            .HasMaxLength(64);

        builder.Property(p => p.LastName)
           .HasMaxLength(64);

        builder.Property(p => p.Bio)
            .HasMaxLength(256);

        builder.Property(p => p.Location)
        .HasMaxLength(32);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder
            .HasOne(p => p.User)
            .WithOne(p => p.Author)
            .HasForeignKey<Author>(x => x.UserId)
            .IsRequired();

        builder.ToTable("Authors");
    }
}
