using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;

namespace Application.DbServices
{
    public class ShowDbService: IDbService<Show>
    {
        private readonly AppDbContext _appDbContext;
        
        public ShowDbService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(IEnumerable<Show> data)
        {
            await _appDbContext.Shows.AddRangeAsync(data);
            await _appDbContext.SaveChangesAsync();
        }

        public IQueryable<Show> Get(Expression<Func<Show,bool>> filter)
        {
            return _appDbContext.Shows
                .Where(filter)
                .Include(s => s.Cast
                    .OrderByDescending(c => c.Actor.Birthday))
                .ThenInclude(s => s.Actor);
        }

        public IQueryable<Show> GetAll(List<Expression<Func<Show, bool>>> filters = null)
        {
            if (filters == null) 
                return _appDbContext.Shows
                .AsQueryable()
                .Include(s => s.Cast
                    .OrderByDescending(c => c.Actor.Birthday))
                .ThenInclude(s => s.Actor);;

            return filters.Aggregate(_appDbContext.Shows.AsQueryable(),
                (set, expression) => set.Where(expression))
                .Include(s => s.Cast
                    .OrderByDescending(c => c.Actor.Birthday))
                .ThenInclude(s => s.Actor);;
        }
    }
}