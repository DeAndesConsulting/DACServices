﻿using DACServices.Entities;
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
		ItrisSessionRepository itrisSessionRepository;
		private static string USER_SESSION_PROPERTY = "usersession";

		public ItrisBaseRepository(ItrisAuthenticateEntity itrisAuthenticateEntity)
		{
			_authenticateEntity = itrisAuthenticateEntity;
		}

		public async Task<RP> Get(string urlRequest)
		{
			string stringSession = string.Empty;
			try
			{
				//Abro session itris
				stringSession = this.OpenSession();

				//Agrego session al request
				string urlSessionRequest = string.Concat(urlRequest, "&usersession=", stringSession);

				httpClient = new HttpClient();

				//Seteo time out al httpCient porque itris responde muy lento
				httpClient.Timeout = TimeSpan.FromMinutes(30);

				httpResponseMessage = await httpClient.GetAsync(new Uri(urlSessionRequest));
				response = await httpResponseMessage.Content.ReadAsAsync<RP>();

				//tengo que mejorar esto porque el forbiden me tira en varios casos
				//por ejemplo cuando los datos de consulta estan mal. arrojar error igual
				if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
				{
					//this.AuthenticateRepository();
					return await this.Get(urlRequest);
					//throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
				}

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
			finally
			{
				string message = this.CloseSession(stringSession);
			}
		}

		public async Task<RP> Post(string urlRequest, RQ request)
		{
			//Llamo a generar el session string
			string stringSession = string.Empty;
			try
			{
				stringSession = this.OpenSession();

				//Agrego session al request por reflection
				request.GetType().GetProperty(USER_SESSION_PROPERTY).SetValue(request, stringSession, null);

				httpClient = new HttpClient();
				httpResponseMessage = await httpClient.PostAsJsonAsync<RQ>(new Uri(urlRequest), request);
				response = await httpResponseMessage.Content.ReadAsAsync<RP>();

				//Revisar esta validación de error en sessión porque tambien entra cuando el request es erroneo
				if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
				{
					return await this.Post(urlRequest, request);
				}
				return response;
			}
			catch (HttpRequestException reqx)
			{
				throw reqx;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				string message = this.CloseSession(stringSession);
			}
		}

		public Task<RP> Put(string urlRequest, RQ request)
		{
			throw new NotImplementedException();
		}

		public Task<RP> Delete(string urlRequest, RQ request)
		{
			throw new NotImplementedException();
		}

		public string OpenSession()
		{
			LoginItrisRequestEntity loginItrisRequestEntity = new LoginItrisRequestEntity()
			{
				username = _authenticateEntity._username,
				password = _authenticateEntity._password,
				database = _authenticateEntity._database
			};

			itrisSessionRepository = new ItrisSessionRepository(loginItrisRequestEntity);

			return itrisSessionRepository.GetItrisSession(_authenticateEntity.LoginUrl());
		}


		public string CloseSession(string itrisSession)
		{
			LoginItrisRequestEntity loginItrisRequestEntity = new LoginItrisRequestEntity()
			{
				username = _authenticateEntity._username,
				password = _authenticateEntity._password,
				database = _authenticateEntity._database
			};

			itrisSessionRepository = new ItrisSessionRepository(loginItrisRequestEntity);

			string message = itrisSessionRepository.CloseItrisSession(_authenticateEntity.LogOutUrl(), itrisSession);
			return message;
		}
	}
}
