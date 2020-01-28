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
		private string ITRIS_PUERTO_API3 = ConfigurationManager.AppSettings["ITRIS_PUERTO_API3"];
		private string ITRIS_API3_APP = ConfigurationManager.AppSettings["ITRIS_API3_APP"];
		private string ITRIS_API3_CONFIG = ConfigurationManager.AppSettings["ITRIS_API3_CONFIG"];
		private string ITRIS_API3_USER = ConfigurationManager.AppSettings["ITRIS_API3_USER"];
		private string ITRIS_API3_PASS = ConfigurationManager.AppSettings["ITRIS_API3_PASS"];

		private string ITRIS_CLASE_EMPRESAS = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_EMPRESAS"];
		private string LAST_SYNC_EMPRESAS = ConfigurationManager.AppSettings["LAST_SYNC_EMPRESAS"];
		private string ITRIS_CLASE_ASESORES = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_ASESORES"];
		private string LAST_SYNC_ASESORES = ConfigurationManager.AppSettings["LAST_SYNC_ASESORES"];
		private string ITRIS_CLASE_LOCALIDADES = ConfigurationManager.AppSettings["ITRIS_CLASE_LOCALIDADES"];
		private string LAST_SYNC_LOCALIDADES = ConfigurationManager.AppSettings["LAST_SYNC_LOCALIDADES"];
		private string ITRIS_CLASE_ARTICULOS = ConfigurationManager.AppSettings["ITRIS_CLASE_ARTICULO"];
		private string LAST_SYNC_ARTICULO = ConfigurationManager.AppSettings["LAST_SYNC_ARTICULO"];
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
					//Obtengo fecha ultima actualizacion de la tabla de configuraciones
					Func<tbConfiguration, bool> predicado = x => x.con_code == LAST_SYNC_EMPRESAS;
					var listRead = (List<tbConfiguration>)serviceConfigurationBusiness.Read(predicado);
					var conf = listRead.FirstOrDefault();

					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO_API3, ITRIS_CLASE_EMPRESAS,
						ITRIS_API3_APP, ITRIS_API3_CONFIG, ITRIS_API3_USER, ITRIS_API3_PASS);
					ServiceErpEmpresasBusiness serviceErpEmpresasBusiness = new ServiceErpEmpresasBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity)");
					model.resultDACSEmpresas = 
						serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity, conf.con_value);
					log.Info("Respuesta serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSEmpresas));

					//Actualizo la fecha de ultima actualizacion con del dia que es la ultima actualizacion
					conf.con_value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					serviceConfigurationBusiness.Update(conf);
				}

				if (arregloTablasItris.Contains(ITRIS_CLASE_ASESORES))
				{
					//Obtengo fecha ultima actualizacion de la tabla de configuraciones
					Func<tbConfiguration, bool> predicado = x => x.con_code == LAST_SYNC_ASESORES;
					var listRead = (List<tbConfiguration>)serviceConfigurationBusiness.Read(predicado);
					var conf = listRead.FirstOrDefault();

					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO_API3, ITRIS_CLASE_ASESORES,
						ITRIS_API3_APP, ITRIS_API3_CONFIG, ITRIS_API3_USER, ITRIS_API3_PASS);
					ServiceErpAsesoresBusiness serviceErpAsesoresBusiness = new ServiceErpAsesoresBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity)");
					model.resultDACSAsesores = serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity, conf.con_value);
					log.Info("Respuesta serviceErpAsesoresBusiness.SynchronizeErpAsesoresDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSAsesores));

					//Actualizo la fecha de ultima actualizacion con del dia que es la ultima actualizacion
					conf.con_value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					serviceConfigurationBusiness.Update(conf);
				}

				if (arregloTablasItris.Contains(ITRIS_CLASE_LOCALIDADES))
				{
					//Obtengo fecha ultima actualizacion de la tabla de configuraciones
					Func<tbConfiguration, bool> predicado = x => x.con_code == LAST_SYNC_LOCALIDADES;
					var listRead = (List<tbConfiguration>)serviceConfigurationBusiness.Read(predicado);
					var conf = listRead.FirstOrDefault();

					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO_API3, ITRIS_CLASE_LOCALIDADES,
						ITRIS_API3_APP, ITRIS_API3_CONFIG, ITRIS_API3_USER, ITRIS_API3_PASS);
					ServiceErpLocalidadesBusiness serviceErpLocalidadesBusiness = new ServiceErpLocalidadesBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity)");
					model.resultDACSLocalidades = serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity, conf.con_value);
					log.Info("Respuesta serviceErpLocalidadesBusiness.SynchronizeErpLocalidadesDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSLocalidades));

					//Actualizo la fecha de ultima actualizacion con del dia que es la ultima actualizacion
					conf.con_value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					serviceConfigurationBusiness.Update(conf);
				}

				if (arregloTablasItris.Contains(ITRIS_CLASE_ARTICULOS))
				{
					//Obtengo fecha ultima actualizacion de la tabla de configuraciones
					Func<tbConfiguration, bool> predicado = x => x.con_code == LAST_SYNC_ARTICULO;
					var listRead = (List<tbConfiguration>)serviceConfigurationBusiness.Read(predicado);
					var conf = listRead.FirstOrDefault();

					authenticateEntity = new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO_API3, ITRIS_CLASE_ARTICULOS,
						ITRIS_API3_APP, ITRIS_API3_CONFIG, ITRIS_API3_USER, ITRIS_API3_PASS);
					ServiceArticuloBusiness serviceArticuloBusiness = new ServiceArticuloBusiness();

					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity)");
					model.resultDACSArticulos = serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity, conf.con_value);
					log.Info("Respuesta serviceArticuloBusiness.SynchronizeArticuloDACS(authenticateEntity): " + JsonConvert.SerializeObject(model.resultDACSArticulos));

					//Actualizo la fecha de ultima actualizacion con del dia que es la ultima actualizacion
					conf.con_value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					serviceConfigurationBusiness.Update(conf);
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
