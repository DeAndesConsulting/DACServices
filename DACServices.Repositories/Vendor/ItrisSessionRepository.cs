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
	public class ItrisSessionRepository : ISession<LoginItrisRequestEntity>
	{
		private HttpClient httpClient = new HttpClient();
		private HttpResponseMessage httpResponseMessage;
		private string _sessionString;
		private string _urlAuthentication;
		private LoginItrisRequestEntity _loginItrisRequest;

		private static ItrisSessionRepository _instance;
		protected ItrisSessionRepository() { }
		public static ItrisSessionRepository GetInstance()
		{
			if (_instance == null)
				_instance = new ItrisSessionRepository();
			return _instance;
		}

		public string sessionString()
		{
			return _sessionString;
		}

		/// <summary>
		/// Se esta forzando el syncronismo únicamente para obtener el id de sessión y evitar errores en los
		/// request. Estem metodo tiene una validación interna y se ejecuta solo en el caso de que 
		/// sessionString sea null or empty.
		/// </summary>
		/// <param name="urlAuthentication">Url para obtener el id de sesión.</param>
		/// <param name="request">Parametros requeridos para obtener la sesión. Ej. Usuario, Pass, etc.</param>
		/// <returns>Id de sesión ya sea "Bearer xxxxx" ó "ASD123ASD".</returns>
		public void ExecuteGetSession(string urlAuthentication, LoginItrisRequestEntity request)
		{
			if (string.IsNullOrEmpty(_urlAuthentication) && _loginItrisRequest == null)
			{
				_urlAuthentication = urlAuthentication;
				_loginItrisRequest = request;
			}

			if (string.IsNullOrEmpty(_sessionString))
				_sessionString =
					Task.Run(async () => await GetSession(urlAuthentication, request)).GetAwaiter().GetResult();
		}

		public void UpdateSession()
		{
			if (_loginItrisRequest != null && _urlAuthentication != null)
				this.ExecuteGetSession(_urlAuthentication, _loginItrisRequest);
			else
				throw new Exception("No se puede actualizar la sesión _urlAuthentication y _loginItrisRequest son NULOS.");
		}

		private async Task<string> GetSession(string urlAuthentication, LoginItrisRequestEntity request)
		{
			try
			{
				httpResponseMessage =
					await httpClient.PostAsJsonAsync<LoginItrisRequestEntity>(new Uri(urlAuthentication), request);

				if (httpResponseMessage.IsSuccessStatusCode)
				{
					var response = await httpResponseMessage.Content.ReadAsAsync<LoginItrisResponseEntity>();
					_sessionString = response.usersession;
				}
				else
					throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return _sessionString;
		}
	}
}
