namespace Palawan.Common.Session
{
	public class NullContextSession : IContextSession
	{
		public string UserId { get; } = null;

		/// <summary>
		/// Singleton instance.
		/// </summary>
		public static NullContextSession Instance { get; } = new NullContextSession();
	}
}