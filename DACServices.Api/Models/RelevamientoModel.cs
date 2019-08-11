using DACServices.Entities.Vendor.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DACServices.Api.Models
{
	public class RelevamientoModel
	{
		public ItrisRelevamientoEntity ItrisRelevamientoEntity;
		public ItrisComercioEntity ItrisComercioEntity;
		public ICollection<ItrisRelevamientoArticuloEntity> ListaRelevamientoArticulo;
	}
}