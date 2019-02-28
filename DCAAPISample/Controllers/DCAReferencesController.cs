using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Business_Model;

namespace DCAAPISample.Controllers
{
	/// <summary>
	/// DCA Reference preforms the Add/Delete/Update of the DCA references
	/// </summary>
	[Route("api/v1/[controller]")]
	public class DCAReferencesController : Controller
	{
		IDCAReferencesService _dcaReferencesService;
		/// <summary>
		/// Initializes DCA References
		/// </summary>
		public DCAReferencesController(IDCAReferencesService dcaReferencesService)
		{
			_dcaReferencesService = dcaReferencesService;
		}

		/// <summary>
		/// Retrieves DCA Reference by reference Id 
		/// </summary>
		/// <remarks>
		///Retrieves DCA Reference by reference Id 
		/// </remarks>
		/// <param name="referenceId">DCA Reference Id </param>
		/// <returns>Response 200 Success </returns>
		[HttpGet("{referenceId}")]
		public async Task<IActionResult> GetReferencesAsync(string referenceId)
		{
			try
			{
				var result= await _dcaReferencesService.GetReferences(referenceId);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Retrieves DCA Reference by Object Id and DCAType
		/// </summary>
		/// <remarks>
		/// Updates specific all DCA Types
		/// </remarks>
		/// <param name="objectId"> DCA Object Id</param>
	   /// <param name="dcaType">DCA Type </param>
		/// <returns>Response 200 Success </returns>
		[HttpGet("/api/v1/objects/{objectId}/dcaTypes/{dcaType}/references")]
		public async Task<IActionResult> GetReferencesBasedOnObjectAndDCATypeAsync(string objectId,string dcaType)
		{
			try
			{
				var result= await _dcaReferencesService.GetReferences(objectId, dcaType);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Retrieves Child Object References by Object Id and DCAType
		/// </summary>
		/// <remarks>
		/// Gets Child Object References by Object Id and DCAType
		/// </remarks>
		/// <param name="objectId">Parent Object Id</param>
		/// <param name="dcaType">Parent DCA Type
		///</param>
		/// <returns>Response 200 Success </returns>
		[HttpGet("/api/v1/objects/{objectId}/dcaTypes/{dcaType}/childObjectreferences")]
		public async Task<IActionResult> GetChildObjectReferences(string objectId, string dcaType)
		{
			try
			{
				var result=  await _dcaReferencesService.GetChildObjectReferences(objectId, dcaType);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Creates Object References from Parent to Child Object
		/// </summary>
		/// <remarks>
		///  Creates a Object Reference between Parent and Child Object
		/// </remarks>
		/// <param name="objectId">Parent Object Id</param>
		/// <param name="dcaType"> Parent DCA type</param>
		/// <param name="payload">
		/// [
		///		 {
		///    "name": "children",
		///    "to": {
		///      "objectId": Child Object Id
		///      "dcaType": Child DCA Type
		///    }
		///  }
		///]
		/// </param>
		/// <returns>Response 200 Success </returns>
		// POST api/values
		[HttpPost("Add")]
		public async Task<IActionResult> PostAsync(string objectId,string dcaType,[FromBody]dynamic payload)
		{
			try
			{
				var result= await _dcaReferencesService.PostReferences(objectId, dcaType, payload);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		//// PUT api/values/5
		//[HttpPatch("{referenceId}/dcaTypes/{dcaType}")]
		//public async Task<Response> PatchAsync(string referenceId,[FromBody] dynamic value)
		//{
		//	try
		//	{
		//		return await _dcaReferencesService.UpdateReferences(referenceId,value);
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new Exception(ex.Message);
		//	}
		//}

		/// <summary>
		/// Deletes  Object References by reference Id
		/// </summary>
		/// <remarks>
		/// Deletes given DCA Reference
		/// </remarks>
		/// <param name="referenceId"></param>
		/// <returns>Response 200 Success </returns>
		[HttpDelete("{referenceId}")]
		public async Task<IActionResult> DeleteAsync(string referenceId)
		{
			try
			{
				var result=await _dcaReferencesService.DeleteReferences(referenceId);
				return Ok(result);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
