using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceArticuloRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceArticuloRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(ARTICULO articulo)
		{
			try
			{
				var respuesta = _contexto.ARTICULO.Add(articulo);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<ARTICULO, bool> predicado)
		{
			try
			{
				return _contexto.ARTICULO.Where(predicado).ToList<ARTICULO>();
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
				return _contexto.ARTICULO.ToList<ARTICULO>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(ARTICULO articulo)
		{
			try
			{
				var result = _contexto.ARTICULO.SingleOrDefault(x => x.ID == articulo.ID);

				if (result != null)
				{
					result.FK_TIP_ART = articulo.FK_TIP_ART;
					result.Z_FK_TIP_ART = articulo.Z_FK_TIP_ART;
					result.DESCRIPCION = articulo.DESCRIPCION;
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

		public object Delete(ARTICULO articulo)
		{
			try
			{
				var result = _contexto.ARTICULO.Remove(articulo);
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
