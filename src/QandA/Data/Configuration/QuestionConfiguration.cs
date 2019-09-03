using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QandA.Features.Questions;

namespace QandA.Data.Configuration
{
	public class QuestionConfiguration : IEntityTypeConfiguration<Question>
	{
		public void Configure(EntityTypeBuilder<Question> builder)
		{
			builder.HasKey(q => q.Id);

			builder.Property(q => q.Title)
				.HasMaxLength(500)
				.IsRequired();

			builder.Property(q => q.QuestionContent)
				.IsRequired();

			builder.HasOne(s => s.Questioner)
				.WithMany(x => x.Questions)
				.HasForeignKey(p => p.QuestionerId)
				.IsRequired();
		}
	}
}