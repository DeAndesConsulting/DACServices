using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Vendor
{
	public abstract class ItrisBaseBusiness<REPOSITORY, RESPONSE> 
		where REPOSITORY: class, new()
		where RESPONSE: class, new()
	{
		private REPOSITORY repo;
		private ItrisAuthenticateEntity itrisAuthenticateEntity;

		public ItrisBaseBusiness(ItrisAuthenticateEntity authenticateEntity)
		{
			itrisAuthenticateEntity = authenticateEntity;
			//repo = new REPOSITORY(authenticateEntity);
		}

	}
}
