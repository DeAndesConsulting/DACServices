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
    public class ServiceArticuloController : ApiController
    {
        private ILog log = LogManager.GetLogger(typeof(ServiceErpAsesoresController));
        private string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
        private string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
        private string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_ARTICULO"];
        private string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
        private string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
        private string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

        [HttpPost]
        public HttpResponseMessage Synchronize([FromBody]List<ARTICULO> lista)
        {
            log.Info("Ingreso");

            HttpResponseMessage response = new HttpResponseMessage();

            ItrisAuthenticateEntity authenticateEntity =
                new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

            ServiceSyncArticuloEntity resultDACS = null;
            ServiceSyncArticuloEntity resultSQLite = null;
            try
            {
                ServiceArticuloBusiness serviceArticuloBusiness = new ServiceArticuloBusiness();

                //Actualizo base de datos local respecto de las modificaciones en la base de itris
                log.Info("Ejecuta serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity)");
                resultDACS = serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity);
                log.Info("Respuesta serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity): " + JsonConvert.SerializeObject(resultDACS));

                //Comparo el input enviado desde SQLite con la base local
                log.Info("Ejecuta serviceArticuloBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));
                resultSQLite = serviceArticuloBusiness.SynchronizeSQLite(lista);
                log.Info("Respuesta serviceArticuloBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));

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
