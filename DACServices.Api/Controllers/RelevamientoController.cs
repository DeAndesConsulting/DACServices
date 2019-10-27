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
		//PARAMETROS CONEXIÓN A ITRIS
		private string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
		private string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
		private string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_TIPO_COMERCIO"];
		//AUTHENTICATE
		private string ITRIS_USERS = ConfigurationManager.AppSettings["ITRIS_USERS"];
		private string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
		private string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

		public HttpResponseMessage Post([FromBody] ItrisPlanillaEntity model)
		{
			log.Info("- Ingreso -");
			HttpResponseMessage response = new HttpResponseMessage();

			try
			{
				ServiceRequestBusiness serviceRequestBusiness = new ServiceRequestBusiness();
				tbRequest request = new tbRequest();

				//Valido request en DACS
				Func<tbRequest, bool> predicado = p => p.req_codigo == model.CodigoRequest;
				request = ((List<tbRequest>)serviceRequestBusiness.Read(predicado)).FirstOrDefault();

				if (request != null)
				{
					if (request.req_body_response != null)
					{
						model = JsonConvert.DeserializeObject<ItrisPlanillaEntity>(request.req_body_response);
						log.Info("Retorno el modelo almacenado en DB_DACS: " + JsonConvert.SerializeObject(model));
						return Request.CreateResponse(HttpStatusCode.Created, model);
					}
					else
					{
						//Envio nuevamente el request a itris si no se proceso
						model = JsonConvert.DeserializeObject<ItrisPlanillaEntity>(request.req_body_request);
						this.PostItris(model);
					}
				}
				else
				{
					//Insert bd local
					request = new tbRequest()
					{
						req_fecha_request = DateTime.Now,
						req_fecha_response = null,
						req_body_request = JsonConvert.SerializeObject(model),
						req_estado = false,
						req_codigo = model.CodigoRequest
					};
					//Creo objeto en base local
					serviceRequestBusiness.Create(request);

					//Inserts itris
					this.PostItris(model);
				}

				//PERSISTENCIA ITRIS OK => ACTUALIZO BASE LOCAL CON OK
				if (model.Relevamiento.ID != 0)
				{
					request.req_fecha_response = DateTime.Now;
					request.req_body_response = JsonConvert.SerializeObject(model);
					request.req_estado = true;
					serviceRequestBusiness.Update(request);

					response = Request.CreateResponse(HttpStatusCode.Created, model);
				}
				else
				{
					response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Itris no inserto registros.");
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

		private void PostItris(ItrisPlanillaEntity model)
		{
			log.Info("- Ingreso -");
			string usuarioItris = this.ObtenerUsuarioItris();

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, usuarioItris, ITRIS_PASS, ITRIS_DATABASE);

			try
			{
				//Inserts itris
				ServiceRelevamientoBusiness serviceRelevamientoBusiness =
							new ServiceRelevamientoBusiness(authenticateEntity);

				log.Info("Ejecuta serviceRelevamientoBusiness.Post(model): " + JsonConvert.SerializeObject(model));
				serviceRelevamientoBusiness.Post(model);
				log.Info("Respuesta serviceRelevamientoBusiness.Post(model): " + JsonConvert.SerializeObject(model));
			}
			catch (Exception ex)
			{
				log.Error("Mensaje de Error: " + ex.Message);
				if (ex.InnerException != null)
					log.Error("Inner exception: " + ex.InnerException.Message);
				throw ex;
			}
			log.Info("- Salio -");
		}

		private string ObtenerUsuarioItris()
		{
			log.Info("Ingreso");
			string usuarioItris = string.Empty;
			try
			{
				string[] itrisUsers = ITRIS_USERS.Split('|');
				Random random = new Random();

				log.Info("Calcula usuario random");
				int posicionUsuarioItris = random.Next(itrisUsers.Count());

				usuarioItris = itrisUsers[posicionUsuarioItris];
				log.Info("Retorna usuario random: " + usuarioItris);
			}
			catch (Exception ex)
			{
				log.Error("Mensaje de Error: " + ex.Message);
				if (ex.InnerException != null)
					log.Error("Inner exception: " + ex.InnerException.Message);
			}
			log.Info("Salio");
			return usuarioItris;
		}

	}
}
