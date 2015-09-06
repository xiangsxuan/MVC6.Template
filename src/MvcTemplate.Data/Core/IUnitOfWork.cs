using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        TDestination GetAs<TModel, TDestination>(String id) where TModel : BaseModel;
        TModel Get<TModel>(String id) where TModel : BaseModel;
        TDestination To<TDestination>(Object source);

        ISelect<TModel> Select<TModel>() where TModel : BaseModel;

        void Insert<TModel>(TModel model) where TModel : BaseModel;
        void Update<TModel>(TModel model) where TModel : BaseModel;
        void Delete<TModel>(TModel model) where TModel : BaseModel;
        void Delete<TModel>(String id) where TModel : BaseModel;

        void Rollback();
        void Commit();
    }
}
