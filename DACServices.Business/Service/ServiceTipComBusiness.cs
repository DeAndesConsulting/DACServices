using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Service;
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
	public class ServiceTipComBusiness
	{
		private ServiceTipComRepository serviceTipComRepository = null;

		public ServiceTipComBusiness()
		{
			serviceTipComRepository = new ServiceTipComRepository();
		}

		public void Create(TIP_COM tipCom)
		{
			serviceTipComRepository.Create(tipCom);
		}

		public object Read(Func<TIP_COM, bool> predicado)
		{
			return serviceTipComRepository.Read(predicado);
		}

		public object Read()
		{
			return serviceTipComRepository.Read();
		}

		public object Update(TIP_COM tipCom)
		{
			return serviceTipComRepository.Update(tipCom);
		}

		#region Actualización DB_DACS respecto de Itris bd
		public ServiceSyncTipComEntity SynchronizeTipComDACS(ItrisAuthenticateEntity authenticateEntity)
		{
			//Listas CUD en DB_DACS
			ServiceSyncTipComEntity serviceSyncTipComEntity = new ServiceSyncTipComEntity();
			serviceSyncTipComEntity.ListaCreate = new List<TIP_COM>();
			serviceSyncTipComEntity.ListaUpdate = new List<TIP_COM>();
			serviceSyncTipComEntity.ListaDelete = new List<TIP_COM>();

			try
			{
				List<TIP_COM> listaTipComItris = new List<TIP_COM>();

				ItrisTipoDeComercioBusiness itrisTipoDeComercioBusiness = new ItrisTipoDeComercioBusiness(authenticateEntity);
				ItrisTipoDeComercioResponse itrisTipoDeComercioResponse =
					Task.Run(async () => await itrisTipoDeComercioBusiness.Get()).GetAwaiter().GetResult();

				List<TIP_COM> listaServiceTipCom = this.Read() as List<TIP_COM>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objItris in itrisTipoDeComercioResponse.data)
				{
					var tipCom = listaServiceTipCom.Where(a => a.ID == objItris.ID).SingleOrDefault();
					if (tipCom != null)
					{
						if (!TipoComercioIguales(tipCom, objItris))
						{
							ActualizoTipCom(tipCom, objItris);
							serviceSyncTipComEntity.ListaUpdate.Add(tipCom);
						}
					}
					else
						serviceSyncTipComEntity.ListaCreate.Add(CreoNuevoTipCom(objItris));
				}

				//Obtengo los elementos que tengo que eliminar en la bd DACS
				foreach (var objService in listaServiceTipCom)
				{
					var objDelete = itrisTipoDeComercioResponse.data.Where(a => a.ID == objService.ID).SingleOrDefault();
					if (objDelete == null)
						serviceSyncTipComEntity.ListaDelete.Add(objService);
				}

				PersistirListas(serviceSyncTipComEntity);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return serviceSyncTipComEntity;
		}

		private void PersistirListas(ServiceSyncTipComEntity serviceSyncTipComEntity)
		{
			try
			{
				foreach (var obj in serviceSyncTipComEntity.ListaCreate)
					serviceTipComRepository.Create(obj);

				foreach (var obj in serviceSyncTipComEntity.ListaUpdate)
					serviceTipComRepository.Update(obj);

				foreach (var obj in serviceSyncTipComEntity.ListaDelete)
					serviceTipComRepository.Delete(obj);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private bool TipoComercioIguales(TIP_COM serviceTipCom, ItrisTipoDeComercioEntity itrisTipoDeComercio)
		{
			if (serviceTipCom.ID == itrisTipoDeComercio.ID &&
				serviceTipCom.DESCRIPCION == itrisTipoDeComercio.DESCRIPCION)
				return true;
			return false;
		}

		private void ActualizoTipCom(TIP_COM serviceTipCom, ItrisTipoDeComercioEntity itrisTipoDeComercio)
		{
			serviceTipCom.DESCRIPCION = itrisTipoDeComercio.DESCRIPCION;
		}

		private TIP_COM CreoNuevoTipCom(ItrisTipoDeComercioEntity itrisTipoDeComercio)
		{
			TIP_COM nuevoTipCom = new TIP_COM()
			{
				ID = itrisTipoDeComercio.ID,
				DESCRIPCION = itrisTipoDeComercio.DESCRIPCION
			};
			return nuevoTipCom;
		}
		#endregion

		#region Reenvio registros actualizados a SQLite
		public ServiceSyncTipComEntity SynchronizeSQLite(List<TIP_COM> listaTipComSQLite)
		{
			//Listas CUD en DB_DACS
			ServiceSyncTipComEntity serviceSyncTipComEntity = new ServiceSyncTipComEntity();
			serviceSyncTipComEntity.ListaCreate = new List<TIP_COM>();
			serviceSyncTipComEntity.ListaUpdate = new List<TIP_COM>();
			serviceSyncTipComEntity.ListaDelete = new List<TIP_COM>();

			try
			{
				List<TIP_COM> listaServiceTipCom = this.Read() as List<TIP_COM>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objService in listaServiceTipCom)
				{
					var tipCom = listaTipComSQLite.Where(a => a.ID == objService.ID).SingleOrDefault();
					if (tipCom != null)
					{
						if (!TipComIguales(tipCom, objService))
						{
							serviceSyncTipComEntity.ListaUpdate.Add(objService);
						}
					}
					else
						serviceSyncTipComEntity.ListaCreate.Add(objService);
				}

				//Obtengo los elementos que tengo que eliminar en la bd DACS
				foreach (var objSQLite in listaTipComSQLite)
				{
					var objDelete = listaServiceTipCom.Where(a => a.ID == objSQLite.ID).SingleOrDefault();
					if (objDelete == null)
						serviceSyncTipComEntity.ListaDelete.Add(objSQLite);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return serviceSyncTipComEntity;
		}

		public bool TipComIguales(TIP_COM tipComUno, TIP_COM tipComDos)
		{
			if (tipComUno.ID == tipComDos.ID &&
				tipComUno.DESCRIPCION == tipComDos.DESCRIPCION)
				return true;
			return false;
		}

		#endregion
	}
}
