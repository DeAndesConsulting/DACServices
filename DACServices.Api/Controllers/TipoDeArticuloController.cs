﻿using DACServices.Business.Vendor;
using DACServices.Entities;
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
    public class TipoDeArticuloController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(TipoDeArticuloController));

		public async Task<HttpResponseMessage> Get()
		{
			log.Info("Ingreso");
			HttpResponseMessage response = new HttpResponseMessage();

			//CLASS
			string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
			string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
			string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_TIPO_ARTICULO"];

			//AUTHENTICATE
			string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
			string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
			string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

			ItrisTipoDeArticuloResponse responseItris;
			try
			{
				ItrisAuthenticateEntity authenticateEntity =
					new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

				ItrisTipoDeArticuloBusiness itrisTipoDeArticuloBusiness = new ItrisTipoDeArticuloBusiness(authenticateEntity);

				log.Info("Ejecuta itrisTipoDeArticuloBusiness.Get():");
				responseItris = await itrisTipoDeArticuloBusiness.Get();
				log.Info("Respuesta itrisTipoDeArticuloBusiness.Get(): " + JsonConvert.SerializeObject(responseItris));

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
