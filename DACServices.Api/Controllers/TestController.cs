using DACServices.Api.Helpers.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DACServices.Api.Controllers
{
	[Authorize]
	public class TestController : ApiController
    {
		//https://www.c-sharpcorner.com/article/asp-net-mvc-oauth-2-0-rest-web-api-authorization-using-database-first-approach/
		[Authorize(Roles = CustomRoles.Admin)]
		public IEnumerable<string> Get()
        {
			//log.Debug("Debug log");
			//log.Info("Info log");
			//log.Warn("Warn log");
			//log.Error("Error log");
			//log.Fatal("Fatal log");
			return new string[] { "asd", "qwe", "zxc" };
        }

		[Authorize(Roles = CustomRoles.Usuario)]
		public string Get(int id)
        {
            return "El id enviado es: " + id.ToString();
        }

        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
