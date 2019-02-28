using Business_Model;
using Repository.Abstract;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class QueryService: IQueryService
	{
		IQueryRepository _queryRepository;
		public QueryService(IQueryRepository queryRepository)
		{
			_queryRepository = queryRepository;
		}

		public async Task<Response> SendQuery(string query)
		{
			return await _queryRepository.SendQuery(query);
		}
	}
}
