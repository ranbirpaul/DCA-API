using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Business_Model;
using DCAAPISample.Middleware;
using System.Net;

namespace DCAAPISample.Controllers
{
	/// <summary>
	/// DCA Object API performs the ADD/UPDATE/DELETE/UPDATE of the DCA Objects
	/// </summary>
	[Route("api/v1/[controller]")]
	public class DCAObjectsController : Controller
	{
		IDCAService _dcaService;
		/// <summary>
		///Initializes DCA Object
		/// </summary>
		public DCAObjectsController(IDCAService dcaService)
		{
			_dcaService = dcaService;
		}
		//RackController(IRackService 
		// GET api/values
		/// <summary>
		/// Retrieve the DCA Objects by Object ID and DCA Type
		/// </summary>
		/// <remarks>
		/// Retrieve the DCA Objects by Object ID and DCA Type
		/// </remarks>
		/// <param name="objectId"> ObjectId created during DCA Object creation</param>
		/// <param name="dcaType">DCA Type mapped with DCA Type API</param>
		/// <returns>A string status</returns>
		[HttpGet("{objectId}/dcaTypes/{dcaType}")]
		public async Task<IActionResult> GetObjectsAsync(string objectId, string dcaType)
		{
			try
			{
				var result=await  _dcaService.GetObjects(objectId, dcaType);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		
		/// <summary>
		/// Returns a object name of the specific DCA Type
		/// </summary>
		/// <remarks>
		/// Search DCA objects name of specific DCA Type 
		/// </remarks>
		/// <param name="name">The name to search for</param>
		/// <param name="dcaType">The dcaType to search for</param>
		/// <returns>A matched pairs with the name and DCA type</returns>
		[HttpGet("search/{name}/dcaTypes/{dcaType}")]
		public async Task<IActionResult> FilterName(string name, string dcaType)
		{
			try
			{
				var result= await _dcaService.FilterName(name, dcaType);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Adds DCA Objects to the API with unique name 
		/// </summary>
		/// <remarks>
		///  Adds DCA Objects to the API with unique name.
		/// </remarks>
		/// <param name="payload">{   
		/// "dcaType":"Rack",
		///"name":"Rack Name",
		///"properties": {
		///"serialNumber": {
		///"value": "123"
		///}
		/// }
		///}</param>
		/// <returns>Response 200 Success 
		/// 400 Bad Request </returns>
		///  POST api/values
		[HttpPost("AddWithNameSearch")]
		public async Task<IActionResult> PostAsync([FromBody]dynamic payload)
		{
			try
			{
				if (payload == null)
				{
					return Json(new Response(false, "Bad Request", "Invalid JSON format in payload"));
					
				}
				var result= await _dcaService.PostObjects(payload);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		/// <summary>
		/// Adds DCA Objects to the API 
		/// </summary>
		/// <remarks>
		///  Adds DCA Objects to the API without unique name. This method doesn't check whether DCA object with same name exists or not. 
		/// </remarks>
		/// <param name="payload">{   
		/// Example 
		/// "dcaType":"Rack",
		///"name":"Rack Name",
		///"properties": {
		///"serialNumber": {
		///"value": "123"
		///}
		/// }
		///}</param>
		/// <returns>Response 200 Success </returns>
	  /// <returns>Response 400 Bad Request </returns>
		/// <returns>Response 409 Conflict</returns>
		///  POST api/values
		[HttpPost("AddWithoutNameSearch")]
		public async Task<IActionResult> PostAsyncWithoutSearch([FromBody]dynamic payload)
		{
			try
			{
				if (payload == null)
				{
					return Json(new Response(false, "Bad Request", "Invalid JSON format in payload"));

				}
				var result= await _dcaService.PostObjectsWithoutSearch(payload);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		/// <summary>
		/// Updates DCA Objects to the API 
		/// </summary>
		/// <remarks>
		///  Updates DCA Objects to the API based on provided Object Id and DCA Type
		/// </remarks>
		/// <param name="objectId">DCA Object Id</param>
		/// <param name="dcaType">DCA Type of the Object</param>
		/// <param name="payload">{   
		/// Example 
		/// "dcaType":"Rack",
		///"name":"Rack Name",
		///"properties": {
		///"serialNumber": {
		///"value": "123"
		///}
		/// }
		///}</param>
		/// <returns>Response 200 Success 
		/// 400 Bad Request </returns>
	   /// <returns>Response 409 Conflict</returns>
		[HttpPut("{objectId}/dcaTypes/{dcaType}")]
		public async Task<IActionResult> PutAsync(string objectId, string dcaType, [FromBody] dynamic payload)
		{
			try
			{
				var result = await _dcaService.UpdateObjects(objectId, dcaType, payload);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		/// <summary>
		/// Deletes DCA Objects to the API 
		/// </summary>
		/// <remarks>
		///  Delete DCA Objects from the API of the specific Object Id and DCA Type
		/// </remarks>
		/// <param name="objectId">
		///</param>
		///	/// <param name="dcaType">
		///</param>
		/// <returns>Response 200 Success 
		/// 400 Bad Request </returns>
		[HttpDelete("{objectId}/dcaTypes/{dcaType}")]
		public async Task<IActionResult> DeleteAsync(string objectId, string dcaType)
		{
			try
			{
			 var result= await _dcaService.DeleteObjects(objectId, dcaType);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
