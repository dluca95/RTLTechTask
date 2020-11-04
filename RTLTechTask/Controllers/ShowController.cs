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
    public class ShowController: ControllerBase
    {
        private readonly IShowService<ShowModel> _showService;

        public ShowController(IShowService<ShowModel> showService)
        {
            _showService = showService;
        }
    
        [HttpGet]
        public async Task<IEnumerable<ShowModel>> GetShows([FromQuery] RequestModel requestModel)
        {
            return await _showService.GetShows(requestModel);
        }
        
        [HttpPost("add")]
        public async Task AddShows(IEnumerable<ShowModel> shows)
        {
            await _showService.AddShows(shows);
        }
    }
}