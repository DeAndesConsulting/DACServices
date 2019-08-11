using DACServices.Api.Models;
using DACServices.Business.Service;
using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DACServices.Api.Controllers
{
    public class RelevamientoController : ApiController
    {
		public ItrisRelevamientoEntity Post([FromBody] RelevamientoModel model)
		{
			ItrisRelevamientoEntity entityReturn = null;
			HttpResponseMessage response = new HttpResponseMessage();

			model = this.LoadMockRelevamientoComercioNuevo();

			tbRequest request = new tbRequest()
			{
				req_fecha_request = DateTime.Now,
				req_fecha_response = null,
				req_body_request = JsonConvert.SerializeObject(model),
				req_estado = false,
				req_imei = "123ASD123"
			};

			ServiceRelevamientoBusiness serviceRelevamientoBusiness = new ServiceRelevamientoBusiness();
			var result = serviceRelevamientoBusiness.Post(request);

			return entityReturn;
		}

		private RelevamientoModel LoadMockRelevamientoComercioNuevo()
		{
			RelevamientoModel model = new RelevamientoModel();

			//FECHA EN EL FORMATO QUE NECESITO
			string fecha = DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss.fff");

			ItrisRelevamientoEntity relevamiento = new ItrisRelevamientoEntity()
			{
				FK_ERP_EMPRESAS = 1,
				FK_ERP_ASESORES = 1,
				FECHA = DateTime.Now, //Convert.ToDateTime("01/01/2019 13:25:15.123"),
				CODIGO = "ASD123ADS"
			};
			model.ItrisRelevamientoEntity = relevamiento;

			ItrisComercioEntity comercio = new ItrisComercioEntity()
			{
				FK_TIP_COM = 1,
				NOMBRE = "Don Claudio",
				CALLE = "Baigorria",
				NUMERO = "5089",
				LOCALIDAD = "CABA",
				PROVINCIA = "Buenos aires",
				LATITUD = "999999.1010101",
				LONGITUD = "12132.55555"
			};
			model.ItrisComercioEntity = comercio;

			List<ItrisRelevamientoArticuloEntity> lista = new List<ItrisRelevamientoArticuloEntity>();

			ItrisRelevamientoArticuloEntity relevamientoArticulo;

			for (int i = 1; i < 10; i++)
			{
				relevamientoArticulo = new ItrisRelevamientoArticuloEntity()
				{
					FK_ARTICULOS = i,
					EXISTE = true,
					PRECIO = 1.4 + i
				};
				lista.Add(relevamientoArticulo);
			}
			model.ListaRelevamientoArticulo = lista;

			return model;
		}
	}
}
