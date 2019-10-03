using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	[DataContract]
	public class ItrisErpLocalidadesEntity
	{
		[DataMember(EmitDefaultValue = false)]
		public int ID { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string DESCRIPCION { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_PARTIDOS { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Z_FK_ERP_PARTIDOS { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_PROVINCIAS { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Z_FK_ERP_PROVINCIAS { get; set; }
	}
}
