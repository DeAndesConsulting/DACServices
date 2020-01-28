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
	public class ItrisErpEmpresasBusiness
	{
		private ItrisErpEmpresasRepository itrisErpEmpresasRepository;
		private ItrisErpEmpresasResponse itrisErpEmpresasResponse;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisErpEmpresasBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			itrisErpEmpresasRepository = new ItrisErpEmpresasRepository(authenticateEntity);
		}

		public async Task<ItrisErpEmpresasResponse> GetLastUpdate(string lastUpdate)
		{
			try
			{
				itrisErpEmpresasResponse =
					await itrisErpEmpresasRepository.Get(itrisAuthenticateEntity.GetApi3FilterDateLastUpdate(lastUpdate));

				return itrisErpEmpresasResponse;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ItrisErpEmpresasResponse> Get()
		{
			try
			{
				itrisErpEmpresasResponse =
					await itrisErpEmpresasRepository.Get(itrisAuthenticateEntity.GetUrl());

				return itrisErpEmpresasResponse;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<ItrisErpEmpresasResponse> Get(ItrisErpEmpresasEntity entity)
		{
			try
			{
				string filter = string.Format("ID='{0}'", entity.ID);

				itrisErpEmpresasResponse =
					await itrisErpEmpresasRepository.Get(itrisAuthenticateEntity.GetAllWithFilterUrl(filter));

				return itrisErpEmpresasResponse;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
