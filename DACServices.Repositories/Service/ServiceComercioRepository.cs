using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceComercioRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceComercioRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(COMERCIO comercio)
		{
			try
			{
				var respuesta = _contexto.COMERCIO.Add(comercio);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<COMERCIO, bool> predicado)
		{
			try
			{
				return _contexto.COMERCIO.Where(predicado).ToList<COMERCIO>();
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
				return _contexto.COMERCIO.ToList<COMERCIO>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(COMERCIO comercio)
		{
			try
			{
				var result = _contexto.COMERCIO.SingleOrDefault(x => x.ID == comercio.ID);

				if (result != null)
				{
					result.FK_TIP_COM = comercio.FK_TIP_COM;
					result.FK_ERP_LOCALIDADES = comercio.FK_ERP_LOCALIDADES;
					result.FK_ERP_PROVINCIAS = comercio.FK_ERP_PROVINCIAS;
					result.NOMBRE = comercio.NOMBRE;
					result.CALLE = comercio.CALLE;
					result.NUMERO = comercio.NUMERO;
					result.LATITUD = comercio.LATITUD;
					result.LONGITUD = comercio.LONGITUD;
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

		public object Delete(COMERCIO comercio)
		{
			try
			{
				var result = _contexto.COMERCIO.Remove(comercio);
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
