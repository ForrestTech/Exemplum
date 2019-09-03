using X.PagedList;

namespace QandA.Features
{
	public static class PagedListExtensions
	{
		public static PageWithMetaData<T> ToPagedListWithMetaData<T>(this IPagedList<T> pagedList)
		{
			return new PageWithMetaData<T>
			{
				Items = pagedList,
				PageDetails = pagedList.GetMetaData()
			};
		}
	}
}