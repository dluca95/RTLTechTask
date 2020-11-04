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
    public class CastDbService: IDbService<ActorShow>
    {
        private readonly AppDbContext _appDbContext;

        public CastDbService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(IEnumerable<ActorShow> data)
        {
            await _appDbContext.Cast.AddRangeAsync(data);
            await _appDbContext.SaveChangesAsync();
        }

        public IQueryable<ActorShow> GetAll(List<Expression<Func<ActorShow, bool>>> filters = null)
        {
            if (filters == null) return _appDbContext.Cast.AsQueryable();

            return filters.Aggregate(_appDbContext.Cast.AsQueryable(),
                ((set, expression) =>
                    set.Where(expression)));
        }
    }
}