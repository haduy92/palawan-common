using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Palawan.Common.Extensions
{
	/// <summary>
	/// Extension methods for String class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Indicates whether this string is insensitive equal with another string.
		/// </summary>
		public static bool EqualsIgnoreCase(this string s, string o)
		{
			return string.Equals(s, o, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Indicates whether this string is null or an System.String.Empty string.
		/// </summary>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// indicates whether this string is null, empty, or consists only of white-space characters.
		/// </summary>
		public static bool IsNullOrWhiteSpace(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		/// <summary>
		/// Uses string.Split method to split given string by given separator.
		/// </summary>
		public static string[] Split(this string str, string separator)
		{
			return str.Split(new[] { separator }, StringSplitOptions.None);
		}

		/// <summary>
		/// Uses string.Split method to split given string by given separator.
		/// </summary>
		public static string[] Split(this string str, string separator, StringSplitOptions options)
		{
			return str.Split(new[] { separator }, options);
		}

		/// <summary>
		/// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
		/// </summary>
		public static string[] SplitToLines(this string str)
		{
			return str.Split(Environment.NewLine);
		}

		/// <summary>
		/// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
		/// </summary>
		public static string[] SplitToLines(this string str, StringSplitOptions options)
		{
			return str.Split(Environment.NewLine, options);
		}

		/// <summary>
		/// Converts string to enum value.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">String value to convert</param>
		/// <returns>Returns enum object</returns>
		public static T ToEnum<T>(this string value)
			where T : struct
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			return (T)Enum.Parse(typeof(T), value);
		}

		/// <summary>
		/// Converts string to enum value.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">String value to convert</param>
		/// <param name="ignoreCase">Ignore case</param>
		/// <returns>Returns enum object</returns>
		public static T ToEnum<T>(this string value, bool ignoreCase)
			where T : struct
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			return (T)Enum.Parse(typeof(T), value, ignoreCase);
		}

		/// <summary>
		/// Deserialize an XML string to object.
		/// </summary>
		/// <param name="xml">XML string.</param>
		/// <typeparam name="T">The type being deserialized.</typeparam>
		/// <returns>The object deserialized as the specified type.</returns>
		public static T FromXML<T>(this string xml)
		{
			using (TextReader reader = new StringReader(xml))
			{
				var serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(reader);
			}
		}

		/// <summary>
		/// Remove all whitespaces from a string.
		/// </summary>
		/// <param name="s">string needs to trim.</param>
		/// <returns>Trimmed string.</returns>
		public static string RemoveWhiteSpaces(this string s)
		{
			return Regex.Replace(s, @"\s+", "");
		}
	}
}