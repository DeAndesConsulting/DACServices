﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities.Vendor.Clases
{
	[DataContract]
	public class ItrisRelevamientoEntity
	{
		[DataMember(EmitDefaultValue = false)]
		public int ID { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_EMPRESAS { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public int FK_ERP_ASESORES { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DateTime FECHA { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string CODIGO { get; set; }
	}
}
