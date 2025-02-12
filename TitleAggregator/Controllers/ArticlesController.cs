using Microsoft.AspNetCore.Mvc;
using TitleAggregator.Services;

namespace TitleAggregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly WiredScraperService _wiredScraper;

        public ArticlesController(WiredScraperService wiredScraper)
        {
            _wiredScraper = wiredScraper;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            var articles = await _wiredScraper.GetArticlesAsync();
            return Ok(articles);
        }
    }
}
