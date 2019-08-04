using DACServices.Entities;
using DACServices.Entities.Request;
using DACServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories
{
    public abstract class ItrisRepository<RQ, RP> : IRepository<RQ, RP>
        where RQ : class, new()
        where RP : class, new()
    {
        private HttpClient httpClient;
        private RP response = new RP();
        private HttpResponseMessage httpResponseMessage = null;
		private AuthenticateEntity _authenticateEntity;

		public ItrisRepository(AuthenticateEntity authenticateEntity)
		{
			_authenticateEntity = authenticateEntity;
			this.AuthenticateRepository();
		}

		public async Task<RP> Get(string urlRequest)
		{
			try
			{
				//Agrego session al request
				string urlSessionRequest = string.Concat(urlRequest,
					"&usersession=", ItrisSessionRepository.GetInstance().sessionString());

				httpClient = new HttpClient();
				httpResponseMessage = await httpClient.GetAsync(new Uri(urlSessionRequest));
				response = await httpResponseMessage.Content.ReadAsAsync<RP>();

				if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
				{
					this.AuthenticateRepository();
					return await this.Get(urlRequest);
					//throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
				}
			}
			catch (HttpRequestException reqx)
			{
				throw reqx;
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
				//Agrego session al request
				string urlSessionRequest = string.Concat(urlRequest,
					"&usersession=", ItrisSessionRepository.GetInstance().sessionString());

				httpClient = new HttpClient();
				httpResponseMessage = await httpClient.PostAsJsonAsync<RQ>(new Uri(urlSessionRequest), request);
				response = await httpResponseMessage.Content.ReadAsAsync<RP>();

				if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
				{
					this.AuthenticateRepository();
					return await this.Get(urlRequest);
				}
			}
			catch (HttpRequestException reqx)
			{
				throw reqx;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return response;
        }

        public Task<RP> Put(string urlRequest, RQ request)
        {
            throw new NotImplementedException();
        }

        public Task<RP> Delete(string urlRequest, RQ request)
        {
            throw new NotImplementedException();
        }

		public void AuthenticateRepository()
		{
			LoginItrisRequestEntity loginItrisRequestEntity = new LoginItrisRequestEntity()
			{
				username = _authenticateEntity.username,
				password = _authenticateEntity.password,
				database = _authenticateEntity.database
			};

			ItrisSessionRepository.GetInstance()
				.ExecuteGetSession(_authenticateEntity.urlAuthenticate, loginItrisRequestEntity);
		}
	}
}
