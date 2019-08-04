using DACServices.Entities;
using DACServices.Entities.Request;
using DACServices.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories
{
	public class TipoDeComercioRepository : ItrisRepository<TipoDeComercioRequest, TipoDeComercioResponse>
	{
		public TipoDeComercioRepository(AuthenticateEntity authenticateEntity)
			:base(authenticateEntity)
		{ }
	}
}
