using DACServices.Entities.Vendor.Request;
using DACServices.Entities.Vendor.Response;
using DACServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Vendor
{
	public class ItrisSessionRepository //: ISession<LoginItrisRequestEntity>
	{
		private HttpClient httpClient = new HttpClient();
		private HttpResponseMessage httpResponseMessage;
		private LoginItrisRequestEntity _loginItrisRequestEntity;

		public ItrisSessionRepository(LoginItrisRequestEntity loginItrisRequestEntity)
		{
			_loginItrisRequestEntity = loginItrisRequestEntity;
		}

		public string GetItrisSession(string urlAuthentication)
		{
			return this.CallGetSession(urlAuthentication);
		}

		public string CloseItrisSession(string urlCloseAuthentication, string itrisSession)
		{
			return this.CallCloseSession(urlCloseAuthentication, itrisSession);
		}

		private string CallGetSession(string urlAuthentication)
		{
			string sessionString = string.Empty;
			sessionString = Task.Run(async () => await ExecutePostOpenSession(urlAuthentication)).GetAwaiter().GetResult();
			return sessionString;
		}

		private string CallCloseSession(string urlCloseAuthentication, string itrisSession)
		{
			string messageLogout = string.Empty;
			messageLogout =
				Task.Run(async () =>
					await ExecutePostCloseSession(urlCloseAuthentication, itrisSession)).GetAwaiter().GetResult();
			return messageLogout;
		}

		private async Task<string> ExecutePostOpenSession(string urlAuthentication)
		{
			string sessionString = string.Empty;
			try
			{
				httpClient.Timeout = TimeSpan.FromMinutes(30);
				httpResponseMessage =
					await httpClient.PostAsJsonAsync<LoginItrisRequestEntity>(new Uri(urlAuthentication), _loginItrisRequestEntity);

				if (httpResponseMessage.IsSuccessStatusCode)
				{
					var response = await httpResponseMessage.Content.ReadAsAsync<LoginItrisResponseEntity>();
					sessionString = response.usersession;
				}
				else
					throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());

				return sessionString;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private async Task<string> ExecutePostCloseSession(string urlAuthentication, string stringSession)
		{
			string messageLogout = string.Empty;
			try
			{
				LoginItrisRequestEntity request = new LoginItrisRequestEntity() { usersession = stringSession };
				httpClient.Timeout = TimeSpan.FromMinutes(30);

				httpResponseMessage =
					await httpClient.PostAsJsonAsync<LoginItrisRequestEntity>(new Uri(urlAuthentication), request);

				if (httpResponseMessage.IsSuccessStatusCode)
				{
					var response = await httpResponseMessage.Content.ReadAsAsync<LoginItrisResponseEntity>();
					messageLogout = response.message;
				}
				else
					throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());

				return messageLogout;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
