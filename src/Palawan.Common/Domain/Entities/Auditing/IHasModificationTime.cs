namespace Palawan.Common.Domain.Entities.Auditing
{
	/// <summary>
	/// An entity can implement this interface if <see cref="LastModificationTime"/> of this entity must be stored.
	/// <see cref="LastModificationTime"/> is automatically set when updating <see cref="Entity{TPrimaryKey}"/>.
	/// </summary>
	public interface IHasModificationTime
	{
		/// <summary>
		/// The last modified time for this entity.
		/// </summary>
		System.DateTime? LastModificationTime { get; set; }
	}
}