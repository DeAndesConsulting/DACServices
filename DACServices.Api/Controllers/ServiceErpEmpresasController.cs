﻿using DACServices.Business.Service;
using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Service.Entities;
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
    public class ServiceErpEmpresasController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(ServiceErpEmpresasController));

		private string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
		private string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
		private string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_ERP_EMPRESAS"];
		private string ITRIS_USERS = ConfigurationManager.AppSettings["ITRIS_USERS"];
		private string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
		private string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];
		private string SINCRONIZAR_CON_ITRIS = ConfigurationManager.AppSettings["SINCRONIZAR_CON_ITRIS"];

		public HttpResponseMessage Get()
		{
			log.Info("Ingreso");
			string usuarioItris = this.ObtenerUsuarioItris();

			HttpResponseMessage response = new HttpResponseMessage();

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, usuarioItris, ITRIS_PASS, ITRIS_DATABASE);

			ItrisErpEmpresasResponse responseItris = null;
			try
			{
				ItrisErpEmpresasBusiness itrisErpEmpresasBusiness =
							new ItrisErpEmpresasBusiness(authenticateEntity);

				log.Info("Ejecuta itrisErpEmpresasBusiness.Get()");
				responseItris = Task.Run(async () => await itrisErpEmpresasBusiness.Get()).GetAwaiter().GetResult();
				log.Info("Respuesta itrisErpEmpresasBusiness.Get(entity): " + JsonConvert.SerializeObject(responseItris));

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

		public HttpResponseMessage GetList(string id)
		{
			log.Info("Ingreso");
			string usuarioItris = this.ObtenerUsuarioItris();

			HttpResponseMessage response = new HttpResponseMessage();

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, usuarioItris, ITRIS_PASS, ITRIS_DATABASE);

			ItrisErpEmpresasResponse responseItris = null;
			try
			{
				ItrisErpEmpresasBusiness itrisErpEmpresasBusiness =
							new ItrisErpEmpresasBusiness(authenticateEntity);

				ItrisErpEmpresasEntity entity = new ItrisErpEmpresasEntity() { ID = id };

				log.Info("Ejecuta itrisErpEmpresasBusiness.Get(entity): " + JsonConvert.SerializeObject(entity));
				responseItris = Task.Run(async () => await itrisErpEmpresasBusiness.Get(entity)).GetAwaiter().GetResult();
				log.Info("Respuesta itrisErpEmpresasBusiness.Get(entity): " + JsonConvert.SerializeObject(responseItris));

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

		[HttpPost]
		public HttpResponseMessage Synchronize([FromBody]List<ERP_EMPRESAS> lista)
		{
			log.Info("Ingreso");
			string usuarioItris = this.ObtenerUsuarioItris();
			bool sincronizarConItris = Convert.ToBoolean(SINCRONIZAR_CON_ITRIS);

			HttpResponseMessage response = new HttpResponseMessage();

			ItrisAuthenticateEntity authenticateEntity =
				new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, usuarioItris, ITRIS_PASS, ITRIS_DATABASE);

			ServiceSyncErpEmpresasEntity resultDACS = null;
			ServiceSyncErpEmpresasEntity resultSQLite = null;
			try
			{
				ServiceErpEmpresasBusiness serviceErpEmpresasBusiness = new ServiceErpEmpresasBusiness();

				if (sincronizarConItris)
				{
					//Actualizo base de datos local respecto de las modificaciones en la base de itris
					log.Info("Ejecuta serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity)");
					resultDACS = serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity);
					log.Info("Respuesta serviceErpEmpresasBusiness.SynchronizeErpEmpresasDACS(authenticateEntity): " + JsonConvert.SerializeObject(resultDACS));
				}

				//Comparo el input enviado desde SQLite con la base local
				log.Info("Ejecuta serviceErpEmpresasBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));
				resultSQLite = serviceErpEmpresasBusiness.SynchronizeSQLite(lista);
				log.Info("Respuesta serviceErpEmpresasBusiness.SynchronizeSQLite(lista): " + JsonConvert.SerializeObject(lista));

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