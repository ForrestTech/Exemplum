using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QandA.Data;

namespace QandA.Features.Questions
{
	public class GetQuestionRequest : IRequest<Question>
	{
		public int QuestionId { get; set; }
	}

	public class Get : IRequestHandler<GetQuestionRequest, Question>
	{
		private readonly DatabaseContext _context;

		public Get(DatabaseContext context)
		{
			_context = context;
		}

		public Task<Question> Handle(GetQuestionRequest request, CancellationToken cancellationToken)
		{
			return _context.Questions.SingleOrDefaultAsync(x => x.Id == request.QuestionId, cancellationToken);
		}
	}
}