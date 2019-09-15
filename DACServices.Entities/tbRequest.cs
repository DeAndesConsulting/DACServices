using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DACServices.Entities
{
	[MetadataType(typeof(tbRequest_Metadata))]
	public partial class tbRequest
	{
	}

	public class tbRequest_Metadata
	{
		[DataMember(EmitDefaultValue = false)]
		public int req_id { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public System.DateTime req_fecha_request { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public Nullable<System.DateTime> req_fecha_response { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string req_body_request { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public bool req_estado { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string req_codigo { get; set; }
	}

}
