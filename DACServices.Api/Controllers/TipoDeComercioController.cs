using DACServices.Api.Models;
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

		//public async Task<TipoDeComercioResponse> Get()
		public async Task<List<ItrisTipoDeComercioEntity>> Get()
		{
			//log.Debug("Debug log");
			//log.Info("Info log");
			//log.Warn("Warn log");
			//log.Error("Error log");
			//log.Fatal("Fatal log");


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

			#region INSERT TABLA REQUEST - LEO
			tbRequest request = new tbRequest()
			{
				req_id_mobile = 1,
				req_fecha_request = DateTime.Now,
				req_fecha_response = null,
				req_body_request = "{ \"usersession\": \"{A31C7B4F-49B4-4B5B-BF93-670D4E43274A}\", \"class\": \"ERP_COM_VEN_FAC\", \"data\": [ { \"ID\": \"\", \"FECHA\": \"26/01/2018\", \"FK_ERP_T_COM_VEN\": \"FVE\", \"FK_ERP_EMPRESAS\": \"507\", \"ERP_DET_COM\": [ { \"FK_ERP_ARTICULOS\": \"20\", \"FK_ERP_COLORES\": \"1\", \"PRE_LIS\": \"1500\" }, { \"FK_ERP_ARTICULOS\": \"25\", \"FK_ERP_COLORES\": \"3\", \"PRE_LIS\": \"2400\" } ], \"ERP_DET_TES\": [ { \"FK_ERP_CUE_TES\": \"1150\", \"FK_ERP_CEN_COS\": \"1\", \"TIPO\": \"H\", \"IMPORTE\": \"3900\" }, { \"FK_ERP_CUE_TES\": \"1250\", \"FK_ERP_CEN_COS\": \"1\", \"TIPO\": \"D\", \"IMPORTE\": \"3900\" } ] } ]}",
				req_estado = false,
				req_imei = "IMEI123456"
			};
			ServiceRequestBusiness serviceRequestBusiness = new ServiceRequestBusiness();
			//serviceRequestBusiness.Create(request);
			#endregion

			TipoDeComercioResponse tipoDeComercioResponse = null;
			ItrisComercioResponse itrisComercioResponse = null;
			try
			{
				UrlEntity urlEntity = new UrlEntity(SERVER, PUERTO, CLASE);

				AuthenticateEntity authenticateEntity = 
					new AuthenticateEntity(USER_NAME, PASSWORD, DATABASE, URL_AUTHENTICATE);

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

				ItrisComercioBusiness itrisComercioBusiness = new ItrisComercioBusiness(authenticateEntity, urlEntity);
				//itrisComercioResponse = await itrisComercioBusiness.Post(urlEntity, itrisComercioRequest);
				#endregion

				#region ----------- POST RELEVAMIENTO (OK)-------------
				ItrisRelevamientoEntity itrisRelevamientoEntity = new ItrisRelevamientoEntity()
				{
					FK_ERP_EMPRESAS = 1,
					FK_ERP_ASESORES = 1,
					FECHA = DateTime.Now,
					CODIGO = "ASD123ASD"
				};
				List<ItrisRelevamientoEntity> listaRelevamiento = new List<ItrisRelevamientoEntity>();
				listaRelevamiento.Add(itrisRelevamientoEntity);

				ItrisRelevamientoRequest itrisRelevamientoRequest = new ItrisRelevamientoRequest()
				{
					@class = "_RELEVAMIENTO",
					data = listaRelevamiento
				};

				ItrisRelevamientoBusiness itrisRelevamientoBusiness = new ItrisRelevamientoBusiness(authenticateEntity, urlEntity);
				//var itrisRelevamientoResponse = await itrisRelevamientoBusiness.Post(urlEntity, itrisRelevamientoRequest);

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

				List<ItrisRelevamientoArticuloEntity> listaRelevamientoArticulo = new List<ItrisRelevamientoArticuloEntity>();
				listaRelevamientoArticulo.Add(itrisRelevamientoArticuloEntity);

				ItrisRelevamientoArticuloRequest itrisRelevamientoArticuloRequest = new ItrisRelevamientoArticuloRequest()
				{
					@class = "_REL_ART",
					data = listaRelevamientoArticulo
				};

				ItrisRelevamientoArticuloBusiness itrisRelevamientoArticuloBuesiness
					= new ItrisRelevamientoArticuloBusiness(authenticateEntity, urlEntity);
				//var itrisRelevamientoArticuloResponse = itrisRelevamientoArticuloBuesiness.Post(urlEntity, itrisRelevamientoArticuloRequest);

				#endregion


				//GET TIPOS DE COMERCIO
				ItrisTipoDeComercioBusiness bus = new ItrisTipoDeComercioBusiness(authenticateEntity, urlEntity);
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
