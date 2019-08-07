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
	public class ItrisComercioBusiness
	{
		private ItrisComercioRepository itrisComercioRepository;
		private ItrisComercioResponse itrisComercioResponse;
		private UrlEntity _urlEntity;

		public ItrisComercioBusiness(AuthenticateEntity authenticateEntity, UrlEntity urlEntity)
		{
			itrisComercioRepository = new ItrisComercioRepository(authenticateEntity);
			_urlEntity = urlEntity;
		}

		public async Task<ItrisComercioResponse> Post(UrlEntity url, ItrisComercioRequest request)
		{
			try
			{
				itrisComercioResponse = await itrisComercioRepository.Post(url.PostUrl(), request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisComercioResponse;
		}
	}
}
