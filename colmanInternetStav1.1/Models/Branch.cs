using System;
using System.Collections.Generic;

namespace colmanInternetStav1._1.Models
{
    public partial class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? CoordinateLat { get; set; }
        public double? CoordinateLng { get; set; }
        public int? ManagerId { get; set; }

        public virtual Users Manager { get; set; }
    }
}
