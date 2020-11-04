using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace RTLTechTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScrapeController: ControllerBase
    {
        private readonly IShowService<ShowModel> _showService;

        public ScrapeController(IShowService<ShowModel> showService)
        {
            _showService = showService;
        }
        
        [HttpGet]
        public async Task<IEnumerable<ShowModel>> ScrapeShows([FromQuery] RequestModel model)
        {
            return await _showService.ScrapeShowsWithCast(model);
        }
    }
}