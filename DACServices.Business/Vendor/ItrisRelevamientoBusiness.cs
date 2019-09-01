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
	public class ItrisRelevamientoBusiness
	{
		private ItrisRelevamientoRepository itrisRelevamientoRepository;
		private ItrisRelevamientoResponse itrisRelevamientoResponse;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisRelevamientoBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			itrisRelevamientoRepository = new ItrisRelevamientoRepository(authenticateEntity);
		}

		public async Task<ItrisRelevamientoResponse> Post(ItrisRelevamientoEntity entity)
		{
			try
			{
				List<ItrisRelevamientoEntity> listaRelevamiento = new List<ItrisRelevamientoEntity>();
				listaRelevamiento.Add(entity);

				ItrisRelevamientoRequest request = new ItrisRelevamientoRequest()
				{
					@class = "_RELEVAMIENTO",
					data = listaRelevamiento
				};

				itrisRelevamientoResponse = 
					await itrisRelevamientoRepository.Post(itrisAuthenticateEntity.PostUrl(), request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisRelevamientoResponse;
		}
	}
}
