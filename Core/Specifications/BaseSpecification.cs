using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {

        public BaseSpecification()
        {

        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool isPagingEnabled { get; private set; }
        private string _search;

        public string Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpresssion)
        {
            Includes.Add(includeExpresssion);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpresssion)
        {
            OrderBy = orderByExpresssion;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByExpresssion)
        {
            OrderByDescending = orderByExpresssion;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            isPagingEnabled = true;
        }

       
    }
}
