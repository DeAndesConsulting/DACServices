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
	public class ServiceErpEmpresasBusiness
	{
		private ServiceErpEmpresasRepository serviceErpEmpresasRepository = null;

		public ServiceErpEmpresasBusiness()
		{
			serviceErpEmpresasRepository = new ServiceErpEmpresasRepository();
		}

		public void Create(ERP_EMPRESAS empresa)
		{
			serviceErpEmpresasRepository.Create(empresa);
		}

		public object Read(Func<ERP_EMPRESAS, bool> predicado)
		{
			return serviceErpEmpresasRepository.Read(predicado);
		}

		public object Read()
		{
			return serviceErpEmpresasRepository.Read();
		}

		public object Update(ERP_EMPRESAS empresa)
		{
			return serviceErpEmpresasRepository.Update(empresa);
		}

		#region Actualización DB_DACS respecto de Itris bd
		public ServiceSyncErpEmpresasEntity SynchronizeErpEmpresasDACS(ItrisAuthenticateEntity authenticateEntity, string lastUpdate, string token)
		{
			//Listas CUD en DB_DACS
			ServiceSyncErpEmpresasEntity serviceSyncErpEmpresasEntity = new ServiceSyncErpEmpresasEntity();
			serviceSyncErpEmpresasEntity.ListaCreate = new List<ERP_EMPRESAS>();
			serviceSyncErpEmpresasEntity.ListaUpdate = new List<ERP_EMPRESAS>();
			serviceSyncErpEmpresasEntity.ListaDelete = new List<ERP_EMPRESAS>();

			try
			{
				List<ERP_EMPRESAS> listaEmpresasItris = new List<ERP_EMPRESAS>();

				ItrisErpEmpresasBusiness itrisErpEmpresasBusiness = new ItrisErpEmpresasBusiness(authenticateEntity);
				ItrisErpEmpresasResponse itrisErpEmpresasResponse =
					Task.Run(async () => await itrisErpEmpresasBusiness.GetLastUpdate(lastUpdate, token)).GetAwaiter().GetResult();

				List<ERP_EMPRESAS> listaServiceEmpresas = this.Read() as List<ERP_EMPRESAS>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objItris in itrisErpEmpresasResponse.data)
				{
					var empresa = listaServiceEmpresas.Where(a => a.ID == objItris.ID).SingleOrDefault();
					if (empresa != null)
					{
						if (!EmpresasIguales(empresa, objItris))
						{
							ActualizoEmpresa(empresa, objItris);
							serviceSyncErpEmpresasEntity.ListaUpdate.Add(empresa);
						}
					}
					else
						serviceSyncErpEmpresasEntity.ListaCreate.Add(CreoNuevaEmpresa(objItris));
				}

				//Obtengo los elementos que tengo que eliminar en la bd DACS
				//foreach (var objService in listaServiceEmpresas)
				//{
				//	var objDelete = itrisErpEmpresasResponse.data.Where(a => a.ID == objService.ID).SingleOrDefault();
				//	if (objDelete == null)
				//		serviceSyncErpEmpresasEntity.ListaDelete.Add(objService);
				//}

				PersistirListas(serviceSyncErpEmpresasEntity);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return serviceSyncErpEmpresasEntity;
		}

		private void PersistirListas(ServiceSyncErpEmpresasEntity serviceSyncErpEmpresasEntity)
		{
			try
			{
				foreach (var obj in serviceSyncErpEmpresasEntity.ListaCreate)
					serviceErpEmpresasRepository.Create(obj);

				foreach (var obj in serviceSyncErpEmpresasEntity.ListaUpdate)
					serviceErpEmpresasRepository.Update(obj);

				foreach (var obj in serviceSyncErpEmpresasEntity.ListaDelete)
					serviceErpEmpresasRepository.Delete(obj);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private bool EmpresasIguales(ERP_EMPRESAS serviceEmpresa, ItrisErpEmpresasEntity itrisEmpresa)
		{
			if (serviceEmpresa.ID == itrisEmpresa.ID &&
				serviceEmpresa.NOM_FANTASIA == itrisEmpresa.NOM_FANTASIA &&
					serviceEmpresa.Z_FK_ERP_LOCALIDADES == itrisEmpresa.Z_FK_ERP_LOCALIDADES &&
						serviceEmpresa.Z_FK_ERP_PARTIDOS == itrisEmpresa.Z_FK_ERP_PARTIDOS &&
							serviceEmpresa.Z_FK_ERP_PROVINCIAS == itrisEmpresa.Z_FK_ERP_PROVINCIAS &&
								serviceEmpresa.FK_ERP_ASESORES == itrisEmpresa.FK_ERP_ASESORES &&
									serviceEmpresa.FK_ERP_ASESORES2 == itrisEmpresa.FK_ERP_ASESORES2 &&
										serviceEmpresa.FK_ERP_ASESORES3 == itrisEmpresa.FK_ERP_ASESORES3)
				return true;

			return false;
		}

		private void ActualizoEmpresa(ERP_EMPRESAS serviceEmpresa, ItrisErpEmpresasEntity itrisEmpresa)
		{
			serviceEmpresa.NOM_FANTASIA = itrisEmpresa.NOM_FANTASIA;
			serviceEmpresa.Z_FK_ERP_LOCALIDADES = itrisEmpresa.Z_FK_ERP_LOCALIDADES;
			serviceEmpresa.Z_FK_ERP_PARTIDOS = itrisEmpresa.Z_FK_ERP_PARTIDOS;
			serviceEmpresa.Z_FK_ERP_PROVINCIAS = itrisEmpresa.Z_FK_ERP_PROVINCIAS;
			serviceEmpresa.FK_ERP_ASESORES = itrisEmpresa.FK_ERP_ASESORES;
			serviceEmpresa.FK_ERP_ASESORES2 = itrisEmpresa.FK_ERP_ASESORES2;
			serviceEmpresa.FK_ERP_ASESORES3 = itrisEmpresa.FK_ERP_ASESORES3;
		}

		private ERP_EMPRESAS CreoNuevaEmpresa(ItrisErpEmpresasEntity itrisEmpresa)
		{
			ERP_EMPRESAS nuevaEmpresa = new ERP_EMPRESAS()
			{
				ID = itrisEmpresa.ID,
				NOM_FANTASIA = itrisEmpresa.NOM_FANTASIA,
				Z_FK_ERP_LOCALIDADES = itrisEmpresa.Z_FK_ERP_LOCALIDADES,
				Z_FK_ERP_PARTIDOS = itrisEmpresa.Z_FK_ERP_PARTIDOS,
				Z_FK_ERP_PROVINCIAS = itrisEmpresa.Z_FK_ERP_PROVINCIAS,
				FK_ERP_ASESORES = itrisEmpresa.FK_ERP_ASESORES,
				FK_ERP_ASESORES2 = itrisEmpresa.FK_ERP_ASESORES2,
				FK_ERP_ASESORES3 = itrisEmpresa.FK_ERP_ASESORES3
			};
			return nuevaEmpresa;
		}
		#endregion

		#region Reenvio registros actualizados a SQLite
		public ServiceSyncErpEmpresasEntity SynchronizeSQLite(List<ERP_EMPRESAS> listaEmpresasSQLite)
		{
			//Listas CUD en DB_DACS
			ServiceSyncErpEmpresasEntity serviceSyncErpEmpresasEntity = new ServiceSyncErpEmpresasEntity();
			serviceSyncErpEmpresasEntity.ListaCreate = new List<ERP_EMPRESAS>();
			serviceSyncErpEmpresasEntity.ListaUpdate = new List<ERP_EMPRESAS>();
			serviceSyncErpEmpresasEntity.ListaDelete = new List<ERP_EMPRESAS>();

			try
			{
				List<ERP_EMPRESAS> listaServiceEmpresas = this.Read() as List<ERP_EMPRESAS>;

				//Comparo elemento por elemento para chequear los insert y actualizaciones
				foreach (var objService in listaServiceEmpresas)
				{
					var empresa = listaEmpresasSQLite.Where(a => a.ID == objService.ID).SingleOrDefault();
					if (empresa != null)
					{
						if (!EmpresasIguales(empresa, objService))
						{
							serviceSyncErpEmpresasEntity.ListaUpdate.Add(objService);
						}
					}
					else
						serviceSyncErpEmpresasEntity.ListaCreate.Add(objService);
				}

				//Obtengo los elementos que tengo que eliminar en la bd DACS
				foreach (var objSQLite in listaEmpresasSQLite)
				{
					var objDelete = listaServiceEmpresas.Where(a => a.ID == objSQLite.ID).SingleOrDefault();
					if (objDelete == null)
						serviceSyncErpEmpresasEntity.ListaDelete.Add(objSQLite);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return serviceSyncErpEmpresasEntity;
		}

		public bool EmpresasIguales(ERP_EMPRESAS empresaUno, ERP_EMPRESAS empresaDos)
		{
			if (empresaUno.ID == empresaDos.ID &&
				empresaUno.FK_ERP_ASESORES == empresaDos.FK_ERP_ASESORES &&
					empresaUno.FK_ERP_ASESORES2 == empresaDos.FK_ERP_ASESORES2 &&
						empresaUno.FK_ERP_ASESORES3 == empresaDos.FK_ERP_ASESORES3 &&
							empresaUno.NOM_FANTASIA == empresaDos.NOM_FANTASIA &&
								empresaUno.Z_FK_ERP_LOCALIDADES == empresaDos.Z_FK_ERP_LOCALIDADES &&
									empresaUno.Z_FK_ERP_PARTIDOS == empresaDos.Z_FK_ERP_PARTIDOS &&
										empresaUno.Z_FK_ERP_PROVINCIAS == empresaDos.Z_FK_ERP_PROVINCIAS)
				return true;
			return false;
		}

		#endregion
	}
}
