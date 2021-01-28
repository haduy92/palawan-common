using System.IO;

namespace Palawan.Common.Extensions
{
	/// <summary>
	/// Some useful extension methods for Stream IO.
	/// </summary>
	public static class StreamExtension
	{
		/// <summary>
		/// Convert a Stream to file.
		/// </summary>
		/// <param name="source">The source of Stream.</param>
		/// <param name="filePath">Full file name.</param>
		public static void ToFile(this Stream source, string filePath)
		{
			var info = new DirectoryInfo(Path.GetDirectoryName(filePath));

			if (!info.Exists)
			{
				info.Create();
			}

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				source.CopyTo(fileStream);
				fileStream.Flush();
			}
		}

		/// <summary>
		/// Convert a Stream to file.
		/// </summary>
		/// <param name="source">The source of Stream.</param>
		/// <returns>Byte Array</returns>
		public static byte[] ToBytes(this Stream source)
		{
			using (var memoryStream = new MemoryStream())
			{
				source.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}