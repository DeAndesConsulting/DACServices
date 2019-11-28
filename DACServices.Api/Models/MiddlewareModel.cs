using DACServices.Entities.Service;
using DACServices.Entities.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DACServices.Api.Models
{
	public class MiddlewareModel
	{
		public ServiceSyncErpEmpresasEntity resultDACSEmpresas { get; set; }
		public ServiceSyncErpAsesoresEntity resultDACSAsesores { get; set; }
		public ServiceSyncErpLocalidadesEntity resultDACSLocalidades { get; set; }
		public ServiceSyncArticuloEntity resultDACSArticulos { get; set; }
	}
}