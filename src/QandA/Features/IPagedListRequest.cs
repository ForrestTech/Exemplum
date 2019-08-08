namespace QandA.Features
{
	public interface IPagedListRequest
	{
		int PageNumber { get; set; }

		int PageSize { get; set; }
	}
}