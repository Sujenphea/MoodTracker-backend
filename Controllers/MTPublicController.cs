using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MoodTrackerBackendCosmos.Controllers
{
    [Route("api")]
    [ApiController]
    public class MTPublicController: Controller
    {
        private static readonly HttpClient client = new HttpClient();

        [HttpGet("GetQuote")]
        public async System.Threading.Tasks.Task<ActionResult> GetQuoteAsync()
        {
            // https://stackoverflow.com/questions/6620165/how-can-i-parse-json-with-c
            // https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient

            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("http://api.forismatic.com/api/1.0/?method=getQuote&lang=en&format=json");

            var msg = await stringTask;

            dynamic stuff = JObject.Parse(msg);

            //string text = stuff.quoteText;
            //string author = stuff.quoteAuthor;
            //string link = stuff.quoteLink;

            //return Ok(msg);
            return Ok(msg);
        }
    }
}
