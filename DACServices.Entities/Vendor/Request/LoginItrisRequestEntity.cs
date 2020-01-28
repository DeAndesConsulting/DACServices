using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Request
{
	[DataContract]
	public class LoginItrisRequestEntity
	{
		[DataMember(EmitDefaultValue = false)]
		public string username { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string password { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string database { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string usersession { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string app { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string config { get; set; }
	}
}
