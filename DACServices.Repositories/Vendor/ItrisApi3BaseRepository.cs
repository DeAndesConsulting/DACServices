using DACServices.Entities;
using DACServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Vendor
{
	public abstract class ItrisApi3BaseRepository<RQ, RP> : IRepository<RQ, RP>
		where RQ : class, new()
		where RP : class, new()
	{
		private HttpClient httpClient;
		private RP response = new RP();
		private HttpResponseMessage httpResponseMessage = null;
		private ItrisAuthenticateEntity _authenticateEntity;

		public ItrisApi3BaseRepository(ItrisAuthenticateEntity itrisAuthenticateEntity)
		{
			_authenticateEntity = itrisAuthenticateEntity;
		}

		public string CloseSession(string session)
		{
			throw new NotImplementedException();
		}

		public Task<RP> Delete(string urlRequest, RQ request)
		{
			throw new NotImplementedException();
		}

		public Task<RP> Get(string urlRequest)
		{
			throw new NotImplementedException();
		}

		public async Task<RP> Get(string urlRequest, string bearerToken)
		{
			try
			{
				string token = string.Format("Bearer {0}", bearerToken);

				httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("Authorization", token);

				//Seteo time out al httpCient porque itris responde muy lento
				httpClient.Timeout = TimeSpan.FromMinutes(30);

				httpResponseMessage = await httpClient.GetAsync(new Uri(urlRequest));
				response = await httpResponseMessage.Content.ReadAsAsync<RP>();

				return response;
			}
			catch (HttpRequestException reqx)
			{
				throw reqx;
			}
			catch (TaskCanceledException tex)
			{
				if (tex.CancellationToken.IsCancellationRequested)
				{
					throw new Exception("DACS: Cancelation token was true");
				}
				throw tex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string OpenSession()
		{
			throw new NotImplementedException();
		}

		public Task<RP> Post(string urlRequest, RQ request)
		{
			throw new NotImplementedException();
		}

		public Task<RP> Put(string urlRequest, RQ request)
		{
			throw new NotImplementedException();
		}
	}
}
