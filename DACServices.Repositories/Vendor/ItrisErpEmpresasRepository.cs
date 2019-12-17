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
	//public class ItrisErpEmpresasRepository : ItrisBaseRepository<ItrisErpEmpresasRequest, ItrisErpEmpresasResponse>
	public class ItrisErpEmpresasRepository : ItrisApi3BaseRepository<ItrisErpEmpresasRequest, ItrisErpEmpresasResponse>
	{
		public ItrisErpEmpresasRepository(ItrisAuthenticateEntity authenticateEntity)
			: base(authenticateEntity)
		{ }
	}
}
