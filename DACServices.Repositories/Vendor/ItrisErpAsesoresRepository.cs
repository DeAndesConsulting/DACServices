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
	public class ItrisErpAsesoresRepository : ItrisApi3BaseRepository<ItrisErpAsesoresRequest, ItrisErpAsesoresResponse>
	{
		public ItrisErpAsesoresRepository(ItrisAuthenticateEntity authenticateEntity)
			: base(authenticateEntity)
		{ }
	}
}
