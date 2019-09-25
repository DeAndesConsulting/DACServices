using DACServices.Entities;
using DACServices.Entities.Vendor.Request;
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
	public abstract class ItrisBaseRepository<RQ, RP> : IRepository<RQ, RP>
		where RQ : class, new()
		where RP : class, new()
	{
		private HttpClient httpClient;
		private RP response = new RP();
		private HttpResponseMessage httpResponseMessage = null;
		private ItrisAuthenticateEntity _authenticateEntity;
		private static string USER_SESSION_PROPERTY = "usersession";

		public ItrisBaseRepository(ItrisAuthenticateEntity authenticateEntity)
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

				//Seteo time out al httpCient porque itris responde muy lento
				httpClient.Timeout = TimeSpan.FromMinutes(30);

				httpResponseMessage = await httpClient.GetAsync(new Uri(urlSessionRequest));
				response = await httpResponseMessage.Content.ReadAsAsync<RP>();

				//tengo que mejorar esto porque el forbiden me tira en varios casos
				//por ejemplo cuando los datos de consulta estan mal. arrojar error igual
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
			catch(TaskCanceledException tex)
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
			return response;
		}

		public async Task<RP> Post(string urlRequest, RQ request)
		{
			try
			{
				//Agrego session al request por reflection
				request.GetType().GetProperty(USER_SESSION_PROPERTY).SetValue(
					request, ItrisSessionRepository.GetInstance().sessionString(), null) ;

				httpClient = new HttpClient();
				httpResponseMessage = await httpClient.PostAsJsonAsync<RQ>(new Uri(urlRequest), request);
				response = await httpResponseMessage.Content.ReadAsAsync<RP>();

				//Revisar esta validación de error en sessión porque tambien entra cuando el request es erroneo
				if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
				{
					this.AuthenticateRepository();
					return await this.Post(urlRequest, request);
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
				username = _authenticateEntity._username,
				password = _authenticateEntity._password,
				database = _authenticateEntity._database
			};

			ItrisSessionRepository.GetInstance()
				.ExecuteGetSession(_authenticateEntity.LoginUrl(), loginItrisRequestEntity);
		}
	}
}
