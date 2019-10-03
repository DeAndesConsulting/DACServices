using DACServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Repositories.Service
{
    public class ServiceErpLocalidadesRepository
    {
        private DB_DACSEntities _contexto = null;

        public ServiceErpLocalidadesRepository()
        {
            _contexto = new DB_DACSEntities();
        }

        public void Create(ERP_LOCALIDADES localidad)
        {
            try
            {
                var respuesta = _contexto.ERP_LOCALIDADES.Add(localidad);
                _contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object Read(Func<ERP_LOCALIDADES, bool> predicado)
        {
            try
            {
                return _contexto.ERP_LOCALIDADES.Where(predicado).ToList<ERP_LOCALIDADES>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object Read()
        {
            try
            {
                return _contexto.ERP_LOCALIDADES.ToList<ERP_LOCALIDADES>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object Update(ERP_LOCALIDADES localidadad)
        {
            try
            {
                var result = _contexto.ERP_LOCALIDADES.SingleOrDefault(x => x.ID == localidadad.ID);

                if (result != null)
                {
                    result.DESCRIPCION = localidadad.DESCRIPCION;
                    result.FK_ERP_PARTIDOS = localidadad.FK_ERP_PARTIDOS;
                    result.FK_ERP_PROVINCIAS = localidadad.FK_ERP_PROVINCIAS;
                    result.Z_FK_ERP_PARTIDOS = localidadad.Z_FK_ERP_PARTIDOS;
                    result.Z_FK_ERP_PROVINCIAS = localidadad.Z_FK_ERP_PROVINCIAS;
                    _contexto.SaveChanges();
                }
                else
                {
                    throw new Exception("No se encontro el objeto a actualizar");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object Delete(ERP_LOCALIDADES localidades)
        {
            try
            {
                var result = _contexto.ERP_LOCALIDADES.Remove(localidades);
                _contexto.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
