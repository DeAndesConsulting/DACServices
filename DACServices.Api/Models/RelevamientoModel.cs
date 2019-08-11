using DACServices.Entities;
using DACServices.Entities.Vendor.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DACServices.Api.Models
{
	public class RelevamientoModel
	{
		public ItrisPlanillaEntity ItrisPlanilla;
		public string Imei;

		public RelevamientoModel()
		{
			ItrisPlanilla = new ItrisPlanillaEntity();
		}
	}
}