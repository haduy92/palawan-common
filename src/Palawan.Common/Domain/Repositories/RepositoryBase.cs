using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Palawan.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Palawan.Common.Domain.Repositories
{
	public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey>
	{
		public abstract IQueryable<TEntity> GetQuery();
		public abstract TEntity Get(TPrimaryKey keyValue);
		public abstract Task<TEntity> GetAsync(TPrimaryKey keyValue);
		public abstract TEntity Insert(TEntity entity);
		public abstract Task<TEntity> InsertAsync(TEntity entity);
		public abstract void InsertMany(IEnumerable<TEntity> entities);
		public abstract Task InsertManyAsync(IEnumerable<TEntity> entities);
		public abstract TEntity Update(TEntity entity);
		public abstract void Delete(TEntity entity);
		public abstract void Delete(TPrimaryKey id);

		public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null
			, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBys = null
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, int? pageIndex = null
			, int? pageSize = null
			, bool disableTracking = true)
		{
			var query = BuildQuery(predicate, orderBys, includes, pageIndex, pageSize, disableTracking);
			return query.ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null
			, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBys = null
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, int? pageIndex = null
			, int? pageSize = null
			, bool disableTracking = true)
		{
			var query = BuildQuery(predicate, orderBys, includes, pageIndex, pageSize, disableTracking);
			return await ExecuteQuery(query);
		}

		public virtual TEntity Get(TPrimaryKey keyValue
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true)
		{
			return GetList(x => x.Id.Equals(keyValue), includes: includes, disableTracking: disableTracking).FirstOrDefault();
		}

		public virtual async Task<TEntity> GetAsync(TPrimaryKey keyValue
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true)
		{
			var entities = await GetListAsync(x => x.Id.Equals(keyValue), includes: includes, disableTracking: disableTracking);
			return entities.FirstOrDefault();
		}

		public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true)
		{
			return BuildQuery(includes: includes, disableTracking: disableTracking).FirstOrDefault(predicate);
		}

		public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, bool disableTracking = true)
		{
			return await BuildQuery(includes: includes, disableTracking: disableTracking).FirstOrDefaultAsync(predicate);
		}

		public virtual TEntity Upsert(TEntity entity)
		{
			return entity.IsTransient()
				? Insert(entity)
				: Update(entity);
		}

		public virtual Task<TEntity> UpsertAsync(TEntity entity)
		{
			return entity.IsTransient()
				? InsertAsync(entity)
				: UpdateAsync(entity);
		}

		public virtual Task<TEntity> UpdateAsync(TEntity entity)
		{
			return Task.FromResult(Update(entity));
		}

		public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
		{
			var entity = Get(id);
			updateAction(entity);
			return entity;
		}

		public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
		{
			var entity = await GetAsync(id);
			await updateAction(entity);
			return entity;
		}

		public virtual Task DeleteAsync(TEntity entity)
		{
			Delete(entity);
			return Task.CompletedTask;
		}

		public virtual Task DeleteAsync(TPrimaryKey id)
		{
			Delete(id);
			return Task.CompletedTask;
		}

		public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
		{
			foreach (var entity in GetList(predicate: predicate, disableTracking: false))
			{
				Delete(entity);
			}
		}

		public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
		{
			foreach (var entity in await GetListAsync(predicate: predicate, disableTracking: false))
			{
				await DeleteAsync(entity);
			}
		}

		protected virtual IQueryable<TEntity> BuildQuery(Expression<Func<TEntity, bool>> predicate = null
			, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBys = null
			, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
			, int? pageIndex = null
			, int? pageSize = null
			, bool disableTracking = true)
		{
			var query = disableTracking ? GetQuery().AsNoTracking() : GetQuery().AsQueryable();

			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			if (orderBys != null)
			{
				query = orderBys(query);
			}

			if (includes != null)
			{
				query = includes(query);
			}

			if (pageIndex.HasValue)
			{
				query = query.Skip((pageIndex.Value - 1) * pageSize.GetValueOrDefault(10));
			}

			if (pageSize.HasValue)
			{
				query = query.Take(pageSize.Value);
			}

			return query;
		}

		protected virtual async Task<IEnumerable<TEntity>> ExecuteQuery(IQueryable<TEntity> query)
		{
			return await query.ToListAsync();
		}
	}
}