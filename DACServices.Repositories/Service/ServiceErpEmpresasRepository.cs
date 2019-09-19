using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceErpEmpresasRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceErpEmpresasRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public void Create(ERP_EMPRESAS empresa)
		{
			try
			{
				var respuesta = _contexto.ERP_EMPRESAS.Add(empresa);
				_contexto.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read(Func<ERP_EMPRESAS, bool> predicado)
		{
			try
			{
				return _contexto.ERP_EMPRESAS.Where(predicado).ToList<ERP_EMPRESAS>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Read()
		{
			try
			{
				return _contexto.ERP_EMPRESAS.ToList<ERP_EMPRESAS>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public object Update(ERP_EMPRESAS empresa)
		{
			try
			{
				var result = _contexto.ERP_EMPRESAS.SingleOrDefault(x => x.ID == empresa.ID);

				if (result != null)
				{
					result.NOM_FANTASIA = empresa.NOM_FANTASIA;
					result.Z_FK_ERP_LOCALIDADES = empresa.Z_FK_ERP_LOCALIDADES;
					result.Z_FK_ERP_PARTIDOS = empresa.Z_FK_ERP_PARTIDOS;
					result.Z_FK_ERP_PROVINCIAS = empresa.Z_FK_ERP_PROVINCIAS;
					result.FK_ERP_ASESORES = empresa.FK_ERP_ASESORES;
					result.FK_ERP_ASESORES2 = empresa.FK_ERP_ASESORES2;
					result.FK_ERP_ASESORES3 = empresa.FK_ERP_ASESORES3;
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

		public object Delete(ERP_EMPRESAS empresa)
		{
			try
			{
				var result = _contexto.ERP_EMPRESAS.Remove(empresa);
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
