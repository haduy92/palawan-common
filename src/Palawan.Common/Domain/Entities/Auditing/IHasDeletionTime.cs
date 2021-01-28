namespace Palawan.Common.Domain.Entities.Auditing
{
	/// <summary>
	/// An entity can implement this interface if <see cref="DeletionTime"/> of this entity must be stored.
	/// <see cref="DeletionTime"/> is automatically set when deleting <see cref="Entity{TPrimaryKey}"/>.
	/// </summary>
	public interface IHasDeletionTime : ISoftDelete
	{
		/// <summary>
		/// Deletion time of this entity.
		/// </summary>
		System.DateTime? DeletionTime { get; set; }
	}
}