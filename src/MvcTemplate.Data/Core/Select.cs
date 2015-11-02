using AutoMapper.QueryableExtensions;
using MvcTemplate.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MvcTemplate.Data.Core
{
    public class Select<TModel> : ISelect<TModel> where TModel : BaseModel
    {
        public Type ElementType => Set.ElementType;
        public Expression Expression => Set.Expression;
        public IQueryProvider Provider => Set.Provider;

        private IQueryable<TModel> Set { get; set; }

        public Select(IQueryable<TModel> set)
        {
            Set = set;
        }

        public ISelect<TModel> Where(Expression<Func<TModel, Boolean>> predicate)
        {
            Set = Set.Where(predicate);

            return this;
        }

        public IQueryable<TView> To<TView>() where TView : BaseView
        {
            return Set.ProjectTo<TView>();
        }

        public IEnumerator<TModel> GetEnumerator()
        {
            return Set.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
