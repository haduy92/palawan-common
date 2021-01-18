using System;
using System.Globalization;
using System.Linq;
using System.ComponentModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Reflection;

namespace Palawan.Common.Extensions
{
	/// <summary>
	/// Extension methods for all objects.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Used to simplify and beautify casting an object to a type.
		/// </summary>
		/// <typeparam name="T">Type to be casted</typeparam>
		/// <param name="obj">Object to cast</param>
		/// <returns>Casted object</returns>
		public static T As<T>(this object obj)
			where T : class
		{
			return (T)obj;
		}

		/// <summary>
		/// Converts given object to a value or enum type using <see cref="Convert.ChangeType(object,TypeCode)"/> or <see cref="Enum.Parse(Type,string)"/> method.
		/// </summary>
		/// <param name="obj">Object to be converted</param>
		/// <typeparam name="T">Type of the target object</typeparam>
		/// <returns>Converted object</returns>
		public static T To<T>(this object obj)
			where T : struct
		{
			if (typeof(T) == typeof(Guid) || typeof(T) == typeof(TimeSpan))
			{
				return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
			}
			if (typeof(T).IsEnum)
			{
				if (Enum.IsDefined(typeof(T), obj))
				{
					return (T)Enum.Parse(typeof(T), obj.ToString());
				}
				else
				{
					throw new ArgumentException($"Enum type undefined '{obj}'.");
				}
			}

			return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Check if an item is in a list.
		/// </summary>
		/// <param name="item">Item to check</param>
		/// <param name="list">List of items</param>
		/// <typeparam name="T">Type of the items</typeparam>
		public static bool IsIn<T>(this T item, params T[] list)
		{
			return list.Contains(item);
		}

		/// <summary>
		/// Convert an object to XElement object
		/// </summary>
		/// <param name="obj">The object to convert</param>
		/// <typeparam name="T">The type being converted.</typeparam>
		/// <returns>XElement</returns>
		public static XElement ToXElement<T>(this T obj)
		{
			var xmlAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(XmlRootAttribute)) as XmlRootAttribute;
			var ns = new XmlSerializerNamespaces();
			ns.Add(xmlAttribute.ElementName, xmlAttribute.Namespace);

			using (var memoryStream = new MemoryStream())
			{
				using (var streamWriter = new StreamWriter(memoryStream))
				{
					var xmlSerializer = new XmlSerializer(typeof(T));
					xmlSerializer.Serialize(streamWriter, obj, ns);

					return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
				}
			}
		}

		public static bool IsAssignableFrom<T>(this T obj, Type type)
		{
			return type.IsAssignableFrom(typeof(T).Ge‌​tTypeInfo());
		}
	}
}