using DACServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories
{
	public abstract class BearerRepository<RQ, RP> : IRepository<RQ, RP>
		where RQ : class, new()
		where RP : class, new()
	{
		private HttpClient httpClient = new HttpClient();
		private HttpResponseMessage httpResponseMessage = null;
		private RP response = new RP();
		private static string AUTHORIZATION_TYPE = "";
		private static string BEARER = "Bearer ";

		public async Task<RP> Get(string urlRequest)
		{
			try
			{
				httpResponseMessage = await httpClient.GetAsync(urlRequest);

				if (httpResponseMessage.IsSuccessStatusCode)
					response = await httpResponseMessage.Content.ReadAsAsync<RP>();
				else
					throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return response;
		}

		public async Task<RP> Post(string urlRequest, RQ request)
		{
			try
			{
				httpResponseMessage = await httpClient.PostAsJsonAsync<RQ>(new Uri(urlRequest), request);

				if (httpResponseMessage.IsSuccessStatusCode)
					response = await httpResponseMessage.Content.ReadAsAsync<RP>();
				else
					throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return response;
		}

		public async Task<RP> Put(string urlRequest, RQ request)
		{
			try
			{
				httpResponseMessage = await httpClient.PutAsJsonAsync<RQ>(urlRequest, request);

				if (httpResponseMessage.IsSuccessStatusCode)
					response = await httpResponseMessage.Content.ReadAsAsync<RP>();
				else
					throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return response;
		}

		public async Task<RP> Delete(string urlRequest, RQ request)
		{
			try
			{
				httpResponseMessage = await httpClient.DeleteAsync(urlRequest);

				if (httpResponseMessage.IsSuccessStatusCode)
					response = await httpResponseMessage.Content.ReadAsAsync<RP>();
				else
					throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return response;
		}

		public string OpenSession()
		{
			throw new NotImplementedException();
		}

		public string CloseSession(string session)
		{
			throw new NotImplementedException();
		}
	}
}
