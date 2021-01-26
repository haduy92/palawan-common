using Microsoft.AspNetCore.Http;

namespace Palawan.Common.Session
{
	public class ContextSession : IContextSession
	{
		public string UserId { get; private set; }

		public ContextSession(IHttpContextAccessor httpContextAccessor)
		{
			const string UserIdKey = "UserId";

			if (httpContextAccessor != null
				&& httpContextAccessor.HttpContext != null
				&& httpContextAccessor.HttpContext.Request.Headers.ContainsKey(UserIdKey))
			{
				UserId = httpContextAccessor.HttpContext.Request.Headers[UserIdKey].ToString();
			}
		}
	}
}