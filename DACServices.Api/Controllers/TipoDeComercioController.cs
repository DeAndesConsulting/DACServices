﻿using DACServices.Api.Models;
using DACServices.Business;
using DACServices.Business.Service;
using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using DACServices.Entities.Vendor.Request;
using DACServices.Entities.Vendor.Response;
using log4net;
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
    public class TipoDeComercioController : ApiController
    {
		private ILog log = LogManager.GetLogger(typeof(TipoDeComercioController));

		public async Task<List<ItrisTipoDeComercioEntity>> Get()
		{
			//log.Debug("Debug log");
			//log.Info("Info log");
			//log.Warn("Warn log");
			//log.Error("Error log");
			//log.Fatal("Fatal log");

			//CLASS
			string ITRIS_SERVER = ConfigurationManager.AppSettings["ITRIS_SERVER"];
			string ITRIS_PUERTO = ConfigurationManager.AppSettings["ITRIS_PUERTO"];
			string ITRIS_CLASE = ConfigurationManager.AppSettings["ITRIS_CLASE_TIPO_COMERCIO"];

			//AUTHENTICATE
			string ITRIS_USER = ConfigurationManager.AppSettings["ITRIS_USER"];
			string ITRIS_PASS = ConfigurationManager.AppSettings["ITRIS_PASS"];
			string ITRIS_DATABASE = ConfigurationManager.AppSettings["ITRIS_DATABASE"];

			TipoDeComercioResponse tipoDeComercioResponse = null;
			ItrisComercioResponse itrisComercioResponse = null;
			try
			{
				ItrisAuthenticateEntity authenticateEntity = 
					new ItrisAuthenticateEntity(ITRIS_SERVER, ITRIS_PUERTO, ITRIS_CLASE, ITRIS_USER, ITRIS_PASS, ITRIS_DATABASE);

				#region ----------- POST COMERCIO (OK)-------------
				ItrisComercioEntity itrisComercioEntity = new ItrisComercioEntity()
				{
					FK_TIP_COM = 1,
					NOMBRE = "Quique",
					CALLE = "Nogoya",
					NUMERO = "1023",
					LOCALIDAD = "CABA",
					PROVINCIA = "Buenos aires",
					LATITUD = "555555.666666",
					LONGITUD = "777777.8888888"
				};

				List<ItrisComercioEntity> listaComercio = new List<ItrisComercioEntity>();
				listaComercio.Add(itrisComercioEntity);

				ItrisComercioRequest itrisComercioRequest = new ItrisComercioRequest()
				{
					@class = "_COMERCIO",
					data = listaComercio
				};

				ItrisComercioBusiness itrisComercioBusiness = new ItrisComercioBusiness(authenticateEntity);
				//itrisComercioResponse = await itrisComercioBusiness.Post(itrisComercioRequest);
				#endregion

				#region ----------- POST RELEVAMIENTO (OK)-------------
				ItrisRelevamientoEntity itrisRelevamientoEntity = new ItrisRelevamientoEntity()
				{
					FK_ERP_EMPRESAS = "1",
					FK_ERP_ASESORES = 1,
					FECHA = "12/12/2012",
					CODIGO = "ASD123ASD"
				};
				List<ItrisRelevamientoEntity> listaRelevamiento = new List<ItrisRelevamientoEntity>();
				listaRelevamiento.Add(itrisRelevamientoEntity);

				ItrisRelevamientoRequest itrisRelevamientoRequest = new ItrisRelevamientoRequest()
				{
					@class = "_RELEVAMIENTO",
					data = listaRelevamiento
				};

				ItrisRelevamientoBusiness itrisRelevamientoBusiness = new ItrisRelevamientoBusiness(authenticateEntity);
				//var itrisRelevamientoResponse = await itrisRelevamientoBusiness.Post(itrisRelevamientoEntity);

				#endregion

				#region ----------------- POST RELEVAMIENTO-ARTICULO -------------------
				ItrisRelevamientoArticuloEntity itrisRelevamientoArticuloEntity = new ItrisRelevamientoArticuloEntity()
				{
					FK_RELEVAMIENTO = 1,
					FK_COMERCIO = 1,
					FK_ARTICULOS = 1,
					EXISTE = true,
					PRECIO = 1.4
				};

				List<ItrisRelevamientoArticuloEntity> listaRelevamientoArticulo = 
					new List<ItrisRelevamientoArticuloEntity>();
				listaRelevamientoArticulo.Add(itrisRelevamientoArticuloEntity);

				ItrisRelevamientoArticuloRequest itrisRelevamientoArticuloRequest = 
					new ItrisRelevamientoArticuloRequest()
				{
					@class = "_REL_ART",
					data = listaRelevamientoArticulo
				};

				ItrisRelevamientoArticuloBusiness itrisRelevamientoArticuloBuesiness
					= new ItrisRelevamientoArticuloBusiness(authenticateEntity);
				//var itrisRelevamientoArticuloResponse = 
				//	itrisRelevamientoArticuloBuesiness.Post(itrisRelevamientoArticuloRequest);

				#endregion


				//GET TIPOS DE COMERCIO
				ItrisTipoDeComercioBusiness bus = new ItrisTipoDeComercioBusiness(authenticateEntity);
				tipoDeComercioResponse = await bus.Get();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return tipoDeComercioResponse.data.ToList<ItrisTipoDeComercioEntity>();
		}

    }
}
