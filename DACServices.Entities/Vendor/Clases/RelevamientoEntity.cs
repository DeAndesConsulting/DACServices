using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	public class RelevamientoEntity
	{
		public int ID { get; set; }
		public int FK_ERP_EMPRESAS { get; set; }
		public int FK_ERP_ASESORES { get; set; }
		public DateTime FECHA { get; set; }
		public string CODIGO { get; set; }
	}
}
