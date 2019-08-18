using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using DACServices.Entities.Vendor.Request;
using DACServices.Entities.Vendor.Response;
using DACServices.Repositories.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Vendor
{
	public class ItrisRelevamientoArticuloBusiness
	{
		private ItrisRelevamientoArticuloRepository itrisRelevamientoArticuloRepository;
		private ItrisRelevamientoArticuloResponse itrisRelevamientoArticuloResponse;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisRelevamientoArticuloBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			this.itrisRelevamientoArticuloRepository = new ItrisRelevamientoArticuloRepository(authenticateEntity);
		}

		//public async Task<List<ItrisRelevamientoArticuloEntity>> Post(int idRelevamiento, int idComercio, List<ItrisRelevamientoArticuloEntity> listaRelevamientoArticulo)
		public async Task<ItrisRelevamientoArticuloResponse> Post(int idRelevamiento, int idComercio, List<ItrisRelevamientoArticuloEntity> listaRelevamientoArticulo)
		{
			//List<ItrisRelevamientoArticuloEntity> listaSalida = new List<ItrisRelevamientoArticuloEntity>();
			ItrisRelevamientoArticuloResponse itrisRelevamientoArticuloResponseSalida = 
				new ItrisRelevamientoArticuloResponse();
			itrisRelevamientoArticuloResponseSalida.data = new List<ItrisRelevamientoArticuloEntity>();
			try
			{
				List<ItrisRelevamientoArticuloEntity> lista;

				foreach (var obj in listaRelevamientoArticulo)
				{
					obj.FK_RELEVAMIENTO = idRelevamiento;
					obj.FK_COMERCIO = idComercio;

					lista = new List<ItrisRelevamientoArticuloEntity>();
					lista.Add(obj);

					ItrisRelevamientoArticuloRequest itrisRelevamientoArticuloRequest =
						new ItrisRelevamientoArticuloRequest()
						{
							@class = "_REL_ART",
							data = lista
						};

					itrisRelevamientoArticuloResponse =
						await itrisRelevamientoArticuloRepository.Post(
							itrisAuthenticateEntity.GetPostUrl(), itrisRelevamientoArticuloRequest);

					//listaSalida.Add(itrisRelevamientoArticuloResponse.data.FirstOrDefault());
					itrisRelevamientoArticuloResponseSalida.data.Add(
						itrisRelevamientoArticuloResponse.data.FirstOrDefault());
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}	
			return itrisRelevamientoArticuloResponseSalida;
		}
	}
}
