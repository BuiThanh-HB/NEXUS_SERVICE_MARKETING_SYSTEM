//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.CustomerServicePlans = new HashSet<CustomerServicePlan>();
        }
    
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ServicePlanID { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public int Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int VillageID { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerServicePlan> CustomerServicePlans { get; set; }
        public virtual District District { get; set; }
        public virtual Province Province { get; set; }
        public virtual ServicePlan ServicePlan { get; set; }
        public virtual Village Village { get; set; }
    }
}
