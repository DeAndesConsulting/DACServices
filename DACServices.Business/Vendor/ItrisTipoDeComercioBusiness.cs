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
	public class ItrisTipoDeComercioBusiness
	{
		ItrisTipoDeComercioRepository tipoDeComercioRepository;
		ItrisTipoDeComercioResponse tipoDeComercioResponse;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisTipoDeComercioBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			tipoDeComercioRepository = new ItrisTipoDeComercioRepository(authenticateEntity);
		}

		public async Task<ItrisTipoDeComercioResponse> Get()
		{
			try
			{
				tipoDeComercioResponse = await tipoDeComercioRepository.Get(itrisAuthenticateEntity.GetUrl());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return tipoDeComercioResponse;
		}

	}
}
