using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Interfaces;
using Persistence;
using Persistence.Models;

namespace Application.DbServices
{
    public class ActorDbService: IDbService<Actor>
    {
        private readonly AppDbContext _appDbContext;

        public ActorDbService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(IEnumerable<Actor> data)
        {
            await _appDbContext.Actors.AddRangeAsync(data);
            
            await _appDbContext.SaveChangesAsync();
        }

        public IQueryable<Actor> GetAll(List<Expression<Func<Actor, bool>>> filters = null)
        {
            if (filters == null) return _appDbContext.Actors.AsQueryable();

            return filters.Aggregate(_appDbContext.Actors.AsQueryable(),
                ((set, expression) =>
                    set.Where(expression)));
        }
    }
}