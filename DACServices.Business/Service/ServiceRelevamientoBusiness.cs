using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Service
{
	public class ServiceRelevamientoBusiness
	{
		private ServiceRequestBusiness serviceRequestBusiness;

		public ServiceRelevamientoBusiness()
		{
			serviceRequestBusiness = new ServiceRequestBusiness();
		}

		public ItrisRelevamientoEntity Post(tbRequest request)
		{
			serviceRequestBusiness.Create(request);
			return null;
		}
	}
}
