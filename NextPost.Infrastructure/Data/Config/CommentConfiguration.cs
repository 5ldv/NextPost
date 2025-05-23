using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextPost.Core.Models;

namespace NextPost.Infrastructure.Data.Config
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(p => p.Content)
               .HasMaxLength(800);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.Author)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(p => !p.IsDeleted);


            builder.ToTable("Comments");
        }
    }

}
