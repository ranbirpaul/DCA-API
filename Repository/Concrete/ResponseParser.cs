using Business_Model;
using Newtonsoft.Json.Linq;
using Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Concrete
{

	public static class ResponseParser
	{
		public static Response ParseResult(this Response response, dynamic result, IDCATypeRepository dCATypeRepository)
		{
			if (result is Response)
			{

				var parsedResponse = (Response)result;
				if (parsedResponse.IsSuccess)
				{
					var responseMsg = response.ResponseMessage;
					var dcakeyValuePairs =
					   ((IEnumerable<KeyValuePair<string, JToken>>)responseMsg)
							.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
					var model = dcakeyValuePairs["model"];

					var dcaTypeObject = dCATypeRepository.GetDCATypeofModelType(model);
					dcakeyValuePairs.Remove("model");
					dcakeyValuePairs.Remove("type");
					dcakeyValuePairs.Add("dcaType", dcaTypeObject.DCAType);
					var dcaDeviceCollection = new Dictionary<string, dynamic>();
					foreach (var keyvaluepair in dcakeyValuePairs)
					{
						if (keyvaluepair.Key == "properties" || keyvaluepair.Key == "variables")
						{
							dcaDeviceCollection.Add(keyvaluepair.Key, JToken.Parse(dcakeyValuePairs[keyvaluepair.Key]));
						}
						else
							dcaDeviceCollection.Add(keyvaluepair.Key, dcakeyValuePairs[keyvaluepair.Key]);
					}
					response.ResponseMessage = dcaDeviceCollection;//JObject.FromObject(dcakeyValuePairs);
					return response;
				}
			}
			return response;
		}
		public static Response ParseReferenceIdResult(this Response response, dynamic result, IDCATypeRepository dCATypeRepository)
		{
			string fromModel = string.Empty;
			string toModel = string.Empty;
			var parsedResponse = (Response)result;
			if (parsedResponse.IsSuccess)
			{
				var responseMsg = response.ResponseMessage;
				var jObject = JObject.Parse(responseMsg.ToString());
				fromModel = jObject.SelectToken("from.model").ToString();
				toModel = jObject.SelectToken("to.model").ToString();

				var fromdCATypeModel = dCATypeRepository.GetDCATypeofModelType(fromModel);
				var todCATypeModel = dCATypeRepository.GetDCATypeofModelType(toModel);
				JProperty fromdcaType = new JProperty("dcaType", fromdCATypeModel.DCAType);
				var jProperty = jObject.Properties();
				var fromObject = (JObject)jObject.SelectToken("from");
				fromObject.Add(fromdcaType);
				fromObject.Remove("model");
				jObject["from"] = fromObject;

				var toToken = (JObject)jObject.SelectToken("to");
				toToken.Add(fromdcaType);
				toToken.Remove("model");
				jObject["to"] = toToken;
				response.ResponseMessage = jObject;
			}

			return response;

		}
		public static Response ParseChildReferenceResult(this Response response, dynamic result, IDCATypeRepository dCATypeRepository)
		{
			string model = string.Empty;
			var parsedResponse = (Response)result;
			if (parsedResponse.IsSuccess)
			{
				var responseMsg = response.ResponseMessage;
				var jArray = new JArray(responseMsg);
				var jObject2 = new JArray(responseMsg);
				foreach (JObject jObject in jArray)
				{
					model = jObject.SelectToken("model").ToString();
					var dcaTypeModel = dCATypeRepository.GetDCATypeofModelType(model);
					JProperty dcaType = new JProperty("dcaType", dcaTypeModel.DCAType);
					jObject.Add(dcaType);
					jObject.Remove("model");
					jObject.Remove("type");
				}
				response.ResponseMessage = jArray;
			}
			

			return response;

		}
		public static Response ParseReferenceResult(this Response response, dynamic result, IDCATypeRepository dCATypeRepository)
		{
			string oldmodel = string.Empty; string oldtype = string.Empty; DCATypeModel dCATypeModel = null;
			var responses = new List<ResponseData>();

			var parsedResponse = (Response)result;
			if (parsedResponse.IsSuccess)
			{
				var responseMsg = response.ResponseMessage;
				var responseToken = JToken.Parse(responseMsg.ToString());
				var responsedata = responseToken.SelectToken("data");
				foreach (var token in responsedata)
				{
					var newmodel = token.SelectToken("from.model").ToString();
					var objectId = token.SelectToken("from.objectId").ToString();
					var newtype = token.SelectToken("from.type").ToString();
					if (oldmodel != newmodel && oldtype != newtype)
					{
						dCATypeModel = dCATypeRepository.GetDCATypeofModelType(newmodel);
					}
					var responsemodel = new ResponseData() { dcaType = dCATypeModel.DCAType, ObjectId = objectId };
					responses.Add(responsemodel);
					oldmodel = newmodel;
					oldtype = newtype;
				}
			}
			response.ResponseMessage = responses;
			return response;

		}
		public static Response ParseQueryResult(this Response response, dynamic result, IDCATypeRepository dCATypeRepository)
		{
			var responses = new List<object>();
			try
			{
				if (result is Response)
				{

					var parsedResponse = (Response)result;
					if (parsedResponse.IsSuccess)
					{
						var responseMsg = response.ResponseMessage;

						var responseToken = JToken.Parse(responseMsg.ToString());
						var responsedata = responseToken.SelectToken("data");
						if (responsedata.Count() == 0)
						{
							response.IsSuccess = false;
							return response;
						}
						foreach (var token in responsedata)
						{
							var dcakeyValuePairs =
							   ((IEnumerable<KeyValuePair<string, JToken>>)token)
									.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
							if (dcakeyValuePairs.ContainsKey("model") == false)
							{
								response.IsSuccess = false;
								return response;
							}
							var model = dcakeyValuePairs["model"];
							var dcaTypeObject = dCATypeRepository.GetDCATypeofModelType(model);
							dcakeyValuePairs.Remove("model");
							dcakeyValuePairs.Remove("type");
							dcakeyValuePairs.Add("dcaType", dcaTypeObject.DCAType);
							var dcaDeviceCollection = new Dictionary<string, dynamic>();
							foreach (var keyvaluepair in dcakeyValuePairs)
							{
								if (keyvaluepair.Key == "properties" || keyvaluepair.Key == "variables")
								{
									dcaDeviceCollection.Add(keyvaluepair.Key, JToken.Parse(dcakeyValuePairs[keyvaluepair.Key]));
								}
								else
									dcaDeviceCollection.Add(keyvaluepair.Key, dcakeyValuePairs[keyvaluepair.Key]);
							}
							responses.Add(dcaDeviceCollection);
						
							
						}
						response.ResponseMessage = responses;
						return response;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return response;
		}
		public static bool IsValidObject(this string value)
		{
			try
			{
				var json = JContainer.Parse(value);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
