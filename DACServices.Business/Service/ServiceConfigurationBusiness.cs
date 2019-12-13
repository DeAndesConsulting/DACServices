using DACServices.Entities;
using DACServices.Repositories.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Service
{
	public class ServiceConfigurationBusiness
	{
		private ServiceConfigurationRepository serviceConfigurationRepository = null;

		public ServiceConfigurationBusiness()
		{
			serviceConfigurationRepository = new ServiceConfigurationRepository();
		}

		public object Read(Func<tbConfiguration, bool> predicado)
		{
			return serviceConfigurationRepository.Read(predicado);
		}

		public object Update(tbConfiguration configuration)
		{
			return serviceConfigurationRepository.Update(configuration);
		}
	}
}
