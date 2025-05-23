using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextPost.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Infrastructure.Data.Config
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .UseIdentityColumn(1, 1);

            builder.Property(p => p.Title)
                .HasMaxLength(128);

            builder.Property(p => p.Content)
               .HasMaxLength(4000);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .HasOne(p => p.Author)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.AuthorId);

            builder
                .HasMany(p => p.Comments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);

            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.ToTable("Posts");
        }
    }

}
