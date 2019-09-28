using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Interfaces
{
    public interface IRepository<RQ, RP> 
		where RQ : class, new()
		where RP : class, new()
    {
		Task<RP> Get(string urlRequest);
		Task<RP> Post(string urlRequest, RQ request);
		Task<RP> Put(string urlRequest, RQ request);
		Task<RP> Delete(string urlRequest, RQ request);
		string OpenSession();
		string CloseSession(string session);
	}
}
