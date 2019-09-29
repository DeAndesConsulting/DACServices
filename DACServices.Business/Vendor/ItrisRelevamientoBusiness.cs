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
	public class ItrisRelevamientoBusiness
	{
		private ItrisRelevamientoRepository _itrisRelevamientoRepository;
		private ItrisRelevamientoResponse _itrisRelevamientoResponse;
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

        public ItrisRelevamientoBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			_itrisAuthenticateEntity = authenticateEntity;
			_itrisRelevamientoRepository = new ItrisRelevamientoRepository(authenticateEntity);
		}

        public async Task<ItrisRelevamientoResponse> Post(ItrisRelevamientoEntity entity, string stringSession)
        {
            try
            {
                List<ItrisRelevamientoEntity> listaRelevamiento = new List<ItrisRelevamientoEntity>();
                listaRelevamiento.Add(entity);

                ItrisRelevamientoRequest request = new ItrisRelevamientoRequest()
                {
                    @class = "_RELEVAMIENTO",
                    data = listaRelevamiento
                };

                _itrisRelevamientoResponse =
                    await _itrisRelevamientoRepository.Post(_itrisAuthenticateEntity.PostUrl(), request, stringSession);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _itrisRelevamientoResponse;
        }

        public async Task<ItrisRelevamientoResponse> Post(ItrisRelevamientoEntity entity)
        {
            try
            {
                List<ItrisRelevamientoEntity> listaRelevamiento = new List<ItrisRelevamientoEntity>();
                listaRelevamiento.Add(entity);

                ItrisRelevamientoRequest request = new ItrisRelevamientoRequest()
                {
                    @class = "_RELEVAMIENTO",
                    data = listaRelevamiento
                };

                _itrisRelevamientoResponse =
                    await _itrisRelevamientoRepository.Post(_itrisAuthenticateEntity.PostUrl(), request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _itrisRelevamientoResponse;
        }

        private string OpenSession()
        {
            return _itrisRelevamientoRepository.OpenSession();
        }

        public string CloseSession(string session)
        {
            return _itrisRelevamientoRepository.CloseSession(session);
        }
    }
}
