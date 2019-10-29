using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	[DataContract]
	public class ItrisEstadoEmailRelevamientoEntity
	{
		[DataMember(EmitDefaultValue = false)]
		public int FK_RELEVAMIENTO { get; set; }

		public bool ENVIADO_POR_MAIL { get; set; }
	}
}
