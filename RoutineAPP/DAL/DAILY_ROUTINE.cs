//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoutineAPP.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class DAILY_ROUTINE
    {
        public int dailyRoutineID { get; set; }
        public System.DateTime routineDate { get; set; }
        public string summary { get; set; }
        public int day { get; set; }
        public int monthID { get; set; }
        public int year { get; set; }
        public bool isDeleted { get; set; }
        public Nullable<System.DateTime> deletedDate { get; set; }
    }
}
