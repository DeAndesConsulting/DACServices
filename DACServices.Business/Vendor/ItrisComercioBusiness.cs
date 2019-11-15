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
		private ItrisComercioRepository _itrisComercioRepository;
		private ItrisComercioResponse _itrisComercioResponse;
		private ItrisAuthenticateEntity _itrisAuthenticateEntity;
        private string _sessionString = string.Empty;

        /// <summary>
        /// [IMPORTANT]: Remember that if you are using this property, you should call "CloseSession()" method.
        /// </summary>
        public string SessionString
        {
            get
            {
                if (string.IsNullOrEmpty(_sessionString))
                    return this.OpenSession();
                return _sessionString;
            }
        }

        public ItrisComercioBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			_itrisAuthenticateEntity = authenticateEntity;
			_itrisComercioRepository = new ItrisComercioRepository(authenticateEntity);
		}

        public async Task<ItrisComercioResponse> Post(ItrisComercioEntity entity, string stringSession)
        {
            try
            {
                List<ItrisComercioEntity> listaComercio = new List<ItrisComercioEntity>();
                listaComercio.Add(entity);

				ItrisComercioRequest itrisComercioRequest = new ItrisComercioRequest()
				{
					@class = "_COMERCIO",
					data = listaComercio
				};

				_itrisComercioResponse =
                    await _itrisComercioRepository.Post(_itrisAuthenticateEntity.PostUrl(), itrisComercioRequest, stringSession);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _itrisComercioResponse;
        }

        public async Task<ItrisComercioResponse> Post(ItrisComercioEntity entity)
        {
            try
            {
                List<ItrisComercioEntity> listaComercio = new List<ItrisComercioEntity>();
                listaComercio.Add(entity);

                ItrisComercioRequest itrisComercioRequest = new ItrisComercioRequest()
                {
                    @class = "_COMERCIO",
                    data = listaComercio
                };

                _itrisComercioResponse =
                    await _itrisComercioRepository.Post(_itrisAuthenticateEntity.PostUrl(), itrisComercioRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _itrisComercioResponse;
        }

        private string OpenSession()
        {
            return _itrisComercioRepository.OpenSession();
        }

        public string CloseSession(string session)
        {
            return _itrisComercioRepository.CloseSession(session);
        }
    }
}
