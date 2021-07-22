using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Exceptions
{
    public class ValidationExceptionBuilder
    {
        protected readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public ValidationExceptionBuilder AddError(string property, string error)
        {
            if (!errors.ContainsKey(property))
            {
                errors.Add(property, new List<string>());
            }

            errors[property].Add(error);

            return this;
        }

        public void ThrowIfHasError()
        {
            if (errors.Count > 0)
            {
                throw new IFPSValidationAppException(errors);
            }
        }
    }

    public class ValidationExceptionBuilder<T> : ValidationExceptionBuilder
        where T : class
    {
        public ValidationExceptionBuilder<T> AddError(Expression<Func<T, object>> expression, string error)
        {
            AddError(GetMemberName(expression), error);

            return this;
        }

        private string GetMemberName(Expression<Func<T, object>> expression)
        {
            var memberName = "";
            try
            {
                MemberExpression memberExpr = expression.Body as MemberExpression;
                if (memberExpr == null)
                {
                    if (expression.Body is UnaryExpression unaryExpr && unaryExpr.NodeType == ExpressionType.Convert)
                    {
                        memberExpr = unaryExpr.Operand as MemberExpression;
                    }
                }

                memberName = memberExpr.Member.Name;
            }
            catch (Exception) { }

            return memberName;
        }
    }

}
