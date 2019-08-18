using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Response
{
	public class ItrisResponse<RP> where RP : class, new()
	{
		public int status { get; set; }
		public bool error { get; set; }
		public string message { get; set; }
		public List<RP> data { get; set; }
	}
}
