using System;

namespace Palawan.Common.Session
{
	/// <summary>
	/// Defines some session information that can be useful for applications.
	/// </summary>
	public interface IContextSession
	{
		/// <summary>
		/// Gets current UserId or null.
		/// It can be null if no user logged in.
		/// </summary>
		string UserId { get; }
	}
}