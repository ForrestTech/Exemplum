using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using QandA.Data;

namespace QandA.Features.Questions
{
	public class CreateQuestionCommand : IRequest<QuestionDTO>
	{
		public string Title { get; set; }

		public string QuestionContent { get; set; }

		public int QuestionerId { get; set; }
	}

	public class CreateQuestionValidator : AbstractValidator<CreateQuestionCommand>
	{
		public CreateQuestionValidator()
		{
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.QuestionContent).NotEmpty();
			RuleFor(x => x.QuestionerId).GreaterThan(0);
		}
	}

	public class Create : IRequestHandler<CreateQuestionCommand, QuestionDTO>
	{
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;

		public Create(DatabaseContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<QuestionDTO> Handle(CreateQuestionCommand command, CancellationToken cancellationToken)
		{
			var question = new Question(command.Title, command.QuestionContent, command.QuestionerId);

			_context.Questions.Add(question);
			await _context.SaveChangesAsync(cancellationToken);

			return _mapper.Map<QuestionDTO>(question);
		}
	}
}