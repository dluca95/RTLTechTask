using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Models;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Persistence.Models;

namespace Application
{
    public class ShowCastService: IShowCastService
    {
        private readonly IDbService<Show> _showDbService;
        private readonly IDbService<ActorShow> _castService;
        private readonly IDbService<Actor> _actorService;

        public ShowCastService(IDbService<Show> showDbService, IDbService<ActorShow> castService, IDbService<Actor> actorService)
        {
            _showDbService = showDbService;
            _castService = castService;
            _actorService = actorService;
        }
        
        public IEnumerable<IShowModel> GetNewShowsFrom(IEnumerable<IShowModel> showModels)
        {
            var data = showModels.DistinctBy(s => s.Id).ToList();
            var contentIds = data.Select(s => s.Id);
            
            var existingShows = GetExistingShows(contentIds);
            
            return data.Where(sm => existingShows.All(s => sm.Id != s.Id));
        }

        public IEnumerable<IActorModel> GetNewActorsFrom(IEnumerable<IShowModel> showModels)
        {
            var castModels = showModels
                .SelectMany(c => c.Cast).ToList().DistinctBy(c => c.Id);

            var actors = _actorService.GetAll();
            
            return castModels.Where(c => !actors.Any(a => a.Id == c.Id));
        }
        
        public async Task<IEnumerable<IActorShowModel>> GetAllCastFor(IEnumerable<IShowModel> showModels)
        {
            var data = showModels.ToList();
            var contentIds = data.Select(s => s.Id);

            var castMembersOfExistingShows = await GetCastForExistingShows(contentIds);

            return data
                .SelectMany(s => s.Cast
                    .Where(a => castMembersOfExistingShows
                        .All(c => (c.ActorId == a.Id && c.ShowId != s.Id) || c.ActorId != a.Id))
                .DistinctBy(a => a.Id)
                .Select(c => new ActorShowModel
                        {
                            ActorId = c.Id,
                            ShowId = s.Id
                        }
                    ));
        }

        private async Task<IEnumerable<ActorShow>> GetCastForExistingShows(IEnumerable<int> showsIds)
        {
            var existingShows = GetExistingShows(showsIds);

            return await existingShows.Join(_castService.GetAll(), show => show.Id,
                    model => model.ShowId, (show, model) => model)
                .ToListAsync();
        }
        
        private IQueryable<Show> GetExistingShows(IEnumerable<int> showIds)
        {
            return _showDbService.GetAll()
                .Include(s => s.Cast)
                .ThenInclude(s => s.Actor)
                .Where(s => showIds.Contains(s.Id));        
        }
    }
}