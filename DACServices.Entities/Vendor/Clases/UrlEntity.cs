using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	public class UrlEntity
	{
		public string _server { get; protected set; }
		public int _puerto { get; protected set; }
		public string _recurso { get; protected set; }
		public string _claseItris { get; protected set; }

		//public UrlEntity() { }

		public UrlEntity(string server, int puerto, string recurso, string claseItris)
		{
			_server = server;
			_puerto = puerto;
			_recurso = recurso;
			_claseItris = claseItris;
		}

		public string GetUrl()
		{
			return string.Format("http://{0}:{1}/{2}?class={3}", _server, _puerto, _recurso, _claseItris);
		}
	}
}
