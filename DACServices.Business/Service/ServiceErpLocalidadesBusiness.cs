using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Service.Entities;
using DACServices.Entities.Vendor.Clases;
using DACServices.Entities.Vendor.Response;
using DACServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Service
{
    public class ServiceErpLocalidadesBusiness
    {
        private ServiceErpLocalidadesRepository serviceErpLocalidadesRepository = null;

        public ServiceErpLocalidadesBusiness()
        {
            serviceErpLocalidadesRepository = new ServiceErpLocalidadesRepository();
        }

        public void Create(ERP_LOCALIDADES localidad)
        {
            serviceErpLocalidadesRepository.Create(localidad);
        }

        public object Read(Func<ERP_LOCALIDADES, bool> predicado)
        {
            return serviceErpLocalidadesRepository.Read(predicado);
        }

        public object Read()
        {
            return serviceErpLocalidadesRepository.Read();
        }

        public object Update(ERP_LOCALIDADES localidad)
        {
            return serviceErpLocalidadesRepository.Update(localidad);
        }

        #region Actualización DB_DACS respecto de Itris bd
        public ServiceSyncErpLocalidadesEntity SynchronizeErpLocalidadesDACS(ItrisAuthenticateEntity authenticateEntity)
        {
            //Listas CUD en DB_DACS
            ServiceSyncErpLocalidadesEntity serviceSyncErpLocalidadesEntity = new ServiceSyncErpLocalidadesEntity();
            serviceSyncErpLocalidadesEntity.ListaCreate = new List<ERP_LOCALIDADES>();
            serviceSyncErpLocalidadesEntity.ListaUpdate = new List<ERP_LOCALIDADES>();
            serviceSyncErpLocalidadesEntity.ListaDelete = new List<ERP_LOCALIDADES>();

            try
            {
                List<ERP_LOCALIDADES> listaAsesoresItris = new List<ERP_LOCALIDADES>();

                ItrisErpLocalidadesBusiness itrisErpLocalidadesBusiness = new ItrisErpLocalidadesBusiness(authenticateEntity);
                ItrisErpLocalidadesResponse itrisErpLocalidadesResponse =
                    Task.Run(async () => await itrisErpLocalidadesBusiness.Get()).GetAwaiter().GetResult();

                List<ERP_LOCALIDADES> listaServiceLocalidades = this.Read() as List<ERP_LOCALIDADES>;

                //Comparo elemento por elemento para chequear los insert y actualizaciones
                foreach (var objItris in itrisErpLocalidadesResponse.data)
                {
                    var localidad = listaServiceLocalidades.Where(a => a.ID == objItris.ID).SingleOrDefault();
                    if (localidad != null)
                    {
                        if (!LocalidadesIguales(localidad, objItris))
                        {
                            ActualizoLocalidad(localidad, objItris);
                            serviceSyncErpLocalidadesEntity.ListaUpdate.Add(localidad);
                        }
                    }
                    else
                        serviceSyncErpLocalidadesEntity.ListaCreate.Add(CreoNuevaLocalidad(objItris));
                }

                //Obtengo los elementos que tengo que eliminar en la bd DACS
                foreach (var objService in listaServiceLocalidades)
                {
                    var objDelete = itrisErpLocalidadesResponse.data.Where(a => a.ID == objService.ID).SingleOrDefault();
                    if (objDelete == null)
                        serviceSyncErpLocalidadesEntity.ListaDelete.Add(objService);
                }

                PersistirListas(serviceSyncErpLocalidadesEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return serviceSyncErpLocalidadesEntity;
        }

        private void PersistirListas(ServiceSyncErpLocalidadesEntity serviceSyncErpLocalidadesEntity)
        {
            try
            {
                foreach (var obj in serviceSyncErpLocalidadesEntity.ListaCreate)
                    serviceErpLocalidadesRepository.Create(obj);

                foreach (var obj in serviceSyncErpLocalidadesEntity.ListaUpdate)
                    serviceErpLocalidadesRepository.Update(obj);

                foreach (var obj in serviceSyncErpLocalidadesEntity.ListaDelete)
                    serviceErpLocalidadesRepository.Delete(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool LocalidadesIguales(ERP_LOCALIDADES serviceLocalidad, ItrisErpLocalidadesEntity itrisLocalidad)
        {
			if (serviceLocalidad.ID == itrisLocalidad.ID &&
				serviceLocalidad.DESCRIPCION == itrisLocalidad.DESCRIPCION &&
					serviceLocalidad.FK_ERP_PARTIDOS == itrisLocalidad.FK_ERP_PARTIDOS &&
						serviceLocalidad.Z_FK_ERP_PARTIDOS == itrisLocalidad.Z_FK_ERP_PARTIDOS &&
							serviceLocalidad.FK_ERP_PROVINCIAS == itrisLocalidad.FK_ERP_PROVINCIAS &&
								serviceLocalidad.Z_FK_ERP_PROVINCIAS == itrisLocalidad.Z_FK_ERP_PROVINCIAS)
									return true;
			return false;
        }

        private void ActualizoLocalidad(ERP_LOCALIDADES serviceLocalidad, ItrisErpLocalidadesEntity itrisLocalidad)
        {
			serviceLocalidad.DESCRIPCION = itrisLocalidad.DESCRIPCION;
			serviceLocalidad.FK_ERP_PARTIDOS = itrisLocalidad.FK_ERP_PARTIDOS;
			serviceLocalidad.Z_FK_ERP_PARTIDOS = itrisLocalidad.Z_FK_ERP_PARTIDOS;
			serviceLocalidad.FK_ERP_PROVINCIAS = itrisLocalidad.FK_ERP_PROVINCIAS;
			serviceLocalidad.Z_FK_ERP_PROVINCIAS = itrisLocalidad.Z_FK_ERP_PROVINCIAS;
		}

        private ERP_LOCALIDADES CreoNuevaLocalidad(ItrisErpLocalidadesEntity itrisLocalidad)
        {
            ERP_LOCALIDADES nuevaLocalidad = new ERP_LOCALIDADES()
            {
                ID = itrisLocalidad.ID,
				DESCRIPCION = itrisLocalidad.DESCRIPCION,
				FK_ERP_PARTIDOS = itrisLocalidad.FK_ERP_PARTIDOS,
				Z_FK_ERP_PARTIDOS = itrisLocalidad.Z_FK_ERP_PARTIDOS,
				FK_ERP_PROVINCIAS = itrisLocalidad.FK_ERP_PROVINCIAS,
				Z_FK_ERP_PROVINCIAS = itrisLocalidad.Z_FK_ERP_PROVINCIAS
			};
            return nuevaLocalidad;
        }
        #endregion

        #region Reenvio registros actualizados a SQLite
        public ServiceSyncErpLocalidadesEntity SynchronizeSQLite(List<ERP_LOCALIDADES> listaLocalidadesSQLite)
        {
            //Listas CUD en DB_DACS
            ServiceSyncErpLocalidadesEntity serviceSyncErpLocalidadesEntity = new ServiceSyncErpLocalidadesEntity();
            serviceSyncErpLocalidadesEntity.ListaCreate = new List<ERP_LOCALIDADES>();
            serviceSyncErpLocalidadesEntity.ListaUpdate = new List<ERP_LOCALIDADES>();
            serviceSyncErpLocalidadesEntity.ListaDelete = new List<ERP_LOCALIDADES>();

            try
            {
                List<ERP_LOCALIDADES> listaServiceLocalidades = this.Read() as List<ERP_LOCALIDADES>;

                //Comparo elemento por elemento para chequear los insert y actualizaciones
                foreach (var objService in listaServiceLocalidades)
                {
                    var localidad = listaLocalidadesSQLite.Where(a => a.ID == objService.ID).SingleOrDefault();
                    if (localidad != null)
                    {
                        if (!LocalidadIguales(localidad, objService))
                        {
                            serviceSyncErpLocalidadesEntity.ListaUpdate.Add(objService);
                        }
                    }
                    else
                        serviceSyncErpLocalidadesEntity.ListaCreate.Add(objService);
                }

                //Obtengo los elementos que tengo que eliminar en la bd DACS
                foreach (var objSQLite in listaLocalidadesSQLite)
                {
                    var objDelete = listaServiceLocalidades.Where(a => a.ID == objSQLite.ID).SingleOrDefault();
                    if (objDelete == null)
                        serviceSyncErpLocalidadesEntity.ListaDelete.Add(objSQLite);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceSyncErpLocalidadesEntity;
        }

        public bool LocalidadIguales(ERP_LOCALIDADES localidadUno, ERP_LOCALIDADES localidadDos)
        {
			if (localidadUno.ID == localidadDos.ID &&
				localidadUno.DESCRIPCION == localidadDos.DESCRIPCION &&
					localidadUno.FK_ERP_PARTIDOS == localidadDos.FK_ERP_PARTIDOS &&
						localidadUno.Z_FK_ERP_PARTIDOS == localidadDos.Z_FK_ERP_PARTIDOS &&
							localidadUno.FK_ERP_PROVINCIAS == localidadDos.FK_ERP_PROVINCIAS &&
								localidadUno.Z_FK_ERP_PROVINCIAS == localidadDos.Z_FK_ERP_PROVINCIAS)
				return true;
			return false;
        }

        #endregion
    }
}
