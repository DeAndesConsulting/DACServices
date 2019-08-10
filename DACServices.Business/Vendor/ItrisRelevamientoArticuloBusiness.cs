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
		private UrlEntity _urlEntity;

		public ItrisRelevamientoArticuloBusiness(AuthenticateEntity authenticateEntity, UrlEntity urlEntity)
		{
			this.itrisRelevamientoArticuloRepository = new ItrisRelevamientoArticuloRepository(authenticateEntity);
			this._urlEntity = urlEntity;
		}

		public async Task<ItrisRelevamientoArticuloResponse> Post(UrlEntity urlEntity, ItrisRelevamientoArticuloRequest itrisRelevamientoArticuloRequest)
		{
			try
			{
				itrisRelevamientoArticuloResponse = 
					await itrisRelevamientoArticuloRepository.Post(urlEntity.PostUrl(), itrisRelevamientoArticuloRequest);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisRelevamientoArticuloResponse;
		}
	}
}
