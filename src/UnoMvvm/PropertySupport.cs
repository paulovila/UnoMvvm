using System;
using System.Linq.Expressions;
using System.Reflection;

namespace UnoMvvm
{
    /// <summary>
    ///  Provides support for extracting property information based on a property expression.
    /// </summary>
    public static class PropertySupport
    {
        /// <summary>
        /// Extracts the property name from a property expression.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpression">The property expression (e.g. p =&gt; p.PropertyName)</param>
        /// <returns>The name of the property.</returns>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="propertyExpression" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">Thrown when the expression is:<br />
        ///     Not a <see cref="T:System.Linq.Expressions.MemberExpression" /><br />
        ///     The <see cref="T:System.Linq.Expressions.MemberExpression" /> does not represent a property.<br />
        ///     Or, the property is static.
        /// </exception>
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            return PropertySupport.ExtractPropertyNameFromLambda(propertyExpression);
        }

        /// <summary>Extracts the property name from a LambdaExpression.</summary>
        /// <param name="expression">The LambdaExpression</param>
        /// <returns>The name of the property.</returns>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="expression" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">Thrown when the expression is:<br />
        ///     The <see cref="T:System.Linq.Expressions.MemberExpression" /> does not represent a property.<br />
        ///     Or, the property is static.
        /// </exception>
        internal static string ExtractPropertyNameFromLambda(LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("PropertySupport_NotMemberAccessExpression_Exception", nameof(expression));
            }

            PropertyInfo member = body.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException("PropertySupport_ExpressionNotProperty_Exception", nameof(expression));
            }

            if (member.GetMethod.IsStatic)
            {
                throw new ArgumentException("PropertySupport_StaticExpression_Exception", nameof(expression));
            }

            return body.Member.Name;
        }
    }
}
