using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Response
{
	public class LoginItrisResponseEntity : ItrisResponse<LoginItrisResponseEntity>
	{
		[DataMember(EmitDefaultValue = false)]
		public string usersession { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string access_token { get; set; }
	}
}
