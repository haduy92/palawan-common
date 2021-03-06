using System;
using Palawan.Common.Exceptions;

namespace Palawan.Common.Domain.Entities
{
	/// <summary>
	/// This exception is thrown if an entity excepted to be found but not found.
	/// </summary>
	[Serializable]
	public class EntityNotFoundException : PalawanException
	{
		/// <summary>
		/// Type of the entity.
		/// </summary>
		public Type EntityType { get; set; }

		/// <summary>
		/// Id of the Entity.
		/// </summary>
		public object Id { get; set; }

		/// <summary>
		/// Creates a new <see cref="EntityNotFoundException"/> object.
		/// </summary>
		public EntityNotFoundException()
		{ }

		/// <summary>
		/// Creates a new <see cref="EntityNotFoundException"/> object.
		/// </summary>
		public EntityNotFoundException(Type entityType, object id)
			: this(entityType, id, null)
		{ }

		/// <summary>
		/// Creates a new <see cref="EntityNotFoundException"/> object.
		/// </summary>
		public EntityNotFoundException(Type entityType, object id, Exception innerException)
			: base($"There is no such an entity. Entity type: {entityType.FullName}, id: {id}", innerException)
		{
			EntityType = entityType;
			Id = id;
		}

		/// <summary>
		/// Creates a new <see cref="EntityNotFoundException"/> object.
		/// </summary>
		public EntityNotFoundException(string entityTypeName, string keyName, object keyValue)
			: base($"There is no such an entity. Entity type: {entityTypeName}, key name: {keyName}, key value: {keyValue}")
		{ }

		/// <summary>
		/// Creates a new <see cref="EntityNotFoundException"/> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		public EntityNotFoundException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Creates a new <see cref="EntityNotFoundException"/> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public EntityNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}