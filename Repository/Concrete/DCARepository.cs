using Business_Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
	public class DCARepositry : IDCARepository
	{
		Uri uri = null;// new Uri("https://abiimsvcasbtc2eundev.azurewebsites.net/");
		ApiClient client;
		string editPath = string.Empty;
		string retrievePath = string.Empty;
		string dslQueryPath = string.Empty;
		IDCATypeRepository _dCATypeRepository;
		public DCARepositry(IOptions<Settings> apisettings, IDCATypeRepository dCATypeRepository)
		{
			uri = new Uri(apisettings.Value.InfoModelBaseAddress);
			client = new ApiClient(uri, apisettings.Value.infoModelToken);
			editPath = apisettings.Value.InfoObjects;
			retrievePath = apisettings.Value.RetrieveObjects;
			dslQueryPath = apisettings.Value.QueryPath;
			_dCATypeRepository = dCATypeRepository;
		}
		public async Task<Response> FilterName(string name, string dcaType)
		{
			DCATypeModel dcaModel = _dCATypeRepository.GetDCAType(dcaType);
			var model = dcaModel.AbilityModel;
			var query = "models('" + model + "').hasName('" + name + "')";
			Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
			keyValuePairs.Add("query", query);
			var result = await client.PostAsync(dslQueryPath, keyValuePairs);
			if (result.IsSuccess)
			{
				result = result.ParseQueryResult(result, _dCATypeRepository);
			}
			return result;
		}
		public async Task<Response> GetObjects(string objectId, string dcaType, bool parseResult = true)
		{
			DCATypeModel dcaModel = _dCATypeRepository.GetDCAType(dcaType);
			if (dcaModel == null)
			{
				throw new Exception(string.Format("DCA type mapping is not found for type:{0}", dcaType));
			}
			string path = String.Format(retrievePath, objectId, dcaModel.AbilityModel);
			var result = await client.GetAsync(path);
			if (parseResult)
			{
				result = result.ParseResult(result, _dCATypeRepository);
			}
			return result;
		}
		public async Task<Response> GetAPI()
		{
			var result = await client.GetAsync(editPath);
			return result;
		}


		public async Task<Response> RemoveObjects(string objectId, string dcaType)
		{
			DCATypeModel dcaModel = _dCATypeRepository.GetDCAType(dcaType);
			if (dcaModel == null)
			{
				throw new Exception(string.Format("DCA type mapping is not found for type:{0}", dcaType));
			}
			string path = String.Format(retrievePath, objectId, dcaModel.AbilityModel);
			var result = await client.DeleteAsync(path);
			return result;
		}

		public async Task<Response> SendObjects(dynamic model, bool serialized, bool isValidateNameReq = false)
		{
			Dictionary<string, string> dcakeyValuePairs = new Dictionary<string, string>(); Response result = null;
			try
			{
				if (serialized)
				{
					//Convert into dictionary type KeyValue Pair
					dcakeyValuePairs =
				   ((IEnumerable<KeyValuePair<string, JToken>>)model)
						.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());


					//Fetch the DCA Type and get its corresponding Ability model and type
					dynamic dcaType = dcakeyValuePairs["dcaType"];
					DCATypeModel dcaModel = _dCATypeRepository.GetDCAType(dcaType);
					if (dcaModel == null)
					{
						throw new Exception(string.Format("DCA type mapping is not found for type:{0}", dcaType));
					}
					//Delete dcaType and add model and device type
					dcakeyValuePairs.Remove("dcaType");
					dcakeyValuePairs.Add("model", dcaModel.AbilityModel);
					dcakeyValuePairs.Add("type", dcaModel.AbilityType);
					if (isValidateNameReq)
					{
						var searchresult = await FilterName(dcakeyValuePairs["name"], dcaModel.DCAType);
						if (searchresult.IsSuccess == true)
						{
							var searchresponse = searchresult.ResponseMessage as List<dynamic>;

							if (searchresponse.Count()>0)
							{
								searchresult.ResponseMessage = string.Format("DCA Object name: {0} already exists in dcaType:{1}", dcakeyValuePairs["name"], dcaType);
								searchresult.IsSuccess = false;
								searchresult.StatusCode = "Conflict";
								return searchresult;
							}
						}
					}

					var dcaDeviceCollection = new Dictionary<string, dynamic>();
					foreach (var keyvaluepair in dcakeyValuePairs)
					{
						var objvalue = dcakeyValuePairs[keyvaluepair.Key];
						if (objvalue.IsValidObject())
						{
							dcaDeviceCollection.Add(keyvaluepair.Key, JToken.Parse(objvalue));
						}
						else
							dcaDeviceCollection.Add(keyvaluepair.Key, objvalue);


						result = await client.PostAsync(editPath, dcaDeviceCollection);
					}
				}
				else
				{
					result = await client.PostAsync(editPath, model);
				}
				result = result.ParseResult(result, _dCATypeRepository);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<Response> UpdateObjects(string objectId, string dcaType, dynamic model)
		{
			try
			{
				//Convert into dictionary type KeyValue Pair
				Dictionary<string, string> dcakeyValuePairs =
				((IEnumerable<KeyValuePair<string, JToken>>)model)
					 .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
				//Fetch the DCA Type and get its corresponding Ability model and type
				DCATypeModel dcaModel = _dCATypeRepository.GetDCAType(dcaType);

				//Delete dcaType and add model and device type
				dcakeyValuePairs.Remove("dcaType");
				dcakeyValuePairs.Add("model", dcaModel.AbilityModel);
				dcakeyValuePairs.Add("type", dcaModel.AbilityType);
				var dcaDeviceCollection = new Dictionary<string, dynamic>();
				foreach (var keyvaluepair in dcakeyValuePairs)
				{
					var objvalue = dcakeyValuePairs[keyvaluepair.Key];
					if (objvalue.IsValidObject())
					{
						dcaDeviceCollection.Add(keyvaluepair.Key, JToken.Parse(objvalue));
					}
					else
						dcaDeviceCollection.Add(keyvaluepair.Key, objvalue);
				}
				string path = String.Format(retrievePath, objectId, model);
				var result = await client.PutAsync(path, dcaDeviceCollection);
				result = result.ParseResult(result, _dCATypeRepository);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}


