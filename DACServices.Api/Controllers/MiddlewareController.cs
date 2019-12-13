using DACServices.Api.Models;
using DACServices.Business.Service;
using DACServices.Entities;
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
	//[Authorize]
    public class MiddlewareController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(MiddlewareController));

		private string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
		private string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
		private string ITRIS_CLASE_EMPRESAS = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_EMPRESAS"];
		private string ITRIS_CLASE_ASESORES = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_ASESORES"];
		private string LAST_UPDATE_ASESORES = ConfigurationManager.AppSettings["LAST_SYNC_ASESORES"];
		private string ITRIS_CLASE_LOCALIDADES = ConfigurationManager.AppSettings["ITRIS_CLASE_LOCALIDADES"];
		private string ITRIS_CLASE_ARTICULOS = ConfigurationManager.AppSettings["ITRIS_CLASE_ARTICULO"];
		private string ITRIS_USERS = ConfigurationManager.AppSettings["ITRIS_USERS"];
		private string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
		private string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

		[HttpPost]
		public HttpResponseMessage Synchronize([FromBody]TablasItris tablasItris)
		{
			log.Info("Ingreso Synchronize");
			string usuarioItris = this.ObtenerUsuarioItris();
			string[] arregloTablasItris = tablasItris.NombreTablas.Select(x => x.ToUpper()).ToArray();
			MiddlewareModel model = new MiddlewareModel();
			HttpResponseMessage response = new HttpResponseMessage();
			ItrisAuthenticateEntity authenticateEntity = null;

			ServiceConfigurationBusiness serviceConfigurationBusiness = new ServiceConfigurationBusiness();
			string lastUpdate = string.Empty;

			try
			{
				if (arregloTablasItris.Contains(ITRIS_CLASE_EMPRESAS))
				{
					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE_EMPRESAS,
						usuarioItris, ITRIS_PASS, ITRIS_DATABASE);
					ServiceErpEmpresasBusiness serviceErpEmpresasBusiness = new ServiceErpEmpresasBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity)");
					model.resultDACSEmpresas = serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity);
					log.Info("Respuesta serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSEmpresas));
				}

				if (arregloTablasItris.Contains(ITRIS_CLASE_ASESORES))
				{
					//Obtengo fecha ultima actualizacion de la tabla de configuraciones
					Func<tbConfiguration, bool> predicado = x => x.con_code == LAST_UPDATE_ASESORES;
					var listRead = (List<tbConfiguration>)serviceConfigurationBusiness.Read(predicado);
					var conf = listRead.FirstOrDefault();

					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE_ASESORES,
						usuarioItris, ITRIS_PASS, ITRIS_DATABASE);
					ServiceErpAsesoresBusiness serviceErpAsesoresBusiness = new ServiceErpAsesoresBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity)");
					model.resultDACSAsesores = serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity, conf.con_value);
					log.Info("Respuesta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSAsesores));

					//Actualizo la fecha de ultima actualizacion con del dia que es la ultima actualizacion
					conf.con_value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
					serviceConfigurationBusiness.Update(conf);
				}

				if (arregloTablasItris.Contains(ITRIS_CLASE_LOCALIDADES))
				{
					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE_LOCALIDADES,
						usuarioItris, ITRIS_PASS, ITRIS_DATABASE);
					ServiceErpLocalidadesBusiness serviceErpLocalidadesBusiness = new ServiceErpLocalidadesBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity)");
					model.resultDACSLocalidades = serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity);
					log.Info("Respuesta serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSLocalidades));
				}

				if (arregloTablasItris.Contains(ITRIS_CLASE_ARTICULOS))
				{
					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE_ARTICULOS,
						usuarioItris, ITRIS_PASS, ITRIS_DATABASE);
					ServiceArticuloBusiness serviceArticuloBusiness = new ServiceArticuloBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity)");
					model.resultDACSArticulos = serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity);
					log.Info("Respuesta serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSArticulos));
				}

				response = Request.CreateResponse(HttpStatusCode.Created, model);
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

	public class TablasItris
	{
		public string[] NombreTablas { get; set; }
	}
}
