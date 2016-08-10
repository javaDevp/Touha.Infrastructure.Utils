using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Touha.Infrastructure.Utils.Lambda
{
    public class LambdaUtil
    {
        #region GetValue(获取值)

        /// <summary>
        /// 获取值,范例：t => t.Name == "A",返回 A
        /// </summary>
        /// <param name="expression">表达式,范例：t => t.Name == "A"</param>
        public static object GetValue(LambdaExpression expression)
        {
            if (expression == null)
            {
                return null;
            }
            BinaryExpression binaryExpression = GetBinaryExpression(expression);
            if (binaryExpression != null)
            {
                return GetBinaryValue(binaryExpression);
            }
            var callExpression = expression.Body as MethodCallExpression;
            if (callExpression != null)
            {
                return GetMethodValue(callExpression);
            }
            return null;
        }

        /// <summary>
        /// 获取方法调用表达式的值
        /// </summary>
        private static object GetMethodValue(MethodCallExpression callExpression)
        {
            var argumentExpression = callExpression.Arguments.FirstOrDefault();
            return GetConstantValue(argumentExpression);
        }

        /// <summary>
        /// 获取二元表达式的值
        /// </summary>
        private static object GetBinaryValue(BinaryExpression binaryExpression)
        {
            var unaryExpression = binaryExpression.Right as UnaryExpression;
            if (unaryExpression != null)
            {
                return GetConstantValue(unaryExpression.Operand);
            }
            return GetConstantValue(binaryExpression.Right);
        }

        /// <summary>
        /// 获取常量值
        /// </summary>
        private static object GetConstantValue(Expression expression)
        {
            var constantExpression = expression as ConstantExpression;
            if (constantExpression == null)
            {
                return null;
            }
            return constantExpression.Value;
        }

        /// <summary>
        /// 获取二元表达式
        /// </summary>
        private static BinaryExpression GetBinaryExpression(LambdaExpression expression)
        {
            var binaryExpression = expression.Body as BinaryExpression;
            if (binaryExpression != null)
            {
                return binaryExpression;
            }

            var unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression == null)
            {
                return null;
            }

            return unaryExpression.Operand as BinaryExpression;
        }

        #endregion

        #region GetCriteriaCount(获取谓词条件的个数)

        /// <summary>
        /// 获取谓词条件的个数
        /// </summary>
        /// <param name="expression">谓词表达式,范例：t => t.Name == "A"</param>
        public static int GetCriteriaCount(LambdaExpression expression)
        {
            if (expression == null)
            {
                return 0;
            }
            var result = expression.ToString().Replace("AndAlso", "|").Replace("OrElse", "|");
            return result.Split('|').Count();
        }

        #endregion
    }
}
