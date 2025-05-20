using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextPost.Core.Models.Identity;

namespace NextPost.Infrastructure.Data.config
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.Property(p => p.Id)
                .UseIdentityColumn();

        }
    }
}
