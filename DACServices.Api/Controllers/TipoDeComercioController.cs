using DACServices.Business;
using DACServices.Business.Service;
using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
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

		public async Task<TipoDeComercioResponse> Get()
		{
			log.Debug("Debug log");
			log.Info("Info log");
			log.Warn("Warn log");
			log.Error("Error log");
			log.Fatal("Fatal log");

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
			serviceRequestBusiness.Create(request);

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

				ItrisTipoDeComercioBusiness bus = new ItrisTipoDeComercioBusiness(authenticateEntity, urlEntity);
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
