using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Interfaces;
using Application.Interfaces.Models;
using Application.Models;
using AutoMapper;
using Common;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application
{
    public class ShowService : IShowService<ShowModel>
    {
        private readonly IScraperService _scraperService;
        private readonly IDbService<Show> _showDbService;
        private readonly IDbService<Actor> _actorDbService;
        private readonly IDbService<ActorShow> _castDbService;
        private readonly IShowCastService _showCastService;
        private readonly IMapper _mapper;

        public ShowService(IScraperService scraperService,
            IDbService<Show> dbService,
            IDbService<Actor> actorDbService, IDbService<ActorShow> castService, IMapper mapper, IShowCastService showCastService)
        {
            _scraperService = scraperService;
            _showDbService = dbService;
            _actorDbService = actorDbService;
            _castDbService = castService;
            _mapper = mapper;
            _showCastService = showCastService;
        }

        public async Task<IEnumerable<ShowModel>> ScrapeShowsWithCast(IRequestModel model)
        {
            var scrapedShows = (await _scraperService
                .ScrapeWith<TvMazeShowResponse>(model)).ToList();

            var showIds = scrapedShows.Select(s => s.Show.Id);

            var scrapedActors = await _scraperService
                .ScrapeManyWithSubPath<TvMazeCastResponse>("shows", showIds, "cast");

            var shows = scrapedShows.GroupJoin(scrapedActors,
                showResponse => showResponse.Show.Id,
                actorResponse => actorResponse.ParentId, (showResponse, castResponse) =>
                {
                    showResponse.Show.Cast = castResponse
                        .Select(s => s.Person)
                        .OrderByDescending(a => a.Birthday);
                    
                    return showResponse.Show;
                });

            return shows;
        }

        public async Task<IEnumerable<ShowModel>> GetShows(IRequestModel model)
        {
             var skip = model.Count * (model.Page - 1);
             var filters = new List<Expression<Func<Show, bool>>>
             {
                 s => s.Name.Contains(model.Query)
             };
             
             var shows = await _showDbService
                .GetAll(filters)
                .Skip(skip)
                .Take(model.Count)
                .ToListAsync();
             
             return shows.Map<Show, ShowModel>(_mapper);
        }

        public async Task AddShows(IEnumerable<ShowModel> data)
        {
            var showModels = data.ToList();

            var newShows = _showCastService
                .GetNewShowsFrom(showModels)
                .Map<IShowModel, Show>(_mapper);

            var newActors = _showCastService
                .GetNewActorsFrom(showModels)
                .Map<IActorModel, Actor>(_mapper).ToList();

            var allNewActorShows = (await _showCastService
                    .GetAllCastFor(showModels))
                .Map<IActorShowModel, ActorShow>(_mapper).ToList();

            await _showDbService.Add(newShows);
            await _actorDbService.Add(newActors);
            await _castDbService.Add(allNewActorShows);
        }
    }
}