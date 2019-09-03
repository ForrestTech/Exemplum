using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QandA.Features.Questions;

namespace QandA.Data.Configuration
{
	public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
	{
		public void Configure(EntityTypeBuilder<Answer> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(q => q.Content)
				.IsRequired();

			builder.HasOne(s => s.Answerer)
				.WithMany(x => x.Answers)
				.HasForeignKey(p => p.AnswererId)
				.IsRequired();

			builder.HasOne(s => s.Question)
				.WithMany(x => x.Answers)
				.HasForeignKey(p => p.QuestionId)
				.IsRequired();
		}
	}
}