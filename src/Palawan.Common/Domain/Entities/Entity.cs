using System;
using System.Collections.Generic;
using System.Reflection;

namespace Palawan.Common.Domain.Entities
{
	/// <summary>
	/// Basic implementation of IEntity interface.
	/// An entity can inherit this class of directly implement to IEntity interface.
	/// </summary>
	/// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
	[Serializable]
	public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
	{
		/// <summary>
		/// Unique identifier for this entity.
		/// </summary>
		public virtual TPrimaryKey Id { get; set; }

		/// <summary>
		/// Checks if this entity is transient (it doesn't has an Id as primary key).
		/// </summary>
		/// <returns>True, if this entity is transient</returns>
		public virtual bool IsTransient()
		{
			if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey)))
			{
				return true;
			}

			// Workaround for EF Core since it sets int/long to min value when attaching to dbContext
			if (typeof(TPrimaryKey) == typeof(int))
			{
				return Convert.ToInt32(Id) <= 0;
			}

			if (typeof(TPrimaryKey) == typeof(long))
			{
				return Convert.ToInt64(Id) <= 0;
			}

			return false;
		}

		/// <inheritdoc/>
		public virtual bool EntityEquals(object obj)
		{
			if (obj == null || !(obj is Entity<TPrimaryKey>))
			{
				return false;
			}

			// Same instances must be considered as equal
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			// Transient objects are not considered as equal
			var other = (Entity<TPrimaryKey>)obj;
			if (IsTransient() && other.IsTransient())
			{
				return false;
			}

			// Must have a IS-A relation of types or must be same type
			var typeOfThis = GetType();
			var typeOfOther = other.GetType();
			if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
			{
				return false;
			}

			return Id.Equals(other.Id);
		}

		public override bool Equals(object obj)
		{
			return EntityEquals(obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return $"[{GetType().Name} {Id}]";
		}
	}
}