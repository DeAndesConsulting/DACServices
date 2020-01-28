using DACServices.Entities;
using DACServices.Entities.Vendor.Response;
using DACServices.Repositories.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Vendor
{
	public class ItrisArticuloBusiness
	{
		ItrisArticuloRepository itrisArticuloRepository;
		ItrisArticuloResponse itrisArticuloResponse;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisArticuloBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			itrisArticuloRepository = new ItrisArticuloRepository(authenticateEntity);
		}

		public async Task<ItrisArticuloResponse> GetLastUpdate(string lastUpdate)
		{
			try
			{
				itrisArticuloResponse =
					await itrisArticuloRepository.Get(itrisAuthenticateEntity.GetApi3FilterDateLastUpdate(lastUpdate));

				return itrisArticuloResponse;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ItrisArticuloResponse> Get()
		{
			try
			{
				itrisArticuloResponse = await itrisArticuloRepository.Get(itrisAuthenticateEntity.GetUrl());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisArticuloResponse;
		}
	}
}
