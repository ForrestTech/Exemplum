namespace Exemplum.Infrastructure.Persistence
{
    using Domain.Common;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class SoftDeleteQueryExtension
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension).GetMethod(nameof(GetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);

            var filter = methodToCall?.Invoke(null, new object[] { });

            if (filter != null)
            {
                entityData.SetQueryFilter((LambdaExpression)filter);
            }

            entityData.AddIndex(entityData.FindProperty(nameof(ISoftDelete.IsDeleted)));
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }
}