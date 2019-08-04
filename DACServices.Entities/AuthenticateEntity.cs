using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities
{
	public class AuthenticateEntity
	{
		public string username { get; protected set; }
		public string password { get; protected set; }
		public string database { get; protected set; }
		public string urlAuthenticate { get; protected set; }

		//public AuthenticateEntity() { }
		public AuthenticateEntity(string _username, string _password, string _database, string _urlAuthenticate)
		{
			username = _username;
			password = _password;
			database = _database;
			urlAuthenticate = _urlAuthenticate;
		}
	}
}
