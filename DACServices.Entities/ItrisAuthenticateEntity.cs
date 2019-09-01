﻿using System;
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

		public string GetUrl()
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

		public string PostUrl()
		{
			return string.Format("http://{0}:{1}/class", _server, _puerto);
		}

		public string LoginUrl()
		{
			return string.Format("http://{0}:{1}/Login", _server, _puerto);
		}
	}
}