using DACServices.Entities;
using DACServices.Entities.Response;
using DACServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business
{
    public class TipoDeComercioBusiness
    {
		TipoDeComercioRepository tipoDeComercioRepository;
		TipoDeComercioResponse tipoDeComercioResponse;
		private UrlEntity _urlEntity;

		public TipoDeComercioBusiness(AuthenticateEntity authenticateEntity, UrlEntity urlEntity)
		{
			tipoDeComercioRepository = new TipoDeComercioRepository(authenticateEntity);
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
