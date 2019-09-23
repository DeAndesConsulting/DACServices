using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceTipArtRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceTipArtRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(TIP_ART tipoArticulo)
		{
			try
			{
				var respuesta = _contexto.TIP_ART.Add(tipoArticulo);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<TIP_ART, bool> predicado)
		{
			try
			{
				return _contexto.TIP_ART.Where(predicado).ToList<TIP_ART>();
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
				return _contexto.TIP_ART.ToList<TIP_ART>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(TIP_ART tipoArticulo)
		{
			try
			{
				var result = _contexto.TIP_ART.SingleOrDefault(x => x.ID == tipoArticulo.ID);

				if (result != null)
				{
					result.DESCRIPCION = tipoArticulo.DESCRIPCION;
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

		public object Delete(TIP_ART tipoArticulo)
		{
			try
			{
				var result = _contexto.TIP_ART.Remove(tipoArticulo);
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
