using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HM.OMS.PageOneMessageConsole
{
    public class LamdaToString
    {
        public void Execute()
        {
            string[] names = new string[] { "Huan", "Long", "Loc" };
            Expression<Func<Product, bool>> expr = p => p.Description == "test" && names.Contains(p.Name);
            string name = GetPropertyName(expr);

            Console.Read();
        }

        public string GetPropertyName(Expression<Func<Product, bool>> expr)
        {
            var mexpr = (BinaryExpression)expr.Body;

            var member = (MemberExpression)expr.Body;
            var property = (PropertyInfo)member.Member;
            return property.Name;
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
