namespace Palawan.Common.Domain.Entities.Auditing
{
	/// <summary>
	/// This interface is implemented by entities that is wanted to store modification information (who and when modified lastly).
	/// Modification time and modifier user are automatically set when saving <see cref="Entity{TPrimaryKey}"/> to database.
	/// </summary>
	public interface IModificationAudited : IHasModificationTime
	{
		/// <summary>
		/// Last modifier user for this entity.
		/// </summary>
		string LastModifierUserId { get; set; }
	}
}