
using Newtonsoft.Json.Linq;

namespace Business_Model
{
	public class Settings
	{
		private InfoModelTokenRequest infomodel = new InfoModelTokenRequest();
		public string InfoModelBaseAddress { get; set; }
		public string TypeDefBaseAddress { get; set; }
		public string InfoObjects { get; set; }
		public string RetrieveObjects { get; set; }
		public string EditType { get; set; }
		public string EditModel { get; set; }
		public string QueryPath { get; set; }
		public string ReferencePath {get; set; }
		public string ReferenceRetrievePath { get; set; }
		public InfoModelTokenRequest infoModelToken
		{
			get { return infomodel; }
			set
			{
				infomodel = value;
			}

		}
	}
	public class InfoModelTokenRequest
	{
		public string TokenURI { get; set; }
		public string GrantType { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string Resource { get; set; }
		public string Scope { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string test { get; set; }
	}
	public class Response
	{
		public bool IsSuccess {get;set;}
		public string StatusCode { get; set; }
		public object ResponseMessage { get; set; }
		public Response(bool status, string statusCode, object data)
		{
			IsSuccess = status;
			StatusCode = statusCode;
			ResponseMessage = data;
		}
	}
	public class ResponseData
	{
		public object ObjectId { get; set; }
		public string dcaType { get; set; }
	}
	
}


