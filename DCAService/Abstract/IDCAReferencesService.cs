using Business_Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstract
{
	public interface IDCAReferencesService
	{
		Task<Response> GetReferences(string referenceId);
		Task<dynamic> GetReferences(string objectId,string dcaType);
		Task<dynamic> GetChildObjectReferences(string objectId, string dcaType);
		Task<Response> PostReferences(string parentObjectId, string parentdcaType,dynamic model);
		Task<Response> DeleteReferences(string referenceId);
		Task<Response> UpdateReferences(string referenceId , dynamic model);
		
		}
}
