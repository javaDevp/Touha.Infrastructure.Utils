using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touha.Infrastructure.Utils.Lambda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Touha.Infrastructure.Utils.Lambda.Tests
{
    [TestClass()]
    public class LambdaUtilTests
    {
        /// <summary>
        /// 测试
        /// </summary>
        public class Test
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int? NullableInt { get; set; }
            public decimal? NullableDecimal { get; set; }
            public TestA A { get; set; }
            public class TestA
            {
                public int Integer { get; set; }
                public string Address { get; set; }
                public TestB B { get; set; }
                public class TestB
                {
                    public string Name { get; set; }
                }
            }
        }

        [TestMethod()]
        public void GetValueTest()
        {
            //UnaryExpression ---> BinaryExpression
            Expression<Func<Test, object>> expression = test => test.Name == "A";
            Assert.AreEqual(LambdaUtil.GetValue(expression), "A");

            //MethodCallExpression
            Expression<Action<Test>> expression2 = test => Console.WriteLine(test);
            Assert.AreEqual(LambdaUtil.GetValue(expression2), null);

            Expression<Func<int, int>> expression3 = i => -i;
            Assert.AreEqual(LambdaUtil.GetValue(expression3), null);

            //二级返回值
            Expression<Func<Test, bool>> expression4 = test => test.A.Integer == 1;
            Assert.AreEqual(1, LambdaUtil.GetValue(expression4));

            //三级返回值
            Expression<Func<Test, bool>> expression5 = test => test.A.B.Name == "B";
            Assert.AreEqual("B", LambdaUtil.GetValue(expression5));
        }

        [TestMethod()]
        public void GetCriteriaCountTest()
        {
            //0个条件
            Assert.AreEqual(0, LambdaUtil.GetCriteriaCount(null));

            //1个条件
            Expression<Func<Test, bool>> expression = test => test.Name == "A";
            Assert.AreEqual(1, LambdaUtil.GetCriteriaCount(expression));

            //2个条件，与连接符
            expression = test => test.Name == "A" && test.Name == "B";
            Assert.AreEqual(2, LambdaUtil.GetCriteriaCount(expression));

            //2个条件，或连接符
            expression = test => test.Name == "A" || test.Name == "B";
            Assert.AreEqual(2, LambdaUtil.GetCriteriaCount(expression));

            //3个条件
            expression = test => test.Name == "A" && test.Name == "B" || test.Name == "C";
            Assert.AreEqual(3, LambdaUtil.GetCriteriaCount(expression));

            //3个条件,包括导航属性
            expression = test => test.A.Address == "A" && test.Name == "B" || test.Name == "C";
            Assert.AreEqual(3, LambdaUtil.GetCriteriaCount(expression));
        }
    }
}