using Business_Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Concrete
{
	
	public partial class ApiClient
	{

		private readonly HttpClient _httpClient;
		private Uri BaseEndpoint { get; set; }

		private readonly HttpClient _tokenHttpClient;
		private Uri InfoModelBaseEndpoint { get; set; }
		private InfoModelTokenRequest TokenRequest { get; set; }
		private string Token { get; set; }
		public ApiClient(Uri baseEndpoint, InfoModelTokenRequest tokenRequest)
		{
			if (baseEndpoint == null)
			{
				throw new ArgumentNullException("baseEndpoint");
			}
			BaseEndpoint = baseEndpoint;
			TokenRequest = tokenRequest;
			if (_httpClient == null)
			{
				_httpClient = new HttpClient() { BaseAddress = baseEndpoint };
			
			}
			BaseEndpoint = InfoModelBaseEndpoint;
			if (_tokenHttpClient == null)
			{
				_tokenHttpClient = new HttpClient() { BaseAddress = new Uri(tokenRequest.TokenURI) };
			}
			
		}

		/// <summary>  
		/// Common method for making GET calls  
		/// </summary>  
		public async Task<Response> GetAsync(string requestUrl)
		{
			
			try
			{
				//if (Token == null)
				//{
				//	Token = await GetToken(TokenRequest.TokenURI);
				//	addHeaders();
				//}
				var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
				var data = await response.Content.ReadAsStringAsync();
				dynamic json= JsonConvert.DeserializeObject<dynamic>(data);
				return new Response(response.IsSuccessStatusCode,response.StatusCode.ToString(), json);
				
				
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		public async Task<string> GetToken(string requestUrl)
		{
			var contentCollection = new Dictionary<string, string>();
			contentCollection.Add("grant_type", TokenRequest.GrantType);
			contentCollection.Add("client_id", TokenRequest.ClientId);
			contentCollection.Add("client_secret", TokenRequest.ClientSecret);
			contentCollection.Add("resource", TokenRequest.Resource);
			contentCollection.Add("scope", TokenRequest.Scope);
			contentCollection.Add("username", TokenRequest.username);
			contentCollection.Add("password", TokenRequest.password);
			var req = new HttpRequestMessage(HttpMethod.Post, requestUrl) { Content = new FormUrlEncodedContent(contentCollection) };
			var bearerResult = await _tokenHttpClient.SendAsync(req);
			var bearerData = await bearerResult.Content.ReadAsStringAsync();
			var bearerToken = JObject.Parse(bearerData)["access_token"].ToString();
			if (bearerResult.IsSuccessStatusCode)
			{
				bearerResult.EnsureSuccessStatusCode();
			}
			return bearerToken;
		}

			/// <summary>  
			/// Common method for making POST calls  
			/// </summary>  
			public async Task<Response> PostAsync(string requestUrl, dynamic content)
		{
			try
			{
				//if (Token == null)
				//{
				//	Token = await GetToken(TokenRequest.TokenURI);
				//	addHeaders();
				//}
				var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<dynamic>(content));
				
				var data = await response.Content.ReadAsStringAsync();
				dynamic json = JsonConvert.DeserializeObject<dynamic>(data);
				return new Response(response.IsSuccessStatusCode, response.StatusCode.ToString(), json);

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<Response> PutAsync(string requestUrl, dynamic content)
		{
			try
			{
			//{
			//	if (Token == null)
			//		Token = await GetToken(TokenRequest.TokenURI);
			//	addHeaders();
				var response = await _httpClient.PutAsync(requestUrl.ToString(), CreateHttpContent<dynamic>(content));
				var data = await response.Content.ReadAsStringAsync();
				dynamic json = JsonConvert.DeserializeObject<dynamic>(data);
				return new Response(response.IsSuccessStatusCode, response.StatusCode.ToString(), json);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<Response> DeleteAsync(string requestUrl)
		{
			try
			{
				//if (Token == null)
				//{
				//	Token = await GetToken(TokenRequest.TokenURI);
				//	addHeaders();
				//}
				var response = await _httpClient.DeleteAsync(requestUrl.ToString());//CreateHttpContent<dynamic>(content));
				var data = await response.Content.ReadAsStringAsync();
				dynamic json = JsonConvert.DeserializeObject<dynamic>(data);
				return new Response(response.IsSuccessStatusCode, response.StatusCode.ToString(), json);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private Uri CreateRequestUri(string relativePath, string queryString = "")
		{
			var endpoint = new Uri(BaseEndpoint, relativePath);
			var uriBuilder = new UriBuilder(endpoint);
			uriBuilder.Query = queryString;
			return uriBuilder.Uri;
		}

		private HttpContent CreateHttpContent<T>(T content)
		{
			var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
			var strcontent= new StringContent(json, Encoding.UTF8, "application/json");
			return strcontent;
		}

		private static JsonSerializerSettings MicrosoftDateFormatSettings
		{
			get
			{
				return new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
				};
			}
		}

		private void addHeaders()
		{
			_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
		}
	}
	
}
