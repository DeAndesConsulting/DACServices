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
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisComercioBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			itrisComercioRepository = new ItrisComercioRepository(authenticateEntity);
		}

		public async Task<ItrisComercioResponse> Post(ItrisComercioRequest request)
		{
			try
			{
				itrisComercioResponse = 
					await itrisComercioRepository.Post(itrisAuthenticateEntity.GetPostUrl(), request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisComercioResponse;
		}
	}
}
