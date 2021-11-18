namespace Exemplum.Application.Common.Pagination;

public interface IPaginatedQuery
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
}