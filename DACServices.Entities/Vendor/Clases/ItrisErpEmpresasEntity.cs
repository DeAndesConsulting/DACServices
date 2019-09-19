using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	[DataContract]
	public class ItrisErpEmpresasEntity
	{
		[DataMember(EmitDefaultValue = false)]
		public string ID { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string NOM_FANTASIA { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Z_FK_ERP_LOCALIDADES { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Z_FK_ERP_PARTIDOS { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Z_FK_ERP_PROVINCIAS { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_ASESORES { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_ASESORES2 { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_ASESORES3 { get; set; }
	}
}
