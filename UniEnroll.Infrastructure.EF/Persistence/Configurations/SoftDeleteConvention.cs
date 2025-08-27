
// UniEnroll.Infrastructure.EF/Persistence/Configurations/SoftDeleteConvention.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations;

public static class SoftDeleteConvention
{
    public static void ApplySoftDelete<TEntity>(this EntityTypeBuilder<TEntity> b) where TEntity : class
    {
        b.Property<bool>("IsDeleted").HasDefaultValue(false);
        b.Property<DateTimeOffset?>("DeletedAt");
        b.Property<string?>("DeletedBy").HasMaxLength(128);
        //b.HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
    }

    public static void ApplyToModel(ModelBuilder modelBuilder)
    {
        foreach (var et in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(et.ClrType))
            {
                var builder = modelBuilder.Entity(et.ClrType);
                builder.Property<bool>("IsDeleted").HasDefaultValue(false);
                builder.Property<DateTimeOffset?>("DeletedAt");
                builder.Property<string?>("DeletedBy").HasMaxLength(128);
                //builder.HasQueryFilter(
                //    LambdaExpressionBuilder.BuildBoolFilter(et.ClrType, "IsDeleted", expected: false));
            }
        }
    }
}


//public static class LambdaExpressionBuilder
//{
//    public static LambdaExpression BuildBoolFilter(Type entityType, string shadowName, bool expected)
//    {
//        var p = Expression.Parameter(entityType, "e");
//        var body = Expression.Equal(
//            Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, p, Expression.Constant(shadowName)),
//            Expression.Constant(expected));
//        return Expression.Lambda(body, p);
//    }
//}
