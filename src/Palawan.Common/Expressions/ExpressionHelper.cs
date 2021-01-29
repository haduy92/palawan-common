using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Palawan.Common.Expressions
{
	/// <summary>
	/// This class contains all the necessary expressions and concatenate them with an <strong>And</strong> / <strong>Or</strong>
	/// <para>
	/// <see href="https://www.codementor.io/@juliandambrosio/how-to-use-expression-trees-to-build-dynamic-queries-c-xyk1l2l82">Source</see>
	/// </para>
	/// </summary>
	public static class ExpressionHelper
	{
		/// <summary>
		/// Create expression
		/// </summary>
		/// <example>
		/// This sample shows how to call the <see cref="CreateExpression{T}(Expression{Func{T, object}}, ExpressionOperator, object)"/> method.
		/// <code>
		/// var exp = ExpressionHelper.CreateExpression{Book}(a => a.Id, OperationExpression.Equals, bookId);
		/// </code>
		/// </example>
		/// <param name="expr">Expression to select property of type "T"</param>
		/// <param name="selectedOperator">Expression operator</param>
		/// <param name="fieldValue">Field value to compare</param>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <returns>Expression{Func{T, bool}}</returns>
		public static Expression<Func<T, bool>> CreateExpression<T>(Expression<Func<T, object>> expr, ExpressionOperator selectedOperator, object fieldValue)
		{
			string name = GetOperand<T>(expr);
			return CreateExpression<T>(name, selectedOperator, fieldValue);
		}

		/// <summary>
		/// Create expression
		/// </summary>
		/// <example>
		/// This sample shows how to call the <see cref="CreateExpression{T, T2}(Expression{Func{T, object}}, ExpressionOperator, object)"/> method.
		/// <code>
		/// public class Author
		/// {
		///		public long Id { get; set; }
		///		public string Name { get; set; }
		///		public ICollection{Book} Books { get; set; }
		///	}
		/// public class Book
		/// {
		///		public long Id { get; set; }
		///		public string Name { get; set; }
		///		public Book Book { get; set; }
		///	}
		/// var exp = ExpressionHelper.CreateExpression{Author, Book}(a => a.Books, OperationExpression.Any, bookId);
		/// </code>
		/// </example>
		/// <param name="expr">Expression to select property of type "T"</param>
		/// <param name="selectedOperator">Expression operator</param>
		/// <param name="fieldValue">Field value to compare</param>
		/// <typeparam name="T1">Type of entity</typeparam>
		/// <typeparam name="T2">Type of entity</typeparam>
		/// <returns>Expression{Func{T, bool}}</returns>
		public static Expression<Func<T1, bool>> CreateExpression<T1, T2>(Expression<Func<T1, object>> expr, ExpressionOperator selectedOperator, object fieldValue)
		{
			string name = GetOperand<T1>(expr);
			return CreateExpression<T1, T2>(name, selectedOperator, fieldValue);
		}

		/// <summary>
		/// Create expression
		/// </summary>
		/// <example>
		/// This sample shows how to call the <see cref="CreateExpression{T}(string, ExpressionOperator, object)"/> method.
		/// <code>
		/// var exp = ExpressionHelper.CreateExpression{Book}("Id", OperationExpression.Equals, bookId);
		/// </code>
		/// </example>
		/// <param name="fieldName">Property name</param>
		/// <param name="selectedOperator">Expression operator</param>
		/// <param name="fieldValue">Field value to compare</param>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <returns>Expression{Func{T, bool}}</returns>
		public static Expression<Func<T, bool>> CreateExpression<T>(string fieldName, ExpressionOperator selectedOperator, object fieldValue)
		{
			var props = TypeDescriptor.GetProperties(typeof(T));
			var prop = GetProperty(props, fieldName, true);

			var parameter = Expression.Parameter(typeof(T));
			var expressionParameter = GetMemberExpression<T>(parameter, fieldName);

			if (prop != null && fieldValue != null)
			{

				BinaryExpression body = null;

				switch (selectedOperator)
				{
					case ExpressionOperator.Equals:
						body = Expression.Equal(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
						return Expression.Lambda<Func<T, bool>>(body, parameter);
					case ExpressionOperator.NotEquals:
						body = Expression.NotEqual(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
						return Expression.Lambda<Func<T, bool>>(body, parameter);
					case ExpressionOperator.Minor:
						body = Expression.LessThan(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
						return Expression.Lambda<Func<T, bool>>(body, parameter);
					case ExpressionOperator.MinorEquals:
						body = Expression.LessThanOrEqual(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
						return Expression.Lambda<Func<T, bool>>(body, parameter);
					case ExpressionOperator.Mayor:
						body = Expression.GreaterThan(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
						return Expression.Lambda<Func<T, bool>>(body, parameter);
					case ExpressionOperator.MayorEquals:
						body = Expression.GreaterThanOrEqual(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
						return Expression.Lambda<Func<T, bool>>(body, parameter);
					case ExpressionOperator.Like:
						var contains = typeof(string).GetMethod("Contains");
						var bodyLike = Expression.Call(expressionParameter, contains, Expression.Constant(fieldValue, prop.PropertyType));
						return Expression.Lambda<Func<T, bool>>(bodyLike, parameter);
					case ExpressionOperator.Contains:
						return Contains<T>(fieldValue, parameter, expressionParameter);
					default:
						throw new Exception("Not implement Operation");
				}
			}
			else
			{
				Expression<Func<T, bool>> filter = x => true;
				return filter;
			}
		}

		/// <summary>
		/// Create expression
		/// </summary>
		/// <example>
		/// This sample shows how to call the <see cref="CreateExpression{T, T2}(string, ExpressionOperator, object)"/> method.
		/// <code>
		/// public class Author
		/// {
		///		public long Id { get; set; }
		///		public string Name { get; set; }
		///		public ICollection{Book} Books { get; set; }
		///	}
		/// public class Book
		/// {
		///		public long Id { get; set; }
		///		public string Name { get; set; }
		///		public Book Book { get; set; }
		///	}
		/// var exp = ExpressionHelper.CreateExpression{Author, Book}("Books", OperationExpression.Any, bookId);
		/// </code>
		/// </example>
		/// <param name="fieldName">Property name</param>
		/// <param name="selectedOperator">Expression operator</param>
		/// <param name="fieldValue">Field value to compare</param>
		/// <typeparam name="T1">Type of entity</typeparam>
		/// <typeparam name="T2">Type of entity</typeparam>
		/// <returns>Expression{Func{T, bool}}</returns>
		public static Expression<Func<T1, bool>> CreateExpression<T1, T2>(string fieldName, ExpressionOperator selectedOperator, object fieldValue)
		{
			var props = TypeDescriptor.GetProperties(typeof(T1));
			var prop = GetProperty(props, fieldName, true);

			var parameter = Expression.Parameter(typeof(T1));
			var expressionParameter = GetMemberExpression<T1>(parameter, fieldName);

			if (prop != null && fieldValue != null)
			{
				switch (selectedOperator)
				{
					case ExpressionOperator.Any:
						return Any<T1, T2>(fieldValue, parameter, expressionParameter);

					default:
						throw new Exception("Not implement Operation");
				}
			}
			else
			{
				Expression<Func<T1, bool>> filter = x => true;
				return filter;
			}
		}

		/// <summary>
		/// A shortcut of <see cref="Expression.OrElse(Expression, Expression)" /> method
		/// </summary>
		/// <example>
		/// This sample shows how to call the <see cref="Or{T}(Expression{Func{T, bool}}, Expression{Func{T, bool}})"/> method.
		/// <code>
		/// var expr = ExpressionHelper.CreateExpression{Author}(x => x.Name, OperationExpression.Like, authorName);
		/// expr = expr.Or(ExpressionHelper.CreateExpression{Author}(x => x.NickName, OperationExpression.Like, authorNickName))
		/// </code>
		/// </example>
		/// <param name="expr">Left expression to evaluate</param>
		/// <param name="or">Right expression to evaluate</param>
		/// <typeparam name="T">The type of the parameter of the expression that this operation evaluates</typeparam>
		/// <returns>Expression{Func{T, bool}}</returns>
		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> or)
		{
			if (expr == null) return or;
			return Expression.Lambda<Func<T, bool>>(Expression.OrElse(new SwapVisitor(expr.Parameters[0], or.Parameters[0]).Visit(expr.Body), or.Body), or.Parameters);
		}

		/// <summary>
		/// A shortcut of <see cref="Expression.AndAlso(Expression, Expression)" /> method
		/// </summary>
		/// <example>
		/// This sample shows how to call the <see cref="And{T}(Expression{Func{T, bool}}, Expression{Func{T, bool}})"/> method.
		/// <code>
		/// var expr = ExpressionHelper.CreateExpression{Author}(x => x.Name, OperationExpression.Like, authorName);
		/// expr = expr.Or(ExpressionHelper.CreateExpression{Author}(x => x.NickName, OperationExpression.Like, authorNickName))
		/// </code>
		/// </example>
		/// <param name="expr">Left expression to evaluate</param>
		/// <param name="and">Right expression to evaluate</param>
		/// <typeparam name="T">The type of the parameter of the expression that this operation evaluates</typeparam>
		/// <returns>Expression{Func{T, bool}}</returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> and)
		{
			if (expr == null) return and;
			return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(new SwapVisitor(expr.Parameters[0], and.Parameters[0]).Visit(expr.Body), and.Body), and.Parameters);
		}

		#region -- Private methods --

		private static string GetOperand<T>(Expression<Func<T, object>> exp)
		{
			var body = exp.Body as MemberExpression;

			if (body == null)
			{
				var uBody = (UnaryExpression)exp.Body;
				body = uBody.Operand as MemberExpression;
			}

			var operand = body.ToString();

			return operand.Substring(2);

		}

		private static MemberExpression GetMemberExpression<T>(ParameterExpression parameter, string propName)
		{
			if (string.IsNullOrEmpty(propName)) return null;
			var propertiesName = propName.Split('.');
			if (propertiesName.Count() == 2)
				return Expression.Property(Expression.Property(parameter, propertiesName[0]), propertiesName[1]);
			return Expression.Property(parameter, propName);
		}

		private static Expression<Func<T, bool>> Contains<T>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
		{
			var list = (IList<long>)fieldValue;

			if (list == null || list.Count == 0) return x => true;

			var containsInList = typeof(List<long>).GetMethod("Contains", new Type[] { typeof(long) });
			var bodyContains = Expression.Call(Expression.Constant(fieldValue), containsInList, memberExpression);

			return Expression.Lambda<Func<T, bool>>(bodyContains, parameterExpression);
		}

		private static Expression<Func<T, bool>> Any<T, T2>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
		{
			var lambda = (Expression<Func<T2, bool>>)fieldValue;
			var anyMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(m => m.Name == "Any" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(T2));

			var body = Expression.Call(anyMethod, memberExpression, lambda);

			return Expression.Lambda<Func<T, bool>>(body, parameterExpression);
		}

		private static PropertyDescriptor GetProperty(PropertyDescriptorCollection props, string fieldName, bool ignoreCase)
		{
			if (!fieldName.Contains('.'))
				return props.Find(fieldName, ignoreCase);

			var fieldNameProperty = fieldName.Split('.');
			return props.Find(fieldNameProperty[0], ignoreCase).GetChildProperties().Find(fieldNameProperty[1], ignoreCase);

		}

		#endregion
	}
}