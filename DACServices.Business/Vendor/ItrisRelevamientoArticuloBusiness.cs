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
	public class ItrisRelevamientoArticuloBusiness
	{
		private ItrisRelevamientoArticuloRepository _itrisRelevamientoArticuloRepository;
		private ItrisRelevamientoArticuloResponse _itrisRelevamientoArticuloResponse;
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

        public ItrisRelevamientoArticuloBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			_itrisAuthenticateEntity = authenticateEntity;
			this._itrisRelevamientoArticuloRepository = new ItrisRelevamientoArticuloRepository(authenticateEntity);
		}

        public async Task<ItrisRelevamientoArticuloResponse> Post(int idRelevamiento, int idComercio, List<ItrisRelevamientoArticuloEntity> listaRelevamientoArticulo, string stringSession)
        {
            ItrisRelevamientoArticuloResponse itrisRelevamientoArticuloResponseSalida =
                new ItrisRelevamientoArticuloResponse();
            itrisRelevamientoArticuloResponseSalida.data = new List<ItrisRelevamientoArticuloEntity>();
            try
            {
                List<ItrisRelevamientoArticuloEntity> lista;

                foreach (var obj in listaRelevamientoArticulo)
                {
                    obj.FK_RELEVAMIENTO = idRelevamiento;
                    obj.FK_COMERCIO = idComercio;

                    lista = new List<ItrisRelevamientoArticuloEntity>();
                    lista.Add(obj);

                    ItrisRelevamientoArticuloRequest itrisRelevamientoArticuloRequest =
                        new ItrisRelevamientoArticuloRequest()
                        {
                            @class = "_REL_ART",
                            data = lista
                        };

                    _itrisRelevamientoArticuloResponse =
                        await _itrisRelevamientoArticuloRepository.Post(
                            _itrisAuthenticateEntity.PostUrl(), itrisRelevamientoArticuloRequest, stringSession);

                    itrisRelevamientoArticuloResponseSalida.data.Add(
                        _itrisRelevamientoArticuloResponse.data.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itrisRelevamientoArticuloResponseSalida;
        }

        public async Task<ItrisRelevamientoArticuloResponse> Post(int idRelevamiento, int idComercio, List<ItrisRelevamientoArticuloEntity> listaRelevamientoArticulo)
        {
            ItrisRelevamientoArticuloResponse itrisRelevamientoArticuloResponseSalida =
                new ItrisRelevamientoArticuloResponse();
            itrisRelevamientoArticuloResponseSalida.data = new List<ItrisRelevamientoArticuloEntity>();
            try
            {
                List<ItrisRelevamientoArticuloEntity> lista;

                foreach (var obj in listaRelevamientoArticulo)
                {
                    obj.FK_RELEVAMIENTO = idRelevamiento;
                    obj.FK_COMERCIO = idComercio;

                    lista = new List<ItrisRelevamientoArticuloEntity>();
                    lista.Add(obj);

                    ItrisRelevamientoArticuloRequest itrisRelevamientoArticuloRequest =
                        new ItrisRelevamientoArticuloRequest()
                        {
                            @class = "_REL_ART",
                            data = lista
                        };

                    _itrisRelevamientoArticuloResponse =
                        await _itrisRelevamientoArticuloRepository.Post(
                            _itrisAuthenticateEntity.PostUrl(), itrisRelevamientoArticuloRequest);

                    itrisRelevamientoArticuloResponseSalida.data.Add(
                        _itrisRelevamientoArticuloResponse.data.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itrisRelevamientoArticuloResponseSalida;
        }

        private string OpenSession()
        {
            return _itrisRelevamientoArticuloRepository.OpenSession();
        }

        public string CloseSession(string session)
        {
            return _itrisRelevamientoArticuloRepository.CloseSession(session);
        }
    }
}
