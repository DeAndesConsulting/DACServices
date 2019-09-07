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
	public class ItrisTipoDeArticuloBusiness
	{
		ItrisTipoDeArticuloRepository tipoDeArticuloRepository;
		ItrisTipoDeArticuloResponse tipoDeArticuloResponse;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisTipoDeArticuloBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			tipoDeArticuloRepository = new ItrisTipoDeArticuloRepository(authenticateEntity);
		}

		public async Task<ItrisTipoDeArticuloResponse> Get()
		{
			try
			{
				tipoDeArticuloResponse = await tipoDeArticuloRepository.Get(itrisAuthenticateEntity.GetUrl());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return tipoDeArticuloResponse;
		}
	}
}
