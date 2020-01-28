using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using DACServices.Entities.Vendor.Response;
using DACServices.Repositories.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Vendor
{
	public class ItrisErpAsesoresBusiness
	{
		private ItrisErpAsesoresRepository itrisErpAsesoresRepository;
		private ItrisErpAsesoresResponse itrisErpAsesoresResponse;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisErpAsesoresBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			itrisErpAsesoresRepository = new ItrisErpAsesoresRepository(authenticateEntity);
		}

		public async Task<ItrisErpAsesoresResponse> GetLastUpdate(string lastUpdate)
		{
			try
			{
				itrisErpAsesoresResponse =
					await itrisErpAsesoresRepository.Get(
						itrisAuthenticateEntity.GetApi3FilterDateLastUpdate(lastUpdate));

				return itrisErpAsesoresResponse;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ItrisErpAsesoresResponse> Get()
		{
			try
			{
				itrisErpAsesoresResponse =
					await itrisErpAsesoresRepository.Get(itrisAuthenticateEntity.GetAll());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisErpAsesoresResponse;
		}

		public async Task<ItrisErpAsesoresResponse> Get(ItrisErpAsesoresEntity entity)
		{
			try
			{
				string filter = string.Format("ID>{0}", entity.ID);

				itrisErpAsesoresResponse =
					await itrisErpAsesoresRepository.Get(itrisAuthenticateEntity.GetAllWithFilterUrl(filter));
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return itrisErpAsesoresResponse;
		}
	}
}
