using DACServices.Business.Service;
using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Service;
using DACServices.Entities.Vendor.Clases;
using DACServices.Entities.Vendor.Response;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DACServices.Api.Controllers
{
    public class ServiceErpAsesoresController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(ServiceErpAsesoresController));
		private string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
		private string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
		private string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_ASESORES"];
		private string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
		private string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
		private string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

		//public async Task<HttpResponseMessage> Get(int id)
		public HttpResponseMessage Get(int id)
		{
			log.Info("Ingreso");

			HttpResponseMessage response = new HttpResponseMessage();

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

			ItrisErpAsesoresResponse responseItris = null;
			try
			{
				ItrisErpAsesoresBusiness itrisErpAsesoresBusiness =
							new ItrisErpAsesoresBusiness(authenticateEntity);

				ItrisErpAsesoresEntity entity = new ItrisErpAsesoresEntity() { ID = id };

				log.Info("Ejecuta itrisErpAsesoresBusiness.Get(entity): " + JsonConvert.SerializeObject(entity));
				responseItris = Task.Run(async () => await itrisErpAsesoresBusiness.Get(entity)).GetAwaiter().GetResult();
				log.Info("Respuesta itrisErpAsesoresBusiness.Get(entity): " + JsonConvert.SerializeObject(responseItris));

				response = Request.CreateResponse(HttpStatusCode.Created, responseItris.data);
			}
			catch (Exception ex)
			{
				log.Error("Mensaje de Error: " + ex.Message);
				if (ex.InnerException != null)
					log.Error("Inner exception: " + ex.InnerException.Message);

				response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
			}

			log.Info("Salio");
			return response;
		}

		[HttpPost]
		public HttpResponseMessage Synchronize([FromBody]List<ERP_ASESORES> lista)
		{
			log.Info("Ingreso");

			HttpResponseMessage response = new HttpResponseMessage();

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

			ServiceSyncErpAsesoresEntity resultDACS = null;
			ServiceSyncErpAsesoresEntity resultSQLite = null;
			try
			{
				ServiceErpAsesoresBusiness serviceErpAsesoresBusiness = new ServiceErpAsesoresBusiness();

				//Actualizo base de datos local respecto de las modificaciones en la base de itris
				log.Info("Ejecuta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity)");
				resultDACS = serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity);
				log.Info("Respuesta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity): " + JsonConvert.SerializeObject(resultDACS));

				//Comparo el input enviado desde SQLite con la base local
				log.Info("Ejecuta serviceErpAsesoresBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));
				resultSQLite = serviceErpAsesoresBusiness.SynchronizeSQLite(lista);
				log.Info("Respuesta serviceErpAsesoresBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));

				response = Request.CreateResponse(HttpStatusCode.Created, resultSQLite);
			}
			catch (Exception ex)
			{
				log.Error("Mensaje de Error: " + ex.Message);
				if (ex.InnerException != null)
					log.Error("Inner exception: " + ex.InnerException.Message);

				response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
			}

			log.Info("Salio");
			return response;
		}
	}
}
