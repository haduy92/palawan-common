namespace Palawan.Common.Session
{
	/// <summary>
	/// Null instance of <see ref="IContextSession" />
	/// </summary>
	public class NullContextSession : IContextSession
	{
		/// <summary>
		/// Id of current logged in user
		/// </summary>
		/// <value></value>
		public string UserId { get; } = null;

		/// <summary>
		/// Singleton instance.
		/// </summary>
		public static NullContextSession Instance { get; } = new NullContextSession();
	}
}