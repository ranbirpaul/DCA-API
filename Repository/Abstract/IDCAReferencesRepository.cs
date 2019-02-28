using Business_Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
	public interface IDCAReferencesRepository
	{
	 Task<Response> GetReferences(string objectId, string dcaType);
	Task<Response> GetChildObjects(string objectId, string dcaType);
	Task<Response> GetReferences(string referenceId);
	Task<Response> SendReferences(string parentObjectId, string parentModelId,dynamic model);
	Task<Response> RemoveReferences(string referenceId);
	Task<Response> UpdateReferences(string referenceId,dynamic objects);
	}
}
