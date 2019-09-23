using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories
{
	public class ServiceErpLocalidadesRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceErpLocalidadesRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(ERP_LOCALIDADES localidad)
		{
			try
			{
				var respuesta = _contexto.ERP_LOCALIDADES.Add(localidad);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<ERP_LOCALIDADES, bool> predicado)
		{
			try
			{
				return _contexto.ERP_LOCALIDADES.Where(predicado).ToList<ERP_LOCALIDADES>();
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
				return _contexto.ERP_LOCALIDADES.ToList<ERP_LOCALIDADES>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(ERP_LOCALIDADES localidad)
		{
			try
			{
				var result = _contexto.ERP_LOCALIDADES.SingleOrDefault(x => x.ID == localidad.ID);

				if (result != null)
				{
					result.FK_ERP_PARTIDOS = localidad.FK_ERP_PARTIDOS;
					result.FK_ERP_PROVINCIAS = localidad.FK_ERP_PROVINCIAS;
					result.DESCRIPCION = localidad.DESCRIPCION;
					result.Z_FK_ERP_PARTIDOS = localidad.Z_FK_ERP_PARTIDOS;
					result.Z_FK_ERP_PROVINCIAS = localidad.Z_FK_ERP_PROVINCIAS;
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

		public object Delete(ERP_LOCALIDADES localidad)
		{
			try
			{
				var result = _contexto.ERP_LOCALIDADES.Remove(localidad);
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
