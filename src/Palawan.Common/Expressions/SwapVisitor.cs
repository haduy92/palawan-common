using System.Linq.Expressions;

namespace Palawan.Common.Expressions
{
	/// <summary>
	/// This class allows to replace the parameters of the concatenated expressions with the parameter of one of them.
	/// <para>
	/// <see href="https://www.codementor.io/@juliandambrosio/how-to-use-expression-trees-to-build-dynamic-queries-c-xyk1l2l82">Source</see>
	/// </para>
	/// </summary>
	public class SwapVisitor : ExpressionVisitor
	{
		private readonly Expression _from;
		private readonly Expression _to;

		public SwapVisitor(Expression from, Expression to)
		{
			_from = from;
			_to = to;
		}

		public override Expression Visit(Expression node)
		{
			return node == _from ? _to : base.Visit(node);
		}
	}
}