using Business_Model;
using Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
	public interface IDCARepository
	{
	 Task<Response> GetObjects(string objectId, string dcaType,bool parseResult=true);
	Task<Response> SendObjects(dynamic model,bool serialized,bool isValidateNameReq= false);
	Task<Response> FilterName(string name, string dcaType);
   Task<Response> RemoveObjects(string objectId,string modelId);
	Task<Response> UpdateObjects(string objectId, string modelId, dynamic model);
	}
}
