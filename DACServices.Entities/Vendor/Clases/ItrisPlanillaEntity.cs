using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	public class ItrisPlanillaEntity
	{
		public ItrisRelevamientoEntity Relevamiento { get; set; }
		public ItrisComercioEntity Comercio { get; set; }
		public List<ItrisRelevamientoArticuloEntity> RelevamientoArticulo { get; set; }
		public string Imei { get; set; }
	}
}
