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

		public async Task<ItrisRelevamientoArticuloResponse> Post(ItrisRelevamientoArticuloRequest itrisRelevamientoArticuloRequest)
		{
			try
			{
				itrisRelevamientoArticuloResponse = 
					await itrisRelevamientoArticuloRepository.Post(
						itrisAuthenticateEntity.GetPostUrl(), itrisRelevamientoArticuloRequest);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisRelevamientoArticuloResponse;
		}
	}
}
