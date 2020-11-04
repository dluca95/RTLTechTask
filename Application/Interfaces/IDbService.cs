using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;

namespace Application.Interfaces
{
    public interface IDbService<T> where T : IEntityModel
    {
        public Task Add(IEnumerable<T> data);
        public IQueryable<T> GetAll(List<Expression<Func<T, bool>>> filters = null);
    }
}