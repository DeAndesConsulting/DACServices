using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	public class ItrisComercioEntity
	{
		public int ID { get; set; }
		public int FK_TIP_COM{ get; set; }
		public string NOMBRE { get; set; }
		public string CALLE { get; set; }
		[Obsolete("Este parametro tiene que ser entero, leandro lo mapeo como string. Preguntar porque")]
		public string NUMERO { get; set; }
		public string LOCALIDAD { get; set; }
		public string PROVINCIA { get; set; }
		public string LATITUD { get; set; }
		public string LONGITUD { get; set; }
	}
}
