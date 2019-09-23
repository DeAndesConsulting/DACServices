using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Service.Entities
{
	public abstract class ServiceSyncBaseEntity<TABLE> where TABLE : class, new()
	{
		public List<TABLE> ListaCreate { get; set; }
		public List<TABLE> ListaUpdate { get; set; }
		public List<TABLE> ListaDelete { get; set; }
	}
}
