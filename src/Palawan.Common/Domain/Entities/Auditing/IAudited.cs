namespace Palawan.Common.Domain.Entities.Auditing
{
	/// <summary>
	/// This interface is implemented by entities that is wanted to store creation and modification information.
	/// Related properties automatically set when saving/updating <see cref="Entity"/> objects.
	/// </summary>
	public interface IAudited : ICreationAudited, IModificationAudited
	{ }
}