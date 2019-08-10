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
		private UrlEntity _urlEntity;

		public ItrisRelevamientoBusiness(AuthenticateEntity authenticateEntity, UrlEntity urlEntity)
		{
			itrisRelevamientoRepository = new ItrisRelevamientoRepository(authenticateEntity);
			_urlEntity = urlEntity;
		}

		public async Task<ItrisRelevamientoResponse> Post(UrlEntity urlEntity, ItrisRelevamientoRequest request)
		{
			try
			{
				itrisRelevamientoResponse = await itrisRelevamientoRepository.Post(urlEntity.PostUrl(), request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisRelevamientoResponse;
		}
	}
}
