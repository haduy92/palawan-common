namespace Palawan.Common.Domain.Entities.Auditing
{
	/// <summary>
	/// This interface is implemented by entities that is wanted to store creation information (who and when created).
	/// Creation time and creator user are automatically set when saving <see cref="Entity{TPrimaryKey}"/> to database.
	/// </summary>
	public interface ICreationAudited : IHasCreationTime
	{
		/// <summary>
		/// Id of the creator user of this entity.
		/// </summary>
		string CreatorUserId { get; set; }
	}
}