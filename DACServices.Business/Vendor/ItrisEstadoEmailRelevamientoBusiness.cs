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
	public class ItrisEstadoEmailRelevamientoBusiness
	{
		private ItrisEstadoEmailRelevamientoRepository _itrisEstadoEmailRelevamientoRepository;
		private ItrisEstadoEmailRelevamientoResponse _itrisEstadoEmailRelevamientoResponse;
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

		public ItrisEstadoEmailRelevamientoBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			_itrisAuthenticateEntity = authenticateEntity;
			_itrisEstadoEmailRelevamientoRepository = new ItrisEstadoEmailRelevamientoRepository(authenticateEntity);
		}

		public async Task<ItrisEstadoEmailRelevamientoResponse> Post(ItrisEstadoEmailRelevamientoEntity entity, string stringSession)
		{
			try
			{
				List<ItrisEstadoEmailRelevamientoEntity> listaEstadoEmailRelevamiento = new List<ItrisEstadoEmailRelevamientoEntity>();
				listaEstadoEmailRelevamiento.Add(entity);

				ItrisEstadoEmailRelevamientoRequest request = new ItrisEstadoEmailRelevamientoRequest()
				{
					@class = "_EST_MAIL_REL",
					data = listaEstadoEmailRelevamiento
				};

				_itrisEstadoEmailRelevamientoResponse =
					await _itrisEstadoEmailRelevamientoRepository.Post(_itrisAuthenticateEntity.PostUrl(), request, stringSession);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return _itrisEstadoEmailRelevamientoResponse;
		}

		public async Task<ItrisEstadoEmailRelevamientoResponse> Post(ItrisEstadoEmailRelevamientoEntity entity)
		{
			try
			{
				List<ItrisEstadoEmailRelevamientoEntity> listaEstadoEmailRelevamiento = new List<ItrisEstadoEmailRelevamientoEntity>();
				listaEstadoEmailRelevamiento.Add(entity);

				ItrisEstadoEmailRelevamientoRequest request = new ItrisEstadoEmailRelevamientoRequest()
				{
					@class = "_EST_MAIL_REL",
					data = listaEstadoEmailRelevamiento
				};

				_itrisEstadoEmailRelevamientoResponse =
					await _itrisEstadoEmailRelevamientoRepository.Post(_itrisAuthenticateEntity.PostUrl(), request);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return _itrisEstadoEmailRelevamientoResponse;
		}

		private string OpenSession()
		{
			return _itrisEstadoEmailRelevamientoRepository.OpenSession();
		}

		public string CloseSession(string session)
		{
			return _itrisEstadoEmailRelevamientoRepository.CloseSession(session);
		}
	}
}
