using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Vendor
{
	public abstract class ItrisBaseBusiness<REPOSITORY, RESPONSE> 
		where REPOSITORY: class, new()
		where RESPONSE: class, new()
	{
		private REPOSITORY repo;
		private RESPONSE response;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;
		private string _sessionString = string.Empty;

		public ItrisBaseBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			repo = Activator.CreateInstance(typeof(REPOSITORY), new object[] { authenticateEntity }) as REPOSITORY;
			//repo = new REPOSITORY(authenticateEntity);
		}

		public async Task<RESPONSE> Get()
		{
			try
			{
				//Ejecuto el metodo que obtiene la url a travez de generics
				//var method = typeof(REPOSITORY).GetMethod("GetUrl");
				//var repositoryMethod = method.MakeGenericMethod();
				//string url = repositoryMethod.Invoke(null, null).ToString();

				var repoMethod = typeof(REPOSITORY).GetMethod("Get");
				dynamic repositoryMethod = repoMethod.MakeGenericMethod();
				//repositoryMethod.Invoke(null, null);

				response =
					await repositoryMethod.Invoke(null, new object[] { itrisAuthenticateEntity.GetUrl() });
				
				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		private string OpenSession()
		{
			//return _tipoDeArticuloRepository.OpenSession();
			return null;
		}

		public string CloseSession(string session)
		{
			return null;
		}

	}
}
