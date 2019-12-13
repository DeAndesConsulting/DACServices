using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceConfigurationRepository
	{
		private DB_DACSEntities _contexto = null;

		public ServiceConfigurationRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public object Read(Func<tbConfiguration, bool> predicado)
		{
			try
			{
				return _contexto.tbConfiguration.Where(predicado).ToList<tbConfiguration>();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object Update(tbConfiguration configuration)
		{
			try
			{
				var result = _contexto.tbConfiguration.SingleOrDefault(x => x.con_id == configuration.con_id);

				if (result != null)
				{
					result.con_value = configuration.con_value;
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
