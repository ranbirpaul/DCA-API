using Business_Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
	public interface IQueryRepository
	{
	 Task<dynamic> ObjectExists(string objectId, string modelId);
	 Task<Response> SendQuery(string query);
	}
}
