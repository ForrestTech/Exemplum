using X.PagedList;

namespace QandA.Features
{
	public class PageWithMetaData<T> 
	{
		public IPagedList<T> Items { get; set; }

		public IPagedList PageDetails { get; set; }
	}
}