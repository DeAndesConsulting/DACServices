using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using DACServices.Repositories.Vendor;
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
		private ItrisRelevamientoRepository itrisRelevamientoRepository;

		public ServiceRelevamientoBusiness()
		{
			serviceRequestBusiness = new ServiceRequestBusiness();
			//itrisRelevamientoRepository = new ItrisRelevamientoRepository();
		}

		public ItrisRelevamientoEntity Post(tbRequest request, ItrisPlanillaEntity planilla)
		{
			ItrisRelevamientoEntity itrisRelevamientoEntity = new ItrisRelevamientoEntity();

			//Creo objeto en base local
			serviceRequestBusiness.Create(request);

			//PROCSO DE ENVIAR LOS DATOS A ITRIS
			//...
			//...
			//PROCSO DE ENVIAR LOS DATOS A ITRIS

			//PERSISTENCIA ITRIS OK => ACTUALIZO BASE LOCAL CON OK
			if (itrisRelevamientoEntity.ID != 0)
			{
				request.req_fecha_response = DateTime.Now;
				request.req_estado = true;
				serviceRequestBusiness.Update(request);
			}

			return null;
		}
	}
}
