using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	public class ItrisRelevamientoArticuloEntity
	{
		public int ID { get; set; }
		public int FK_RELEVAMIENTO { get; set; }
		public int FK_ARTICULOS { get; set; }
		public int FK_COMERCIO { get; set; }
		public bool EXISTE { get; set; }
		public double PRECIO { get; set; }
	}
}
