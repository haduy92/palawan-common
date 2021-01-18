using System;
using Palawan.Common.Extensions;

namespace Palawan.Common.Domain.Entities.Auditing
{
	public static class EntityAuditingHelper
	{
		public static void SetCreationAuditProperties(object entityAsObj, string userId)
		{
			var entityWithCreationTime = entityAsObj as IHasCreationTime;
			if (entityWithCreationTime == null)
			{
				//Object does not implement IHasCreationTime
				return;
			}

			if (entityWithCreationTime.CreationTime == default(DateTime))
			{
				entityWithCreationTime.CreationTime = DateTime.UtcNow;
			}

			if (!(entityAsObj is ICreationAudited))
			{
				//Object does not implement ICreationAudited
				return;
			}

			if (userId.IsNullOrWhiteSpace())
			{
				//Unknown user
				return;
			}

			var entity = entityAsObj as ICreationAudited;
			if (entity.CreatorUserId != null)
			{
				//CreatorUserId is already set
				return;
			}

			entity.CreatorUserId = userId;
		}

		public static void SetModificationAuditProperties(object entityAsObj, string userId)
		{
			if (entityAsObj is IHasModificationTime)
			{
				entityAsObj.As<IHasModificationTime>().LastModificationTime = DateTime.UtcNow;
			}

			if (!(entityAsObj is IModificationAudited))
			{
				//Entity does not implement IModificationAudited
				return;
			}

			var entity = entityAsObj.As<IModificationAudited>();

			if (userId.IsNullOrWhiteSpace())
			{
				//Unknown user
				entity.LastModifierUserId = null;
				return;
			}

			entity.LastModifierUserId = userId;
		}

		public static void SetDeletionAuditProperties(object entityAsObj, string userId)
		{
			if (entityAsObj is IHasDeletionTime)
			{
				var entity = entityAsObj.As<IHasDeletionTime>();

				if (entity.DeletionTime == null)
				{
					entity.DeletionTime = DateTime.UtcNow;
				}
			}

			if (entityAsObj is IDeletionAudited)
			{
				var entity = entityAsObj.As<IDeletionAudited>();

				if (entity.DeleterUserId != null)
				{
					return;
				}

				if (userId == null)
				{
					entity.DeleterUserId = null;
					return;
				}

				entity.DeleterUserId = userId;
			}
		}
	}
}