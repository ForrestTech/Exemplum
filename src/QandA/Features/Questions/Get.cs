using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QandA.Data;

namespace QandA.Features.Questions
{
	public class GetQuestionRequest : IRequest<QuestionDTO>
	{
		public int QuestionId { get; set; }
	}

	public class Get : IRequestHandler<GetQuestionRequest, QuestionDTO>
	{
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;

		public Get(DatabaseContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<QuestionDTO> Handle(GetQuestionRequest request, CancellationToken cancellationToken)
		{
			var question = await _context.Questions
				.ProjectTo<QuestionDTO>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync(x => x.Id == request.QuestionId, cancellationToken);

			return question;
		}
	}
}