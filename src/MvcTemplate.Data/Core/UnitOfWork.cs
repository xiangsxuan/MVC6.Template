using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext Context { get; set; }
        private Boolean Disposed { get; set; }
        private IAuditLogger Logger { get; }

        public UnitOfWork(DbContext context, IAuditLogger logger = null)
        {
            Context = context;
            Logger = logger;
        }

        public TDestination GetAs<TModel, TDestination>(String id) where TModel : BaseModel
        {
            return Context.Set<TModel>().Where(model => model.Id == id).ProjectTo<TDestination>().FirstOrDefault();
        }
        public TModel Get<TModel>(String id) where TModel : BaseModel
        {
            return Context.Set<TModel>().SingleOrDefault(model => model.Id == id);
        }
        public TDestination To<TDestination>(Object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        public ISelect<TModel> Select<TModel>() where TModel : BaseModel
        {
            return new Select<TModel>(Context.Set<TModel>());
        }

        public void InsertRange<TModel>(IEnumerable<TModel> models) where TModel : BaseModel
        {
            Context.AddRange(models);
        }
        public void Insert<TModel>(TModel model) where TModel : BaseModel
        {
            Context.Add(model);
        }
        public void Update<TModel>(TModel model) where TModel : BaseModel
        {
            EntityEntry<TModel> entry = Context.Update(model);
            entry.Property(property => property.CreationDate).IsModified = false;
        }

        public void DeleteRange<TModel>(IEnumerable<TModel> models) where TModel : BaseModel
        {
            Context.RemoveRange(models);
        }
        public void Delete<TModel>(TModel model) where TModel : BaseModel
        {
            Context.Remove(model);
        }
        public void Delete<TModel>(String id) where TModel : BaseModel
        {
            Delete(Context.Set<TModel>().Single(model => model.Id == id));
        }

        public void Rollback()
        {
            Context.Dispose();
            Context = Activator.CreateInstance(Context.GetType()) as DbContext;
        }
        public void Commit()
        {
            Logger?.Log(Context.ChangeTracker.Entries<BaseModel>());
            Context.SaveChanges();
            Logger?.Save();
        }

        public void Dispose()
        {
            if (Disposed) return;

            Logger?.Dispose();
            Context.Dispose();

            Disposed = true;
        }
    }
}
