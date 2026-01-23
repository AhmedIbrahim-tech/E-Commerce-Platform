using System.Linq.Expressions;

namespace Application.Common.Helpers;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() => _ => true;
    public static Expression<Func<T, bool>> False<T>() => _ => false;

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
        => left.Compose(right, Expression.AndAlso);

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
        => left.Compose(right, Expression.OrElse);

    private static Expression<Func<T, bool>> Compose<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        Func<Expression, Expression, Expression> merge)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var leftBody = new ReplaceExpressionVisitor(left.Parameters[0], parameter).Visit(left.Body);
        var rightBody = new ReplaceExpressionVisitor(right.Parameters[0], parameter).Visit(right.Body);

        return Expression.Lambda<Func<T, bool>>(merge(leftBody!, rightBody!), parameter);
    }

    private sealed class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
    {
        public override Expression? Visit(Expression? node)
            => node == oldValue ? newValue : base.Visit(node);
    }
}

