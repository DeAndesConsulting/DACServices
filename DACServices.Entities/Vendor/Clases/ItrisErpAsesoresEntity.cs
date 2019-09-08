using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	[DataContract]
	public class ItrisErpAsesoresEntity
	{
		[DataMember(EmitDefaultValue = false)]
		public int ID { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string DESCRIPCION { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string _EMAIL { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string _IMEI { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool _IMEI_ADMIN { get; set; }
	}
}
