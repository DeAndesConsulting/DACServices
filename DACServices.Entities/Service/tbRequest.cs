//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DACServices.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbRequest
    {
        public int req_id { get; set; }
        public int req_id_mobile { get; set; }
        public System.DateTime req_fecha_request { get; set; }
        public Nullable<System.DateTime> req_fecha_response { get; set; }
        public string req_body_request { get; set; }
        public bool req_estado { get; set; }
        public string req_imei { get; set; }
    }
}