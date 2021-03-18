using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CqrsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {     
        private readonly IQueryProcessor _queryProcessor;

        public WeatherForecastController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {   
            var test = await _queryProcessor.Query<GetSomethingQuery, GetSomethingQueryResponse>(new GetSomethingQuery());
            return Ok();
        }
    }
}
