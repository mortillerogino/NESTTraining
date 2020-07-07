using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;
using NuSearch.Domain.ConfigServer;
using NuSearch.Domain.Model;
using NuSearch.Web.Models;

namespace NuSearch.Web.Controllers
{
	public class SearchController : Controller
    {
		private readonly IElasticClient _client;

		public SearchController(IElasticClient client, IOptionsSnapshot<ConfigServerData> configServerData)
		{
			// Debug here to check configServerData
			_client = client;
		}

	    [HttpGet]
        public IActionResult Index(SearchForm form)
        {
			var result = _client.Search<Package>(s => s
							.Size(25)
							.Query(q => q
								.QueryString(qs => qs
									.Query(form.Query)
								)
							));

			var model = new SearchViewModel
			{
				Hits = result.Hits,
				Total = result.Total,
				Form = form
			};

			return View(model);
		}
    }
}
