using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using QandA.Data;

namespace QandA.Features.Questions
{
	public class CreateQuestionRequest : IRequest<Question>
	{
		public string Title { get; set; }

		public string QuestionContent { get; set; }

		public int QuestionerId { get; set; }
	}

	public class CreateQuestionValidator : AbstractValidator<CreateQuestionRequest>
	{
		public CreateQuestionValidator()
		{
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.QuestionContent).NotEmpty();
			RuleFor(x => x.QuestionerId).GreaterThan(0);
		}
	}

	public class Create : IRequestHandler<CreateQuestionRequest, Question>
	{
		private readonly DatabaseContext _context;

		public Create(DatabaseContext context)
		{
			_context = context;
		}

		public async Task<Question> Handle(CreateQuestionRequest request, CancellationToken cancellationToken)
		{
			var question = new Question(request.Title, request.QuestionContent, request.QuestionerId);

			_context.Questions.Add(question);
			await _context.SaveChangesAsync(cancellationToken);

			return question;
		}
	}
}