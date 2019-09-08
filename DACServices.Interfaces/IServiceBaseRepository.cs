using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Interfaces
{
	public interface IServiceBaseRepository<E> where E: class, new()
	{
		E Create(E entity);
		E Update(E entity);
		bool Delete(E entity);
		object Get();
		object GetByFilter(Func<E, bool> predicate);
	}
}
