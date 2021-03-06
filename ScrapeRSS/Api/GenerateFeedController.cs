﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScrapeRSS.Data;
using ScrapeRSS.Models;

namespace ScrapeRSS.Api
{
    [Route("api/generate-feed")]
    [ApiController]
    public class GenerateFeedController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> GetFeed([FromQuery]FeedGeneratorSettings settings)
        {
            if (!settings.Validate())
            {
                return Content(settings.ValidationError, "text/plain");
            }

            return Content(await RssFeedBuilder.GetFeed(settings), "text/xml");
        }
    }
}