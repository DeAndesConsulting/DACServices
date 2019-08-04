using DACServices.Business;
using DACServices.Entities;
using DACServices.Entities.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace DACServices.Api.Controllers
{
    public class TipoDeComercioController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(TipoDeComercioController));

		public async Task<TipoDeComercioResponse> Get()
		{
			log.Debug("Debug log");
			log.Info("Info log");
			log.Warn("Warn log");
			log.Error("Error log");
			log.Fatal("Fatal log");


			//CLASS
			string SERVER = "iserver.itris.com.ar";
			int PUERTO = 2217;
			string RECURSO = "class";
			string CLASE = "_TIP_COM";

			//AUTHENTICATE
			string USER_NAME = "lrodriguez";
			string PASSWORD = "";
			string DATABASE = "REFRES_POS";
			string URL_AUTHENTICATE = string.Format("http://{0}:{1}/Login", SERVER, PUERTO);

			TipoDeComercioResponse tipoDeComercioResponse = null;
			try
			{
				AuthenticateEntity authenticateEntity = 
					new AuthenticateEntity(USER_NAME, PASSWORD, DATABASE, URL_AUTHENTICATE);

				UrlEntity urlEntity = new UrlEntity(SERVER, PUERTO, RECURSO, CLASE);

				TipoDeComercioBusiness bus = new TipoDeComercioBusiness(authenticateEntity, urlEntity);
				tipoDeComercioResponse = await bus.Get();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return tipoDeComercioResponse;
		}
    }
}
