﻿using DACServices.Business.Service;
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
    public class ErpAsesoresController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(ErpAsesoresController));
		private string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
		private string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
		private string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_ASESORES"];
		private string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
		private string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
		private string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

		public ErpAsesoresController()
		{
			//PARAMETROS CONEXIÓN A ITRIS
			//CLASS
			ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
			ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
			ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_ASESORES"];
			//AUTHENTICATE
			ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
			ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
			ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];
		}

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

		//public async Task<HttpResponseMessage> Synchronize(List<ItrisErpAsesoresEntity> lista)
		[HttpPost]
		public HttpResponseMessage Synchronize([FromBody]List<ERP_ASESORES> lista)
		{
			log.Info("Ingreso");

			HttpResponseMessage response = new HttpResponseMessage();

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

			ServiceSyncErpAsesoresEntity result = null;
			try
			{
				ServiceErpAsesoresBusiness serviceErpAsesoresBusiness = new ServiceErpAsesoresBusiness();

				log.Info("Ejecuta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity)");
				result = serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity);
				log.Info("Respuesta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity): " + JsonConvert.SerializeObject(result));

				response = Request.CreateResponse(HttpStatusCode.Created, result);
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
