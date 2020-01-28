using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities
{
	public class ItrisAuthenticateEntity
	{
		public string _server { get; protected set; }
		public string _puerto { get; protected set; }
		public string _claseItris { get; protected set; }

		public string _username { get; protected set; }
		public string _password { get; protected set; }
		public string _database { get; protected set; }
		public string _app { get; protected set; }
		public string _config { get; protected set; }

		public ItrisAuthenticateEntity(
			string server, string puerto, string claseItris, string username, string password, string database)
		{
			_server = server;
			_puerto = puerto;
			_claseItris = claseItris;

			_username = username;
			_password = password;
			_database = database;
		}

		public ItrisAuthenticateEntity(
			string server, string puerto, string claseItris, string app, string config, string username, string password)
		{
			_server = server;
			_puerto = puerto;
			_claseItris = claseItris;

			_app = app;
			_config = config;
			_username = username;
			_password = password;
		}

		public string GetUrl()
		{
			if (!string.IsNullOrEmpty(_claseItris))
				return string.Format("http://{0}:{1}/class?class={2}&recordCount=-1", _server, _puerto, _claseItris);
			throw new ArgumentNullException("_claseItris: Debe asignar este valor en el constructor");
		}

		public string GetAll()
		{
			if (!string.IsNullOrEmpty(_claseItris))
				return string.Format("http://{0}:{1}/class?class={2}&recordCount=-1", _server, _puerto, _claseItris);
			throw new ArgumentNullException("_claseItris: Debe asignar este valor en el constructor");
		}

		public string GetAllWithFilterUrl(string sqlFilter)
		{
			if (!string.IsNullOrEmpty(_claseItris))
				return string.Format("http://{0}:{1}/class?class={2}&sqlFilter={3}&recordCount=-1", _server, _puerto, _claseItris, sqlFilter);
			throw new ArgumentNullException("_claseItris: Debe asignar este valor en el constructor");
		}


		/// <summary>
		/// This method return filter to search in itris class by date last update filter
		/// </summary>
		/// <param name="lastUpdate">Date format must be 'dd/MM/yyyy HH:mm:ss'</param>
		/// <returns>Itris sqlFilter param</returns>
		public string GetFilterDateLastUpdate(string lastUpdate)
		{
			string sqlFilter = string.Format("FEC_ULT_ACT > convert(datetime, '{0}', 103)", lastUpdate);
			return this.GetAllWithFilterUrl(sqlFilter);
		}

		/// <summary>
		/// This method return filter to search in itris API 3 class by date last update filter
		/// </summary>
		/// <param name="lastUpdate">Date format must be 'aaaa-mm-dd'</param>
		/// <returns>Itris sqlFilter param</returns>
		public string GetApi3FilterDateLastUpdate(string lastUpdate)
		{
			string url = string.Empty;
			string sqlFilter = string.Format("FEC_ULT_ACT > '{0}'", lastUpdate);

			if (!string.IsNullOrEmpty(_claseItris))
				url = string.Format("http://{0}:{1}/v1/class/{2}?sqlFilter={3}&limit=-1", _server, _puerto, _claseItris, sqlFilter);

			return url;
		}

		public string PostUrl()
		{
			return string.Format("http://{0}:{1}/class", _server, _puerto);
		}

		public string GetApi3LoginUrl()
		{
			return string.Format("http://{0}:{1}/v1/auth", _server, _puerto);
		}

		public string LoginUrl()
		{
			return string.Format("http://{0}:{1}/Login", _server, _puerto);
		}

		public string LogOutUrl()
		{
			return string.Format("http://{0}:{1}/Logout", _server, _puerto);
		}
	}
}
