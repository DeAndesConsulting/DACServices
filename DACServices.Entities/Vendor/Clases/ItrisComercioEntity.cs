using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	[DataContract]
	public class ItrisComercioEntity
	{
		[DataMember(EmitDefaultValue = false)]
		public int ID { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_TIP_COM{ get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string NOMBRE { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string CALLE { get; set; }

		[DataMember(EmitDefaultValue = false)]
		[Obsolete("Este parametro tiene que ser entero, leandro lo mapeo como string. Preguntar porque")]
		public string NUMERO { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_LOCALIDADES { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_PROVINCIAS { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string LATITUD { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string LONGITUD { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string HORA_VISITA { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public object _DET_REL_ART { get; set; }
	}
}
