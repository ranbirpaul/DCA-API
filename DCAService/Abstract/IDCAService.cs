using Business_Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstract
{
	public interface IDCAService
	{
		Task<Response> GetObjects( string objectId, string dcaType);
		Task<Response> FilterName(string name, string dcaType);
		Task<Response> PostObjects(dynamic payload);
		Task<Response> PostObjectsWithoutSearch(dynamic payload);
		Task<Response> DeleteObjects(string objectId, string dcaType);
		Task<Response> UpdateObjects(string objectId, string dcaType, dynamic payload);
		
		}
}
