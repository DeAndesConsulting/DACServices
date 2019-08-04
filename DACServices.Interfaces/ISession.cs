using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Interfaces
{
    public interface ISession<RQ>
        where RQ : class, new()
    {
        string sessionString();
        void ExecuteGetSession(string urlAuthentication, RQ request);
        void UpdateSession();
    }
}
