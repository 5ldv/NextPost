using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Infrastructure.Data.config
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(p => p.Id)
                .UseIdentityColumn();

            builder.Property(p => p.FirstName)
                 .HasMaxLength(64);

            builder.Property(p => p.LastName)
               .HasMaxLength(64);

            builder.Property(p => p.Email)
            .HasMaxLength(128)
            .IsRequired();

            builder.Property(p => p.NormalizedEmail)
            .HasMaxLength(128)
            .IsRequired();

            builder.Property(p => p.Bio)
               .HasMaxLength(256);

            builder.OwnsMany(p => p.RefreshTokens)
                .ToTable("RefreshTokens");

        }
    }
}
