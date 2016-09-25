using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext Context { get; set; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public TDestination GetAs<TModel, TDestination>(Int32 id) where TModel : BaseModel
        {
            return Context.Set<TModel>().Where(model => model.Id == id).ProjectTo<TDestination>().FirstOrDefault();
        }
        public TModel Get<TModel>(Int32 id) where TModel : BaseModel
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
            EntityEntry<TModel> entry = Context.Entry(model);
            if (entry.State != EntityState.Modified && entry.State != EntityState.Unchanged)
                entry.State = EntityState.Modified;

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
        public void Delete<TModel>(Int32 id) where TModel : BaseModel
        {
            Delete(Context.Set<TModel>().Single(model => model.Id == id));
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
