namespace Palawan.Common.Domain.Entities.Auditing
{
	/// <summary>
	/// An entity can implement this interface if <see cref="CreationTime"/> of this entity must be stored.
	/// <see cref="CreationTime"/> is automatically set when saving <see cref="Entity{TPrimaryKey}"/> to database.
	/// </summary>
	public interface IHasCreationTime
	{
		/// <summary>
		/// Creation time of this entity.
		/// </summary>
		System.DateTime CreationTime { get; set; }
	}
}