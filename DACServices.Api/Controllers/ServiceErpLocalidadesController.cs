using DACServices.Business.Service;
using DACServices.Entities;
using DACServices.Entities.Service.Entities;
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
    public class ServiceErpLocalidadesController : ApiController
    {
        private ILog log = LogManager.GetLogger(typeof(ServiceErpAsesoresController));
        private string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
        private string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
        private string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_LOCALIDADES"];
		private string ITRIS_USERS = ConfigurationManager.AppSettings["ITRIS_USERS"];
		private string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
        private string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];
		private string SINCRONIZAR_CON_ITRIS = ConfigurationManager.AppSettings["SINCRONIZAR_CON_ITRIS"];

		[HttpPost]
        public HttpResponseMessage Synchronize([FromBody]List<ERP_LOCALIDADES> lista)
        {
            log.Info("Ingreso");
			string usuarioItris = this.ObtenerUsuarioItris();
			bool sincronizarConItris = Convert.ToBoolean(SINCRONIZAR_CON_ITRIS);

			HttpResponseMessage response = new HttpResponseMessage();

            ItrisAuthenticateEntity authenticateEntity =
                new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, usuarioItris, ITRIS_PASS, ITRIS_DATABASE);

            ServiceSyncErpLocalidadesEntity resultDACS = null;
            ServiceSyncErpLocalidadesEntity resultSQLite = null;
            try
            {
                ServiceErpLocalidadesBusiness serviceErpLocalidadesBusiness = new ServiceErpLocalidadesBusiness();

				if (sincronizarConItris)
				{
					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity)");
					resultDACS = serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity);
					log.Info("Respuesta serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity): " + JsonConvert.SerializeObject(resultDACS));
				}

                //Comparo el input enviado desde SQLite con la base local
                log.Info("Ejecuta serviceErpLocalidadesBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));
                resultSQLite = serviceErpLocalidadesBusiness.SynchronizeSQLite(lista);
                log.Info("Respuesta serviceErpLocalidadesBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));

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
