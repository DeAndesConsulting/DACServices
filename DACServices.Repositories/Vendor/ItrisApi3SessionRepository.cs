using DACServices.Entities.Vendor.Request;
using DACServices.Entities.Vendor.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Vendor
{
	public class ItrisApi3SessionRepository
	{
		private HttpClient httpClient = new HttpClient();
		private HttpResponseMessage httpResponseMessage;
		private LoginItrisRequestEntity _loginItrisRequestEntity;

		public ItrisApi3SessionRepository(LoginItrisRequestEntity loginItrisRequestEntity)
		{
			_loginItrisRequestEntity = loginItrisRequestEntity;
		}

		public async Task<string> ExecutePostAuthenticate(string urlAuthentication)
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
					sessionString = response.access_token;
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

	}
}
