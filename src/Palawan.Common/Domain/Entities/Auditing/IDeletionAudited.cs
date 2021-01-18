namespace Palawan.Common.Domain.Entities.Auditing
{
	/// <summary>
	/// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
	/// Deletion time and deleter user are automatically set when saving <see cref="Entity"/> to database.
	/// </summary>
	public interface IDeletionAudited : IHasDeletionTime
	{
		/// <summary>
		/// Which user deleted this entity?
		/// </summary>
		string DeleterUserId { get; set; }
	}
}