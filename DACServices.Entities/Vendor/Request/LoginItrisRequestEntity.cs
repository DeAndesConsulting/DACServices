using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Request
{
	public class LoginItrisRequestEntity
	{
		public string username { get; set; }
		public string password { get; set; }
		public string database { get; set; }
	}
}
