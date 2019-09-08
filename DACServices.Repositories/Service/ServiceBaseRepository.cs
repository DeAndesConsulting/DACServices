using DACServices.Entities;
using DACServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
	public class ServiceBaseRepository : IServiceBaseRepository<ERP_ASESORES>
	{
		private DB_DACSEntities _contexto = null;

		public ServiceBaseRepository()
		{
			_contexto = new DB_DACSEntities();
		}

		public ERP_ASESORES Create(ERP_ASESORES entity)
		{
			throw new NotImplementedException();
		}

		public bool Delete(ERP_ASESORES entity)
		{
			throw new NotImplementedException();
		}

		public object Get()
		{
			throw new NotImplementedException();
		}

		public object GetByFilter(Func<ERP_ASESORES, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public ERP_ASESORES Update(ERP_ASESORES entity)
		{
			throw new NotImplementedException();
		}
	}
}
