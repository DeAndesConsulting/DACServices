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
		public string _claseItris { get; protected set; }

		public UrlEntity(string server, int puerto)
		{
			_server = server;
			_puerto = puerto;
		}

		public UrlEntity(string server, int puerto, string claseItris)
		{
			_server = server;
			_puerto = puerto;
			_claseItris = claseItris;
		}

		public string GetUrl()
		{
			if (!string.IsNullOrEmpty(_claseItris))
				return string.Format("http://{0}:{1}/class?class={2}", _server, _puerto, _claseItris);
			else
				throw new ArgumentNullException("_claseItris: Debe asignar este valor en el constructor");
		}

		public string PostUrl()
		{
			return string.Format("http://{0}:{1}/class", _server, _puerto);
		}
	}
}
