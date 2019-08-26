using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QandA.Data;
using X.PagedList;

namespace QandA.Features.Questions
{

	public class ListQuestionRequest : IRequest<PageWithMetaData<Question>>, IPagedListRequest
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}

	public class List : IRequestHandler<ListQuestionRequest, PageWithMetaData<Question>>
	{
		private readonly DatabaseContext _context;

		public List(DatabaseContext context)
		{
			_context = context;
		}

		public async Task<PageWithMetaData<Question>> Handle(ListQuestionRequest request, CancellationToken cancellationToken)
		{
			var page = await _context.Questions.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

			return new PageWithMetaData<Question>
			{
				Items = page,
				PageDetails = page.GetMetaData()
			};
		}
	}
}