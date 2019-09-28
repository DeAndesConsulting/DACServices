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
		ItrisTipoDeArticuloRepository _tipoDeArticuloRepository;
		ItrisTipoDeArticuloResponse _tipoDeArticuloResponse;
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

		public ItrisTipoDeArticuloBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			_itrisAuthenticateEntity = authenticateEntity;
			_tipoDeArticuloRepository = new ItrisTipoDeArticuloRepository(authenticateEntity);
		}

		/// <summary>
		/// If you send sessionString parameter, remeber close it from bussiness.
		/// </summary>
		/// <param name="sessionString">Vendor sessionString</param>
		/// <returns></returns>
		public async Task<ItrisTipoDeArticuloResponse> Get(string sessionString)
		{
			try
			{
				_tipoDeArticuloResponse = await _tipoDeArticuloRepository.Get(_itrisAuthenticateEntity.GetUrl(), sessionString);
				return _tipoDeArticuloResponse;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// In this method, sessionString is open and close by repository base.
		/// </summary>
		/// <returns></returns>
		public async Task<ItrisTipoDeArticuloResponse> Get()
		{
			try
			{
				_tipoDeArticuloResponse = await _tipoDeArticuloRepository.Get(_itrisAuthenticateEntity.GetUrl());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return _tipoDeArticuloResponse;
		}

		private string OpenSession()
		{
			return _tipoDeArticuloRepository.OpenSession();
		}

		public string CloseSession(string session)
		{
			 return _tipoDeArticuloRepository.CloseSession(session);
		}
	}
}
