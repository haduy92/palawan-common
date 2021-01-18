using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Palawan.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Palawan.Common.Domain.Repositories
{
	public interface IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
	{
		#region GET

		/// <summary>
		/// Used to get a IQueryable that is used to retrieve entities from entire table.
		/// </summary>
		/// <returns>IQueryable to be used to select entities from database</returns>
		IQueryable<TEntity> GetQuery();

		/// <summary>
		/// Gets an entity with given primary key.
		/// </summary>
		/// <param name="keyValue">Primary key of the entity to get.</param>
		/// <returns>Entity.</returns>
		TEntity Get(TPrimaryKey keyValue);

		/// <summary>
		/// Gets an entity with given primary key.
		/// </summary>
		/// <param name="keyValue">Primary key of the entity to get.</param>
		/// <returns>Entity.</returns>
		Task<TEntity> GetAsync(TPrimaryKey keyValue);

		/// <summary>
		/// Get entities with given predicate.
		/// </summary>
		/// <param name="keyValue">Primary key of the entity to get.</param>
		/// <param name="includes">A list of include expressions.</param>
		/// <param name="disableTracking">Should EF track the change of entity or not.</param>
		/// <returns>Entity.</returns>
		TEntity Get(TPrimaryKey keyValue
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true);

		/// <summary>
		/// Get entities with given predicate.
		/// </summary>
		/// <param name="keyValue">Primary key of the entity to get.</param>
		/// <param name="includes">A list of include expressions.</param>
		/// <param name="disableTracking">Should EF track the change of entity or not.</param>
		/// <returns>Entity.</returns>
		Task<TEntity> GetAsync(TPrimaryKey keyValue
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true);

		/// <summary>
		/// Gets an entity with given given predicate or null if not found.
		/// </summary>
		/// <param name="predicate">Predicate to filter entities</param>
		TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true);

		/// <summary>
		/// Gets an entity with given given predicate or null if not found.
		/// </summary>
		/// <param name="predicate">Predicate to filter entities</param>
		Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true);

		/// <summary>
		/// Get entities with given predicate.
		/// </summary>
		/// <param name="predicate">Predicate to filter entities</param>
		/// <param name="orderBys">A list of order by expressions.</param>
		/// <param name="includes">A list of include expressions.</param>
		/// <param name="pageIndex">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="disableTracking">Should EF track the change of entity or not.</param>
		/// <returns>Collection of entities.</returns>
		Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null
			, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBys = null
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, int? pageIndex = null
			, int? pageSize = null
			, bool disableTracking = true);

		#endregion

		#region INSERT

		/// <summary>
		/// Inserts a new entity.
		/// </summary>
		/// <param name="entity">Inserted entity</param>
		TEntity Insert(TEntity entity);

		/// <summary>
		/// Inserts a new entity.
		/// </summary>
		/// <param name="entity">Inserted entity</param>
		Task<TEntity> InsertAsync(TEntity entity);

		/// <summary>
		/// Inserts multiple new entities.
		/// </summary>
		/// <param name="entities">Inserted entity</param>
		void InsertMany(IEnumerable<TEntity> entities);

		/// <summary>
		/// Inserts multiple new entities.
		/// </summary>
		/// <param name="entities">Inserted entity</param>
		Task InsertManyAsync(IEnumerable<TEntity> entities);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// </summary>
		/// <param name="entity">Entity</param>
		TEntity Upsert(TEntity entity);

		/// <summary>
		/// Inserts or updates given entity depending on Id's value.
		/// </summary>
		/// <param name="entity">Entity</param>
		Task<TEntity> UpsertAsync(TEntity entity);

		#endregion

		#region UPDATE

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">Entity</param>
		TEntity Update(TEntity entity);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">Entity</param>
		Task<TEntity> UpdateAsync(TEntity entity);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		/// <param name="updateAction">Action that can be used to change values of the entity</param>
		/// <returns>Updated entity</returns>
		TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="id">Id of the entity</param>
		/// <param name="updateAction">Action that can be used to change values of the entity</param>
		/// <returns>Updated entity</returns>
		Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

		#endregion

		#region DELETE

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">Entity to be deleted</param>
		void Delete(TEntity entity);

		/// <summary>
		/// Deletes an entity.
		/// </summary>
		/// <param name="entity">Entity to be deleted</param>
		Task DeleteAsync(TEntity entity);

		/// <summary>
		/// Deletes an entity by primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity</param>
		void Delete(TPrimaryKey id);

		/// <summary>
		/// Deletes an entity by primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity</param>
		Task DeleteAsync(TPrimaryKey id);

		/// <summary>
		/// Deletes many entities by function.
		/// Notice that: All entities fits to given predicate are retrieved and deleted.
		/// This may cause major performance problems if there are too many entities with
		/// given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities</param>
		void Delete(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Deletes many entities by function.
		/// Notice that: All entities fits to given predicate are retrieved and deleted.
		/// This may cause major performance problems if there are too many entities with
		/// given predicate.
		/// </summary>
		/// <param name="predicate">A condition to filter entities</param>
		Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

		#endregion
	}
}