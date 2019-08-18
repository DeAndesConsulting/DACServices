using DACServices.Api.Models;
using DACServices.Business.Service;
using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DACServices.Api.Controllers
{
    public class RelevamientoController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(RelevamientoController));

		public HttpResponseMessage Post([FromBody] ItrisPlanillaEntity model)
		{
			log.Info("- Ingreso -");

			//ItrisRelevamientoEntity entityReturn = null;
			HttpResponseMessage response = new HttpResponseMessage();

			//PARAMETROS CONEXIÓN A ITRIS
			//CLASS
			string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
			string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
			string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_TIPO_COMERCIO"];
			//AUTHENTICATE
			string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
			string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
			string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);


			try
			{
				//Insert bd local
				ServiceRequestBusiness serviceRequestBusiness = new ServiceRequestBusiness(); ;
				tbRequest request = new tbRequest()
				{
					req_fecha_request = DateTime.Now,
					req_fecha_response = null,
					req_body_request = JsonConvert.SerializeObject(model),
					req_estado = false,
					req_imei = model.Imei
				};
				//Creo objeto en base local
				serviceRequestBusiness.Create(request);

				//Inserts itris
				ServiceRelevamientoBusiness serviceRelevamientoBusiness =
							new ServiceRelevamientoBusiness(authenticateEntity);

				serviceRelevamientoBusiness.Post(model);

				//PERSISTENCIA ITRIS OK => ACTUALIZO BASE LOCAL CON OK
				if (model.Relevamiento.ID != 0)
				{
					request.req_fecha_response = DateTime.Now;
					request.req_body_response = JsonConvert.SerializeObject(model);
					request.req_estado = true;
					serviceRequestBusiness.Update(request);

					response = Request.CreateResponse(HttpStatusCode.Created, model);
				}

			}
			catch (Exception ex)
			{
				log.Error("Mensaje de Error: " + ex.Message);
				if (ex.InnerException != null)
					log.Error("Inner exception: " + ex.InnerException.Message);

				response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
			}

			log.Info("Retorna objeto: " + JsonConvert.SerializeObject(model));
			log.Info("- Salio -");

			return response;
		}
	}
}
