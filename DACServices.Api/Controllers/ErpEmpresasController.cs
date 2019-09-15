using DACServices.Business.Vendor;
using DACServices.Entities;
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
    public class ErpEmpresasController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(ErpEmpresasController));

		public HttpResponseMessage Get()
		{
			log.Info("Ingreso");

			HttpResponseMessage response = new HttpResponseMessage();

			//PARAMETROS CONEXIÓN A ITRIS
			//CLASS
			string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
			string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
			string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_EMPRESAS"];
			//AUTHENTICATE
			string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
			string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
			string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

			ItrisErpEmpresasResponse responseItris = null;
			try
			{
				ItrisErpEmpresasBusiness itrisErpEmpresasBusiness =
							new ItrisErpEmpresasBusiness(authenticateEntity);

				log.Info("Ejecuta itrisErpEmpresasBusiness.Get()");
				responseItris = Task.Run(async () => await itrisErpEmpresasBusiness.Get()).GetAwaiter().GetResult();
				log.Info("Respuesta itrisErpEmpresasBusiness.Get(entity): " + JsonConvert.SerializeObject(responseItris));

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

		public HttpResponseMessage GetList(string id)
		{
			log.Info("Ingreso");

			HttpResponseMessage response = new HttpResponseMessage();

			//PARAMETROS CONEXIÓN A ITRIS
			//CLASS
			string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
			string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
			string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_EMPRESAS"];
			//AUTHENTICATE
			string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
			string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
			string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

			ItrisErpEmpresasResponse responseItris = null;
			try
			{
				ItrisErpEmpresasBusiness itrisErpEmpresasBusiness =
							new ItrisErpEmpresasBusiness(authenticateEntity);

				ItrisErpEmpresasEntity entity = new ItrisErpEmpresasEntity() { ID = id };

				log.Info("Ejecuta itrisErpEmpresasBusiness.Get(entity): " + JsonConvert.SerializeObject(entity));
				responseItris = Task.Run(async () => await itrisErpEmpresasBusiness.Get(entity)).GetAwaiter().GetResult();
				log.Info("Respuesta itrisErpEmpresasBusiness.Get(entity): " + JsonConvert.SerializeObject(responseItris));

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
	}
}
