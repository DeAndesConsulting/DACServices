using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Service.Entities;
using DACServices.Entities.Vendor.Clases;
using DACServices.Entities.Vendor.Response;
using DACServices.Repositories.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Service
{
    public class ServiceArticuloBusiness
    {
        private ServiceArticuloRepository serviceArticuloRepository = null;

        public ServiceArticuloBusiness()
        {
            serviceArticuloRepository = new ServiceArticuloRepository();
        }

        public void Create(ARTICULO articulo)
        {
            serviceArticuloRepository.Create(articulo);
        }

        public object Read(Func<ARTICULO, bool> predicado)
        {
            return serviceArticuloRepository.Read(predicado);
        }

        public object Read()
        {
            return serviceArticuloRepository.Read();
        }

        public object Update(ARTICULO articulo)
        {
            return serviceArticuloRepository.Update(articulo);
        }

        #region Actualización DB_DACS respecto de Itris bd
        public ServiceSyncArticuloEntity SynchronizeArticuloDACS(ItrisAuthenticateEntity authenticateEntity, string lastUpdate)
        {
            //Listas CUD en DB_DACS
            ServiceSyncArticuloEntity serviceArticuloEntity = new ServiceSyncArticuloEntity();
            serviceArticuloEntity.ListaCreate = new List<ARTICULO>();
            serviceArticuloEntity.ListaUpdate = new List<ARTICULO>();
            serviceArticuloEntity.ListaDelete = new List<ARTICULO>();

            try
            {
                List<ARTICULO> listaArticulosItris = new List<ARTICULO>();

                ItrisArticuloBusiness itrisArticuloBusiness = new ItrisArticuloBusiness(authenticateEntity);
                ItrisArticuloResponse itrisArticuloResponse =
                    Task.Run(async () => await itrisArticuloBusiness.GetLastUpdate(lastUpdate)).GetAwaiter().GetResult();

                List<ARTICULO> listaServiceArticulo = this.Read() as List<ARTICULO>;

                //Comparo elemento por elemento para chequear los insert y actualizaciones
                foreach (var objItris in itrisArticuloResponse.data)
                {
                    var articulo = listaServiceArticulo.Where(a => a.ID == objItris.ID).SingleOrDefault();
                    if (articulo != null)
                    {
                        if (!ArticulosIguales(articulo, objItris))
                        {
                            ActualizoArticulo(articulo, objItris);
                            serviceArticuloEntity.ListaUpdate.Add(articulo);
                        }
                    }
                    else
                        serviceArticuloEntity.ListaCreate.Add(CreoNuevoArticulo(objItris));
                }

                //Obtengo los elementos que tengo que eliminar en la bd DACS
                //foreach (var objService in listaServiceArticulo)
                //{
                //    var objDelete = itrisArticuloResponse.data.Where(a => a.ID == objService.ID).SingleOrDefault();
                //    if (objDelete == null)
                //        serviceArticuloEntity.ListaDelete.Add(objService);
                //}

                PersistirListas(serviceArticuloEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return serviceArticuloEntity;
        }

        private void PersistirListas(ServiceSyncArticuloEntity serviceSyncArticuloEntity)
        {
            try
            {
                foreach (var obj in serviceSyncArticuloEntity.ListaCreate)
                    serviceArticuloRepository.Create(obj);

                foreach (var obj in serviceSyncArticuloEntity.ListaUpdate)
                    serviceArticuloRepository.Update(obj);

                foreach (var obj in serviceSyncArticuloEntity.ListaDelete)
                    serviceArticuloRepository.Delete(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ArticulosIguales(ARTICULO serviceArticulo, ItrisArticuloEntity itrisArticulo)
        {
            if (serviceArticulo.ID == itrisArticulo.ID &&
                serviceArticulo.DESCRIPCION == itrisArticulo.DESCRIPCION &&
                    serviceArticulo.FK_TIP_ART == itrisArticulo.FK_TIP_ART &&
						serviceArticulo.Z_FK_TIP_ART == itrisArticulo.Z_FK_TIP_ART &&
							serviceArticulo.ARTICULO_PROPIO == itrisArticulo.ARTICULO_PROPIO)
				return true;
            return false;
        }

        private void ActualizoArticulo(ARTICULO serviceArticulo, ItrisArticuloEntity itrisArticulo)
        {
            serviceArticulo.DESCRIPCION = itrisArticulo.DESCRIPCION;
            serviceArticulo.FK_TIP_ART = itrisArticulo.FK_TIP_ART;
            serviceArticulo.Z_FK_TIP_ART = itrisArticulo.Z_FK_TIP_ART;
            serviceArticulo.ARTICULO_PROPIO = itrisArticulo.ARTICULO_PROPIO;
		}

        private ARTICULO CreoNuevoArticulo(ItrisArticuloEntity itrisArticulo)
        {
            ARTICULO nuevoArticulo = new ARTICULO()
            {
                ID = itrisArticulo.ID,
                DESCRIPCION = itrisArticulo.DESCRIPCION,
                FK_TIP_ART = itrisArticulo.FK_TIP_ART,
				Z_FK_TIP_ART = itrisArticulo.Z_FK_TIP_ART,
				ARTICULO_PROPIO = itrisArticulo.ARTICULO_PROPIO
			};
            return nuevoArticulo;
        }
        #endregion

        #region Reenvio registros actualizados a SQLite
        public ServiceSyncArticuloEntity SynchronizeSQLite(List<ARTICULO> listaArticulosSQLite)
        {
            //Listas CUD en DB_DACS
            ServiceSyncArticuloEntity serviceSyncArticuloEntity = new ServiceSyncArticuloEntity();
            serviceSyncArticuloEntity.ListaCreate = new List<ARTICULO>();
            serviceSyncArticuloEntity.ListaUpdate = new List<ARTICULO>();
            serviceSyncArticuloEntity.ListaDelete = new List<ARTICULO>();

            try
            {
                List<ARTICULO> listaServiceArticulo = this.Read() as List<ARTICULO>;

                //Comparo elemento por elemento para chequear los insert y actualizaciones
                foreach (var objService in listaServiceArticulo)
                {
                    var articulo = listaArticulosSQLite.Where(a => a.ID == objService.ID).SingleOrDefault();
                    if (articulo != null)
                    {
                        if (!ArticulosIguales(articulo, objService))
                        {
                            serviceSyncArticuloEntity.ListaUpdate.Add(objService);
                        }
                    }
                    else
                        serviceSyncArticuloEntity.ListaCreate.Add(objService);
                }

                //Obtengo los elementos que tengo que eliminar en la bd DACS
                foreach (var objSQLite in listaArticulosSQLite)
                {
                    var objDelete = listaServiceArticulo.Where(a => a.ID == objSQLite.ID).SingleOrDefault();
                    if (objDelete == null)
                        serviceSyncArticuloEntity.ListaDelete.Add(objSQLite);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceSyncArticuloEntity;
        }

        public bool ArticulosIguales(ARTICULO articuloUno, ARTICULO articuloDos)
        {
            if (articuloUno.ID == articuloDos.ID &&
                articuloUno.DESCRIPCION == articuloDos.DESCRIPCION &&
                    articuloUno.FK_TIP_ART == articuloDos.FK_TIP_ART &&
						articuloUno.Z_FK_TIP_ART == articuloDos.Z_FK_TIP_ART &&
							articuloUno.ARTICULO_PROPIO == articuloDos.ARTICULO_PROPIO)
                return true;
            return false;
        }

        #endregion
    }
}
