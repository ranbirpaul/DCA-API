using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Business_Model;

namespace DCAAPISample.Controllers
{
	[Produces("application/json")]
	[Route("api/DCATypes")]
	/// <summary>
	/// DCA Type API 
	/// </summary>
	public class DCATypesController : Controller
	{
		IDCATypeService _dcaTypeService;
		/// <summary>
		/// Initializes DCA Types
		/// </summary>
		public DCATypesController(IDCATypeService dcaTypeService)
		{
			_dcaTypeService = dcaTypeService;
		}

		/// <summary>
		/// Get DCA Type data of specific DCA Type
		/// </summary>
		/// <remarks>
		/// Retrieves DCA Type of specific DCA Type.
		/// </remarks>
		/// <param name="type">
		///</param>
		/// <returns>Response 200 Success </returns>
		[HttpGet("{type}")]
		public IActionResult GetDCAType(string type)
		{
			try
			{
				return  Ok(_dcaTypeService.GetDCAType(type));
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Gets all the DCA Types 
		/// </summary>
		/// <remarks>
		/// Retrieves all the DCA Types
		/// </remarks>
		/// <returns>Response 200 Success </returns>
		// GET api/values
		[HttpGet]
		public IActionResult GetDCATypes()
		{
			try
			{
				return Ok(_dcaTypeService.GetDCATypes());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Adds new DCA Type into the API
		/// </summary>
		/// <remarks>
		/// Retrieves all the DCA Types
		/// </remarks>
	  /// <param name="dCAType">
		///</param>
		/// <returns>Response 200 Success </returns>
		// POST api/values
		[HttpPost("AddDCAType")]
		public IActionResult Post([FromBody]DCATypeModel dCAType)
		{
			try
			{
				return Ok( _dcaTypeService.SendDCATypes(dCAType));
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		//// DELETE api/values/5
		//[HttpDelete("{type}")]
		//public async void Delete(string type)
		//{

		//}
		/// <summary>
		/// Updates DCA Type 
		/// </summary>
		/// <remarks>
		/// Updates specific DCA Type
		/// </remarks>
		/// <param name="dCAType">
		///</param>
		/// <returns>Response 200 Success </returns>
		// POST api/values
		[HttpPut]
		public IActionResult UpdateDCATypes([FromBody]DCATypeModel dCAType)
		{
			try
			{
				return Ok(_dcaTypeService.UpdateDCATypes(dCAType));
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
