using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceRequestRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceRequestRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(tbRequest request)
		{
			try
			{
				var respuesta = _contexto.tbRequest.Add(request);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<tbRequest, bool> predicado)
		{
			try
			{
				return _contexto.tbRequest.Where(predicado).ToList<tbRequest>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(tbRequest request)
		{
			try
			{
				var result = _contexto.tbRequest.SingleOrDefault(x => x.req_id == request.req_id);

				if (result != null)
				{
					result.req_fecha_response = request.req_fecha_response;
					result.req_estado = request.req_estado;
					result.req_body_response = request.req_body_response;
					_contexto.SaveChanges();
				}
				else
				{
					throw new Exception("No se encontro el objeto a actualizar");
				}

				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
