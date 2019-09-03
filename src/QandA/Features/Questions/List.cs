using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using QandA.Data;
using X.PagedList;

namespace QandA.Features.Questions
{

	public class ListQuestionRequest : IRequest<PageWithMetaData<QuestionDTO>>, IPagedListRequest
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}

	public class List : IRequestHandler<ListQuestionRequest, PageWithMetaData<QuestionDTO>>
	{
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;

		public List(DatabaseContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<PageWithMetaData<QuestionDTO>> Handle(ListQuestionRequest request, CancellationToken cancellationToken)
		{
			var page = await _context.Questions
				.ProjectTo<QuestionDTO>(_mapper.ConfigurationProvider)
				.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

			return page.ToPagedListWithMetaData();
		}
	}
}