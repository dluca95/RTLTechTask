using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper.Internal;
using Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application
{
    public class TvMazeService: IScraperService
    {
        private readonly IHttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TvMazeService(IHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<T>> ScrapeWith<T>(IRequestModel model) where T: IAppModel
        {
            var scrapeResult = await _httpClient.GetAll<T>(model.Query);

            if (model.Page == 0 && model.Count == 0)
                return scrapeResult;
                
            var skip = model.Count * (model.Page - 1);

            return scrapeResult
                .Skip(skip)
                .Take(model.Count);        
        }
        
        public async Task<IEnumerable<T>> ScrapeManyWithSubPath<T>(string parentPath, IEnumerable<int> parentsId, string subPath) where T : IAppModel
        {
            int.TryParse(_configuration["BatchSize"], out var batchSize);
            
            var tasks = new List<Task<IEnumerable<T>>>();
            var listOfIds = parentsId.ToList();
            var numberOfBatches = (int)Math.Ceiling((double)listOfIds.Count / batchSize);
 
            for (var i = 0; i < numberOfBatches; i++)
            {
                var currentIds = listOfIds.Skip(i * batchSize).Take(batchSize);
                
                tasks.AddRange(from id in currentIds
                    let showCastPath = $"shows/{id}/{subPath}"
                    select _httpClient.GetAll<T>(showCastPath)
                        .ContinueWith(a =>
                        {
                            a.Result.ForAll(s => s.ParentId = id);
                            return a.Result;
                        }));
            }
            
            return (await Task.WhenAll(tasks)).SelectMany(u => u);
        }
    }
}