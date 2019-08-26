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
				.HasMaxLength(Constants.UsernameMaxLength);

			builder.HasIndex(u => u.Username)
				.IsUnique();

			builder.Property(x => x.Email)
				.IsRequired()
				.HasMaxLength(Constants.EmailMaxLength);

			builder.HasIndex(u => u.Email)
				.IsUnique();
		}

		public static class Constants
		{
			public const int UsernameMaxLength = 300;
			public const int EmailMaxLength = 300;
		}
	}
}