namespace Exemplum.Application.Common.Mapping;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pagination;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken)
        => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable,
        IConfigurationProvider configuration, CancellationToken cancellationToken)
        => queryable.ProjectTo<TDestination>(configuration).ToListAsync(cancellationToken);
}