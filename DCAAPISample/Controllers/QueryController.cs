using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Model;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCAAPISample.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	public class QueryController : Controller
	{
		IQueryService _queryService;
		public QueryController(IQueryService queryService)
		{
			_queryService = queryService;
		}

		/// <summary>
		/// Query API
		/// </summary>
		/// <remarks>
		///  Query syntax will remain same as below except one thing i.e.. replace model with dcaType
	///	https://abb.sharepoint.com/sites/ABBAbility/Wiki/Pages/API%20Development/Domain%20Specific%20Language%20Overview.aspx
		/// </remarks>
		/// <param name="query">
		/// Example
	///".dcaType('Rack')" 
		///</param>
		/// <returns>Response 200 Success </returns>
		[HttpPost("SendQuery")]
		public async  Task<IActionResult> SendQuery([FromBody]dynamic query)
		{
			if (query == null )
			{
				return Json(new Response(false, "Bad Request", "Invalid JSON format in payload"));
			}
			if (query is string)
			{
				if (query.ToString().Length <= 0)
				{

					return Json(new Response(false, "Bad Request", " JSON format is blank or empty in payload"));
				}
			}
			var result= await _queryService.SendQuery(query);
			return Ok(result);
		}

	}
}
