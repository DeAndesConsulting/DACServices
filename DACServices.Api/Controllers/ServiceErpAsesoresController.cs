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

		[HttpPost]
		public HttpResponseMessage Synchronize([FromBody]List<ERP_ASESORES> lista)
		{
			log.Info("Ingreso Synchronize");

			HttpResponseMessage response = new HttpResponseMessage();
			ServiceSyncErpAsesoresEntity resultSQLite = null;

			try
			{
				ServiceErpAsesoresBusiness serviceErpAsesoresBusiness = new ServiceErpAsesoresBusiness();

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

			log.Info("Salio Synchronize");
			return response;
		}

	}
}
