using DACServices.Entities;
using DACServices.Repositories.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Service
{
	public class ServiceRequestBusiness
	{
		private ServiceRequestRepository serviceRequestRepository;

		public ServiceRequestBusiness()
		{
			serviceRequestRepository = new ServiceRequestRepository();
		}

		public void Create(tbRequest request)
		{
			serviceRequestRepository.Create(request);
		}

		public object Read(Func<tbRequest, bool> predicado)
		{
			return serviceRequestRepository.Read(predicado);
		}

		public object Update(tbRequest request)
		{
			return serviceRequestRepository.Update(request);
		}
	}
}
