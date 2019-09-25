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
	public class ServiceTipArtBusiness
	{
		private ServiceTipArtRepository serviceTipArtRepository = null;

		public ServiceTipArtBusiness()
		{
			serviceTipArtRepository = new ServiceTipArtRepository();
		}

		public void Create(TIP_ART tipArt)
		{
			serviceTipArtRepository.Create(tipArt);
		}

		public object Read(Func<TIP_ART, bool> predicado)
		{
			return serviceTipArtRepository.Read(predicado);
		}

		public object Read()
		{
			return serviceTipArtRepository.Read();
		}

		public object Update(TIP_ART tipArt)
		{
			return serviceTipArtRepository.Update(tipArt);
		}

		#region Actualización DB_DACS respecto de Itris bd
		public ServiceSyncTipArtEntity SynchronizeTipArtDACS(ItrisAuthenticateEntity authenticateEntity)
		{
			//Listas CUD en DB_DACS
			ServiceSyncTipArtEntity serviceSyncTipArtEntity = new ServiceSyncTipArtEntity();
			serviceSyncTipArtEntity.ListaCreate = new List<TIP_ART>();
			serviceSyncTipArtEntity.ListaUpdate = new List<TIP_ART>();
			serviceSyncTipArtEntity.ListaDelete = new List<TIP_ART>();

			try
			{
				List<TIP_ART> listaTipArtItris = new List<TIP_ART>();

				ItrisTipoDeArticuloBusiness itrisTipoDeArticuloBusiness = new ItrisTipoDeArticuloBusiness(authenticateEntity);
				ItrisTipoDeArticuloResponse itrisTipoDeArticuloResponse =
					Task.Run(async () => await itrisTipoDeArticuloBusiness.Get()).GetAwaiter().GetResult();

				List<TIP_ART> listaServiceTipArt = this.Read() as List<TIP_ART>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objItris in itrisTipoDeArticuloResponse.data)
				{
					var tipArt = listaServiceTipArt.Where(a => a.ID == objItris.ID).SingleOrDefault();
					if (tipArt != null)
					{
						if (!TipoDeArticuloIguales(tipArt, objItris))
						{
							ActualizoTipArt(tipArt, objItris);
							serviceSyncTipArtEntity.ListaUpdate.Add(tipArt);
						}
					}
					else
						serviceSyncTipArtEntity.ListaCreate.Add(CreoNuevoTipArt(objItris));
				}

				//Obtengo los elementos que tengo que eliminar en la bd DACS
				foreach (var objService in listaServiceTipArt)
				{
					var objDelete = itrisTipoDeArticuloResponse.data.Where(a => a.ID == objService.ID).SingleOrDefault();
					if (objDelete == null)
						serviceSyncTipArtEntity.ListaDelete.Add(objService);
				}

				PersistirListas(serviceSyncTipArtEntity);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return serviceSyncTipArtEntity;
		}

		private void PersistirListas(ServiceSyncTipArtEntity serviceSyncTipArtEntity)
		{
			try
			{
				foreach (var obj in serviceSyncTipArtEntity.ListaCreate)
					serviceTipArtRepository.Create(obj);

				foreach (var obj in serviceSyncTipArtEntity.ListaUpdate)
					serviceTipArtRepository.Update(obj);

				foreach (var obj in serviceSyncTipArtEntity.ListaDelete)
					serviceTipArtRepository.Delete(obj);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private bool TipoDeArticuloIguales(TIP_ART serviceTipArt, ItrisTipoDeArticuloEntity itrisTipoDeArticulo)
		{
			if (serviceTipArt.ID == itrisTipoDeArticulo.ID &&
				serviceTipArt.DESCRIPCION == itrisTipoDeArticulo.DESCRIPCION)
				return true;
			return false;
		}

		private void ActualizoTipArt(TIP_ART serviceTipArt, ItrisTipoDeArticuloEntity itrisTipoDeArticulo)
		{
			serviceTipArt.DESCRIPCION = itrisTipoDeArticulo.DESCRIPCION;
		}

		private TIP_ART CreoNuevoTipArt(ItrisTipoDeArticuloEntity itrisTipoDeArticulo)
		{
			TIP_ART nuevoTipArt = new TIP_ART()
			{
				ID = itrisTipoDeArticulo.ID,
				DESCRIPCION = itrisTipoDeArticulo.DESCRIPCION
			};
			return nuevoTipArt;
		}
		#endregion

		#region Reenvio registros actualizados a SQLite
		public ServiceSyncTipArtEntity SynchronizeSQLite(List<TIP_ART> listaTipArtSQLite)
		{
			//Listas CUD en DB_DACS
			ServiceSyncTipArtEntity serviceSyncTipArtEntity = new ServiceSyncTipArtEntity();
			serviceSyncTipArtEntity.ListaCreate = new List<TIP_ART>();
			serviceSyncTipArtEntity.ListaUpdate = new List<TIP_ART>();
			serviceSyncTipArtEntity.ListaDelete = new List<TIP_ART>();

			try
			{
				List<TIP_ART> listaServiceTipArt = this.Read() as List<TIP_ART>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objService in listaServiceTipArt)
				{
					var asesor = listaTipArtSQLite.Where(a => a.ID == objService.ID).SingleOrDefault();
					if (asesor != null)
					{
						if (!TipArtIguales(asesor, objService))
						{
							serviceSyncTipArtEntity.ListaUpdate.Add(objService);
						}
					}
					else
						serviceSyncTipArtEntity.ListaCreate.Add(objService);
				}

				//Obtengo los elementos que tengo que eliminar en la bd DACS
				foreach (var objSQLite in listaTipArtSQLite)
				{
					var objDelete = listaServiceTipArt.Where(a => a.ID == objSQLite.ID).SingleOrDefault();
					if (objDelete == null)
						serviceSyncTipArtEntity.ListaDelete.Add(objSQLite);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return serviceSyncTipArtEntity;
		}

		public bool TipArtIguales(TIP_ART tipArtUno, TIP_ART tipArtDos)
		{
			if (tipArtUno.ID == tipArtDos.ID &&
				tipArtUno.DESCRIPCION == tipArtDos.DESCRIPCION)
				return true;
			return false;
		}

		#endregion
	}
}
