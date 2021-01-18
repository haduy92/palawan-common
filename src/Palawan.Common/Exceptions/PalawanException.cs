using System;

namespace Palawan.Common.Exceptions
{
	/// <summary>
	/// Base exception type for those are thrown by the system for specific exceptions.
	/// </summary>
	[Serializable]
	public class PalawanException : Exception
	{
		/// <summary>
		/// Creates a new <see cref="PalawanException"/> object.
		/// </summary>
		public PalawanException()
		{ }

		/// <summary>
		/// Creates a new <see cref="PalawanException"/> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		public PalawanException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Creates a new <see cref="PalawanException"/> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public PalawanException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}