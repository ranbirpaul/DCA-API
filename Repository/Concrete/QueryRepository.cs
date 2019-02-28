using Business_Model;
using Microsoft.Extensions.Options;
using Repository.Abstract;
using Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public class QueryRepository: IQueryRepository
	{
		ApiClient client;
		Uri uri;
		QueryModel queryModel = new QueryModel();
		string queryPath = string.Empty;
		IDCATypeRepository _dCATypeRepository;
		public QueryRepository(IOptions<Settings> apisettings,IDCATypeRepository dCATypeRepository)
		{
			uri = new Uri(apisettings.Value.InfoModelBaseAddress);
			client = new ApiClient(uri, apisettings.Value.infoModelToken);
			queryPath = apisettings.Value.QueryPath;
			_dCATypeRepository = dCATypeRepository;
		}
		public async Task<dynamic> ObjectExists(string objetId,string modelId)
		{
			queryModel.query = "models('" + modelId + "').hasObjectId('" + objetId + ";)";
			var result = await client.PostAsync(queryPath, queryModel);
			return result;
		}

		public async Task<Response> SendQuery(string query)
		{
			int dcaTypeStartIndex = query.IndexOf("dcaType");
			if (dcaTypeStartIndex == -1)
				throw new Exception("Invalid query syntax");
			dcaTypeStartIndex += 9;
			int dcaTypeIndex = query.IndexOf(")");
			var dcaType= query.Substring(dcaTypeStartIndex, dcaTypeIndex- dcaTypeStartIndex-1);
			var dcaTypeRepository = _dCATypeRepository.GetDCAType(dcaType);
			var newQuery = "models('" + dcaTypeRepository.AbilityModel + "')" + query.Substring(dcaTypeIndex+1);
			queryModel.query = newQuery;
			var result = await client.PostAsync(queryPath, queryModel);
			if (result.IsSuccess)
			{
				result = result.ParseQueryResult(result, _dCATypeRepository);
			}
			return result;
		}
	}
}
