using Business_Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Repository
{
	public class DCAReferencesRepository : IDCAReferencesRepository
	{
		Uri uri;
		ApiClient client;
		string referencePath = string.Empty;
		string referenceRetrievePath = string.Empty;
		string infoObjectsPath = string.Empty;
		IDCARepository _dCARepository;
		IDCATypeRepository _dCATypeRepository;
		public DCAReferencesRepository(IOptions<Settings> apisettings, IDCARepository dCARepository,IDCATypeRepository dCATypeRepository)
		{
			uri = new Uri(apisettings.Value.InfoModelBaseAddress);
			client = new ApiClient(uri, apisettings.Value.infoModelToken);
			_dCARepository = dCARepository;
			_dCATypeRepository = dCATypeRepository;
			infoObjectsPath = apisettings.Value.RetrieveObjects;
			referencePath = apisettings.Value.ReferencePath;
			referenceRetrievePath = apisettings.Value.ReferenceRetrievePath;
		}
		public async Task<Response> GetReferences(string objectID, string dcaType)
		{
			DCATypeModel dcaModel = _dCATypeRepository.GetDCAType(dcaType);
			if (dcaModel == null)
			{
				throw new Exception(string.Format("DCA type mapping is not found for type:{0}", dcaType));
			}
			string path = String.Format(infoObjectsPath + "/references", objectID, dcaModel.AbilityModel);
			var result = await client.GetAsync(path);
		    result = result.ParseReferenceResult(result, _dCATypeRepository);
			return result;
		}
		public async Task<Response> GetChildObjects(string objectID, string dcaType)
		{
			DCATypeModel dcaModel = _dCATypeRepository.GetDCAType(dcaType);
			if (dcaModel == null)
			{
				throw new Exception(string.Format("DCA type mapping is not found for type:{0}", dcaType));
			}
			string path = String.Format(infoObjectsPath + "/references", objectID, dcaModel.AbilityModel);
			var result = await client.GetAsync(path);
			if (result.IsSuccess == false)
			{
				return result;
			}
			var referencebjects = JObject.Parse(result.ResponseMessage.ToString());
			JArray objs=(JArray) referencebjects["data"];
			IEnumerable<JToken> tokens = objs.AsEnumerable<JToken>();
			var toResult= tokens.Select(o => o.SelectToken("to")).ToList();
			result.ResponseMessage = toResult;
			result = result.ParseChildReferenceResult(result, _dCATypeRepository);
			return result;
		}
		public async Task<Response> GetReferences(string referenceId)
		{
			string path = string.Format(referenceRetrievePath, referenceId);
			var result = await client.GetAsync(path);
			result = result.ParseReferenceIdResult(result, _dCATypeRepository);
			return result;
		}
		public async Task<Response> SendReferences(string parentObjectId, string parentDcaType, dynamic referencemodel)
		{
			try
			{
				dynamic parentObject = null;

				var response = await _dCARepository.GetObjects(parentObjectId, parentDcaType,false);
				if (response.IsSuccess == true)
				{
					parentObject = response.ResponseMessage;
				}
				else
				{
					return response;
				}
				Dictionary<string, string> parent =
				((IEnumerable<KeyValuePair<string, JToken>>)parentObject)
					 .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

				if (parent == null)
					return null;
				else
				{
					var jArray = JArray.Parse(referencemodel.ToString());
					Dictionary<string, object> referenceCollection = new Dictionary<string, object>();

					foreach (JObject content in jArray.Children<JObject>())
					{
						foreach (JProperty prop in content.Properties())
						{
							if(referenceCollection.ContainsKey(prop.Name)==false)
							referenceCollection.Add(prop.Name, prop.Value);
						}
					}
					//Fetch the DCA Type and get its corresponding Ability model and type
					dynamic tobody = referenceCollection["to"];
					Dictionary<string, string> dcatoBody =
						((IEnumerable<KeyValuePair<string, JToken>>)tobody)
							 .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
					
					
					var childObjectId = dcatoBody["objectId"];
					var childModel = dcatoBody["dcaType"];
					response = await _dCARepository.GetObjects(childObjectId, childModel);

					//If child object doesn't exist. Create child model
					if (response.IsSuccess == false)
					{
						var childDeviceModel = new DCADeviceModel()
						{
							model = parent["model"],
							type = parent["type"],
							objectId = childObjectId
						};
						//Create DCA Object if it doesn't exist
						var result = await _dCARepository.SendObjects(childDeviceModel, false);
						if (result.IsSuccess == false)
							return result;
					}
					dcatoBody.Remove("dcaType");
					dcatoBody.Add("model", parent["model"]);
					referenceCollection["to"] = JObject.FromObject(dcatoBody);
					//Create reference object which returns reference Id if sucess
					string path= string.Format(referencePath, parentObjectId, parent["model"]);
					var referenceObject = JObject.FromObject(referenceCollection);
					JArray referenceArray = new JArray(referenceObject);
					var refResult = await client.PostAsync(path, referenceArray);
					return refResult;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<Response> RemoveReferences(string referenceId)
		{
			string path = String.Format(referenceRetrievePath, referenceId);
			var result = await client.DeleteAsync(path);
			// result = result.ParseResult(result, _dCATypeRepository);
			return result;
		}

		public async Task<Response> UpdateReferences(string referenceId, dynamic model)
		{
			referencePath += "/" + referenceId + "/attributes";
			var refResult = await client.PutAsync(referencePath, model);
			return refResult;
		}
		public async Task<Response> DeleteReferences(string referenceId)
		{
			string path = String.Format(referenceRetrievePath, referenceId);
			var refResult = await client.DeleteAsync(path);
			return refResult;
		}
	}
}
