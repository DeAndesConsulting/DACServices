using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Service;
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
	public class ServiceErpAsesoresBusiness
	{
		private ServiceErpAsesoresRepository serviceErpAsesoresRepository = null;

		public ServiceErpAsesoresBusiness()
		{
			serviceErpAsesoresRepository = new ServiceErpAsesoresRepository();
		}

		public void Create(ERP_ASESORES asesor)
		{
			serviceErpAsesoresRepository.Create(asesor);
		}

		public object Read(Func<ERP_ASESORES, bool> predicado)
		{
			return serviceErpAsesoresRepository.Read(predicado);
		}

		public object Read()
		{
			return serviceErpAsesoresRepository.Read();
		}

		public object Update(ERP_ASESORES asesor)
		{
			return serviceErpAsesoresRepository.Update(asesor);
		}

		#region Actualización DB_DACS respecto de Itris bd
		public ServiceSyncErpAsesoresEntity SynchronizeErpAsesoresDACS(ItrisAuthenticateEntity authenticateEntity, string lastUpdate)
		{
			//Listas CUD en DB_DACS
			ServiceSyncErpAsesoresEntity serviceSyncErpAsesoresEntity = new ServiceSyncErpAsesoresEntity();
			serviceSyncErpAsesoresEntity.ListaCreate = new List<ERP_ASESORES>();
			serviceSyncErpAsesoresEntity.ListaUpdate = new List<ERP_ASESORES>();
			serviceSyncErpAsesoresEntity.ListaDelete = new List<ERP_ASESORES>();

			try
			{
				List<ERP_ASESORES> listaAsesoresItris = new List<ERP_ASESORES>();

				ItrisErpAsesoresBusiness itrisErpAsesoresBusiness = new ItrisErpAsesoresBusiness(authenticateEntity);
				//ItrisErpAsesoresResponse itrisErpAsesoresResponse =
				//	Task.Run(async () => await itrisErpAsesoresBusiness.Get()).GetAwaiter().GetResult();
				ItrisErpAsesoresResponse itrisErpAsesoresResponse =
					Task.Run(async () => await itrisErpAsesoresBusiness.GetLastUpdate(lastUpdate)).GetAwaiter().GetResult();

				List<ERP_ASESORES> listaServiceAsesores = this.Read() as List<ERP_ASESORES>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objItris in itrisErpAsesoresResponse.data)
				{
					var asesor = listaServiceAsesores.Where(a => a.ID == objItris.ID).SingleOrDefault();
					if (asesor != null)
					{
						if (!AsesoresIguales(asesor, objItris))
						{
							ActualizoAsesor(asesor, objItris);
							serviceSyncErpAsesoresEntity.ListaUpdate.Add(asesor);
						}
					}
					else
						serviceSyncErpAsesoresEntity.ListaCreate.Add(CreoNuevoAsesor(objItris));
				}

				//No elimino mas porque solo cuento con los ultimos por fecha de actualización
				//Obtengo los elementos que tengo que eliminar en la bd DACS
				//foreach (var objService in listaServiceAsesores)
				//{
				//	var objDelete = itrisErpAsesoresResponse.data.Where(a => a.ID == objService.ID).SingleOrDefault();
				//	if (objDelete == null)
				//		serviceSyncErpAsesoresEntity.ListaDelete.Add(objService);
				//}

				PersistirListas(serviceSyncErpAsesoresEntity);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return serviceSyncErpAsesoresEntity;
		}

		private void PersistirListas(ServiceSyncErpAsesoresEntity serviceSyncErpAsesoresEntity)
		{
			try
			{
				foreach (var obj in serviceSyncErpAsesoresEntity.ListaCreate)
					serviceErpAsesoresRepository.Create(obj);

				foreach (var obj in serviceSyncErpAsesoresEntity.ListaUpdate)
					serviceErpAsesoresRepository.Update(obj);

				foreach (var obj in serviceSyncErpAsesoresEntity.ListaDelete)
					serviceErpAsesoresRepository.Delete(obj);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private bool AsesoresIguales(ERP_ASESORES serviceAsesor, ItrisErpAsesoresEntity itrisAsesor)
		{
			if (serviceAsesor.ID == itrisAsesor.ID &&
				serviceAsesor.DESCRIPCION == itrisAsesor.DESCRIPCION &&
					serviceAsesor.C_IMEI == itrisAsesor._IMEI &&
						serviceAsesor.C_IMEI_ADMIN == itrisAsesor._IMEI_ADMIN &&
							serviceAsesor.C_EMAIL == itrisAsesor._EMAIL)
				return true;
			return false;
		}

		private void ActualizoAsesor(ERP_ASESORES serviceAsesor, ItrisErpAsesoresEntity itrisAsesor)
		{
			serviceAsesor.DESCRIPCION = itrisAsesor.DESCRIPCION;
			serviceAsesor.C_EMAIL = itrisAsesor._EMAIL;
			serviceAsesor.C_IMEI = itrisAsesor._IMEI;
			serviceAsesor.C_IMEI_ADMIN = itrisAsesor._IMEI_ADMIN;
		}

		private ERP_ASESORES CreoNuevoAsesor(ItrisErpAsesoresEntity itrisAsesor)
		{
			ERP_ASESORES nuevoAsesor = new ERP_ASESORES()
			{
				ID = itrisAsesor.ID,
				DESCRIPCION = itrisAsesor.DESCRIPCION,
				C_EMAIL = itrisAsesor._EMAIL,
				C_IMEI = itrisAsesor._IMEI,
				C_IMEI_ADMIN = itrisAsesor._IMEI_ADMIN
			};
			return nuevoAsesor;
		}
		#endregion

		#region Reenvio registros actualizados a SQLite
		public ServiceSyncErpAsesoresEntity SynchronizeSQLite(List<ERP_ASESORES> listaAsesoresSQLite)
		{
			//Listas CUD en DB_DACS
			ServiceSyncErpAsesoresEntity serviceSyncErpAsesoresEntity = new ServiceSyncErpAsesoresEntity();
			serviceSyncErpAsesoresEntity.ListaCreate = new List<ERP_ASESORES>();
			serviceSyncErpAsesoresEntity.ListaUpdate = new List<ERP_ASESORES>();
			serviceSyncErpAsesoresEntity.ListaDelete = new List<ERP_ASESORES>();

			try
			{
				List<ERP_ASESORES> listaServiceAsesores = this.Read() as List<ERP_ASESORES>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objService in listaServiceAsesores)
				{
					var asesor = listaAsesoresSQLite.Where(a => a.ID == objService.ID).SingleOrDefault();
					if (asesor != null)
					{
						if (!AsesoresIguales(asesor, objService))
						{
							serviceSyncErpAsesoresEntity.ListaUpdate.Add(objService);
						}
					}
					else
						serviceSyncErpAsesoresEntity.ListaCreate.Add(objService);
				}

				//Obtengo los elementos que tengo que eliminar en la bd DACS
				foreach (var objSQLite in listaAsesoresSQLite)
				{
					var objDelete = listaServiceAsesores.Where(a => a.ID == objSQLite.ID).SingleOrDefault();
					if (objDelete == null)
						serviceSyncErpAsesoresEntity.ListaDelete.Add(objSQLite);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return serviceSyncErpAsesoresEntity;
		}

		public bool AsesoresIguales(ERP_ASESORES asesorUno, ERP_ASESORES asesorDos)
		{
			if (asesorUno.ID == asesorDos.ID &&
				asesorUno.DESCRIPCION == asesorDos.DESCRIPCION &&
					asesorUno.C_EMAIL == asesorDos.C_EMAIL &&
						asesorUno.C_IMEI == asesorDos.C_IMEI &&
							asesorUno.C_IMEI_ADMIN == asesorDos.C_IMEI_ADMIN)
				return true;
			return false;
		}

		#endregion
	}
}
