using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QandA.Features.Users;

namespace QandA.Data.Configuration
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(q => q.Id);

			builder.Property(x => x.Created)
				.IsRequired();

			builder.Property(x => x.LastUpdated)
				.IsRequired();

			builder.Property(x => x.Username)
				.IsRequired()
				.HasMaxLength(300);

			builder.HasIndex(u => u.Username)
				.IsUnique();

			builder.Property(x => x.Email)
				.IsRequired()
				.HasMaxLength(300);

			builder.HasIndex(u => u.Email)
				.IsUnique();
		}
	}
}