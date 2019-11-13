using DACServices.Business.Vendor;
using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using DACServices.Entities.Vendor.Request;
using DACServices.Repositories.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Business.Service
{
	public class ServiceRelevamientoBusiness
	{
		private ItrisRelevamientoBusiness _itrisRelevamientoBusiness;
		private ItrisComercioBusiness _itrisComercioBusiness;
		private ItrisRelevamientoArticuloBusiness _itrisRelevamientoArticuloBusiness;
		private ItrisEstadoEmailRelevamientoBusiness _itrisEstadoEmailRelevamientoBusiness;

		public ServiceRelevamientoBusiness(ItrisAuthenticateEntity itrisAuthenticateEntity)
		{
			_itrisRelevamientoBusiness = new ItrisRelevamientoBusiness(itrisAuthenticateEntity);
			_itrisComercioBusiness = new ItrisComercioBusiness(itrisAuthenticateEntity);
			_itrisRelevamientoArticuloBusiness = new ItrisRelevamientoArticuloBusiness(itrisAuthenticateEntity);
			_itrisEstadoEmailRelevamientoBusiness = new ItrisEstadoEmailRelevamientoBusiness(itrisAuthenticateEntity);
		}

		public void Post(ItrisPlanillaEntity planilla)
		{
            string stringSession = string.Empty;
			try
			{
                stringSession = _itrisRelevamientoBusiness.SessionString;

				#region Post Planilla Itris
				var resultItrisRelevamientoResponse =
					Task.Run(async () => await _itrisRelevamientoBusiness.Post(planilla.Relevamiento, stringSession)).GetAwaiter().GetResult();

				planilla.Relevamiento = resultItrisRelevamientoResponse.data.FirstOrDefault();
				#endregion

				#region Post Lista Comercios
				foreach (var comercioArticulos in planilla.Comercios)
				{
					#region Post Comercio Itris

					//Asigno los articulos al comercio maestro-detalle itris
					comercioArticulos.RelevamientoArticulo.ForEach(x => x.FK_RELEVAMIENTO = planilla.Relevamiento.ID);
					comercioArticulos.Comercio._DET_REL_ART = comercioArticulos.RelevamientoArticulo;
					//Asigno los articulos al comercio maestro-detalle itris

					var resultItrisComercioResponse =
						Task.Run(async () =>
							await _itrisComercioBusiness.Post(comercioArticulos.Comercio, stringSession)).GetAwaiter().GetResult();

					comercioArticulos.Comercio = resultItrisComercioResponse.data.FirstOrDefault();
					#endregion

					#region Post RelevamientoArticulo - OK
					//if (resultItrisRelevamientoResponse.data.FirstOrDefault().ID != 0 &&
					//		resultItrisComercioResponse.data.FirstOrDefault().ID != 0)
					//{
					//	var resultItrisRelevamientoArticuloResponse =
					//		Task.Run(async () => await _itrisRelevamientoArticuloBusiness.Post(
					//			resultItrisRelevamientoResponse.data.FirstOrDefault().ID,
					//			resultItrisComercioResponse.data.FirstOrDefault().ID,
					//			comercioArticulos.RelevamientoArticulo, stringSession)).GetAwaiter().GetResult();

					//	comercioArticulos.RelevamientoArticulo = resultItrisRelevamientoArticuloResponse.data;
					//}
					#endregion
				}
				#endregion

				#region Post Tabla Envio E-mails
				if (resultItrisRelevamientoResponse.data.FirstOrDefault().ID != 0)
				{
					ItrisEstadoEmailRelevamientoEntity itrisEstadoEmailRelevamientoEntity =
						new ItrisEstadoEmailRelevamientoEntity()
						{
							FK_RELEVAMIENTO = resultItrisRelevamientoResponse.data.FirstOrDefault().ID,
							ENVIADO_POR_MAIL = false
						};

					var resultEstadoEmailRelevamientoResponse = Task.Run(async () =>
						await _itrisEstadoEmailRelevamientoBusiness.Post(itrisEstadoEmailRelevamientoEntity)).GetAwaiter().GetResult();

					planilla.EstadoEmailRelevamiento = resultEstadoEmailRelevamientoResponse.data.FirstOrDefault();
				}
				#endregion

				//PROCESO DE ENVIAR LOS DATOS A ITRIS
			}
			catch (Exception ex)
			{
				throw ex;
			}
            finally
            {
                string mensaje = _itrisRelevamientoBusiness.CloseSession(stringSession);
            }
		}

		//public void PostOld(ItrisPlanillaEntity planilla)
		//{
		//	try
		//	{
		//		//PROCSO DE ENVIAR LOS DATOS A ITRIS
		//		#region Post Planilla - OK
		//		var resultItrisRelevamientoResponse =
		//			Task.Run(async () => await _itrisRelevamientoBusiness.Post(planilla.Relevamiento)).GetAwaiter().GetResult();

		//		planilla.Relevamiento = resultItrisRelevamientoResponse.data.FirstOrDefault();
		//		#endregion

		//		#region Post Comercio - OK
		//		var resultItrisComercioResponse =
		//			Task.Run(async () => await _itrisComercioBusiness.Post(planilla.Comercio)).GetAwaiter().GetResult();

		//		planilla.Comercio = resultItrisComercioResponse.data.FirstOrDefault();
		//		#endregion

		//		#region Post RelevamientoArticulo - OK
		//		if (resultItrisRelevamientoResponse.data.FirstOrDefault().ID != 0 &&
		//				resultItrisComercioResponse.data.FirstOrDefault().ID != 0)
		//		{
		//			var resultItrisRelevamientoArticuloResponse =
		//				Task.Run(async () => await _itrisRelevamientoArticuloBusiness.Post(
		//					resultItrisRelevamientoResponse.data.FirstOrDefault().ID,
		//					resultItrisComercioResponse.data.FirstOrDefault().ID,
		//					planilla.RelevamientoArticulo)).GetAwaiter().GetResult();

		//			planilla.RelevamientoArticulo = resultItrisRelevamientoArticuloResponse.data;
		//		}
		//		#endregion
		//		//PROCSO DE ENVIAR LOS DATOS A ITRIS
		//	}
		//	catch (Exception ex)
		//	{
		//		throw ex;
		//	}
		//}
	}
}
