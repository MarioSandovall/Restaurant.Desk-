using System;
using System.Linq.Expressions;

namespace Service.Utils
{
    public static class ObjectHelper
    {
        public static string PropertyName<T>(this Expression<Func<T, object>> propertyExpression)
        {
            if (propertyExpression.Body is MemberExpression expression)
            {
                return expression.Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)propertyExpression.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }
    }
}
