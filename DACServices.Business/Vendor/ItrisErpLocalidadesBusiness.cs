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
    public class ItrisErpLocalidadesBusiness
    {
        private ItrisErpLocalidadesRepository itrisErpLocalidadesRepository;
        private ItrisErpLocalidadesResponse itrisErpLocalidadesResponse;
        private ItrisAuthenticateEntity itrisAuthenticateEntity;

        public ItrisErpLocalidadesBusiness(ItrisAuthenticateEntity authenticateEntity)
        {
            itrisAuthenticateEntity = authenticateEntity;
            itrisErpLocalidadesRepository = new ItrisErpLocalidadesRepository(authenticateEntity);
        }

		public async Task<ItrisErpLocalidadesResponse> GetLastUpdate(string lastUpdate)
		{
			try
			{
				itrisErpLocalidadesResponse =
					await itrisErpLocalidadesRepository.Get(itrisAuthenticateEntity.GetApi3FilterDateLastUpdate(lastUpdate));

				return itrisErpLocalidadesResponse;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ItrisErpLocalidadesResponse> Get()
        {
            try
            {
                itrisErpLocalidadesResponse =
                    await itrisErpLocalidadesRepository.Get(itrisAuthenticateEntity.GetAll());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itrisErpLocalidadesResponse;
        }
    }
}
