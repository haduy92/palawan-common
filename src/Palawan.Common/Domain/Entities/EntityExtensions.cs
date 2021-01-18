using Palawan.Common.Domain.Entities;
using Palawan.Common.Domain.Entities.Auditing;

namespace Palawan.Common.Extensions
{
	/// <summary>
	/// Some useful extension methods for Entities.
	/// </summary>
	public static class EntityExtensions
	{
		/// <summary>
		/// Check if this Entity is null of marked as deleted.
		/// </summary>
		/// <returns>True if null or being marked as deleted</returns>
		public static bool IsNullOrDeleted(this ISoftDelete entity)
		{
			return entity == null || entity.IsDeleted;
		}

		/// <summary>
		/// Un-deletes this entity by setting <see cref="ISoftDelete.IsDeleted"/> to false and
		/// <see cref="IDeletionAudited"/> properties to null.
		/// </summary>
		public static void UnDelete(this ISoftDelete entity)
		{
			entity.IsDeleted = false;

			if (entity is IDeletionAudited)
			{
				var deletionAuditedEntity = entity.As<IDeletionAudited>();
				deletionAuditedEntity.DeletionTime = null;
				deletionAuditedEntity.DeleterUserId = null;
			}
		}
	}
}