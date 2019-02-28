using Business_Model;
using Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstract
{
	public	interface IQueryService
	{
		 Task<Response> SendQuery(string query);

	}
}
