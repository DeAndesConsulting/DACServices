using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceRequestRepository
	{
		private DB_DACSEntities _contexto = null;

		public void Create(tbRequest request)
		{
			try
			{
				_contexto = new DB_DACSEntities();
				var respuesta = _contexto.tbRequest.Add(request);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Get(Func<tbRequest, bool> predicado)
		{
			try
			{
				_contexto = new DB_DACSEntities();
				return _contexto.tbRequest.Where(predicado).ToList<tbRequest>();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
