using DACServices.Entities;
using DACServices.Entities.Vendor.Request;
using DACServices.Entities.Vendor.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Vendor
{
	public class ItrisTipoDeComercioRepository : ItrisBaseRepository<TipoDeComercioRequest, ItrisTipoDeComercioResponse>
	{
		public ItrisTipoDeComercioRepository(ItrisAuthenticateEntity authenticateEntity)
			: base(authenticateEntity)
		{ }
	}
}
