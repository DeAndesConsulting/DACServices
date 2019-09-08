using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceErpAsesoresRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceErpAsesoresRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(ERP_ASESORES asesor)
		{
			try
			{
				var respuesta = _contexto.ERP_ASESORES.Add(asesor);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<ERP_ASESORES, bool> predicado)
		{
			try
			{
				return _contexto.ERP_ASESORES.Where(predicado).ToList<ERP_ASESORES>();
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
				return _contexto.ERP_ASESORES.ToList<ERP_ASESORES>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(ERP_ASESORES asesor)
		{
			try
			{
				var result = _contexto.ERP_ASESORES.SingleOrDefault(x => x.ID == asesor.ID);

				if (result != null)
				{
					result.DESCRIPCION = asesor.DESCRIPCION;
					result.C_IMEI = asesor.C_IMEI;
					result.C_IMEI_ADMIN = asesor.C_IMEI_ADMIN;
					result.C_EMAIL = asesor.C_EMAIL;
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

		public object Delete(ERP_ASESORES asesor)
		{
			try
			{
				var result = _contexto.ERP_ASESORES.Remove(asesor);
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
