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
		TipoDeComercioResponse tipoDeComercioResponse;
		private UrlEntity _urlEntity;

		public ItrisTipoDeComercioBusiness(AuthenticateEntity authenticateEntity, UrlEntity urlEntity)
		{
			tipoDeComercioRepository = new ItrisTipoDeComercioRepository(authenticateEntity);
			_urlEntity = urlEntity;
		}

		public async Task<TipoDeComercioResponse> Get()
		{
			try
			{
				tipoDeComercioResponse = await tipoDeComercioRepository.Get(_urlEntity.GetUrl());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return tipoDeComercioResponse;
		}

	}
}
