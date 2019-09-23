using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceTipComRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceTipComRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(TIP_COM tipoComercio)
		{
			try
			{
				var respuesta = _contexto.TIP_COM.Add(tipoComercio);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<TIP_COM, bool> predicado)
		{
			try
			{
				return _contexto.TIP_COM.Where(predicado).ToList<TIP_COM>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Read()
		{
			try
			{
				return _contexto.TIP_COM.ToList<TIP_COM>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(TIP_COM tipoComercio)
		{
			try
			{
				var result = _contexto.TIP_COM.SingleOrDefault(x => x.ID == tipoComercio.ID);

				if (result != null)
				{
					result.DESCRIPCION = tipoComercio.DESCRIPCION;
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

		public object Delete(TIP_COM tipoComercio)
		{
			try
			{
				var result = _contexto.TIP_COM.Remove(tipoComercio);
				_contexto.SaveChanges();
				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
