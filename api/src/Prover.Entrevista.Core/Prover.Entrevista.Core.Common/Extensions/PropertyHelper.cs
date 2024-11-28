using System.Linq.Expressions;
using System.Reflection;

namespace Prover.Entrevista.Core.Common.Extensions;

public static class PropertyHelper<T>
{
    public static PropertyInfo GetProperty<TValue>(Expression<Func<T, TValue>> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        Expression body = selector;

        if (body is LambdaExpression expression)
            body = expression.Body;

        switch (body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var property = (PropertyInfo)((MemberExpression)body).Member;
                return typeof(T).GetProperty(property.Name) ?? throw new ArgumentNullException(paramName: property.Name);
            default:
                throw new InvalidOperationException();
        }
    }
}