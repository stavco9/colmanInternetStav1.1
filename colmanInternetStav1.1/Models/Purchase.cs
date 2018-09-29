using System;
using System.Collections.Generic;

namespace colmanInternetStav1._1.Models
{
    public partial class Purchase
    {
        public int JewelryId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public double? Amount { get; set; }
        public string Reference { get; set; }
        public string Country { get; set; }
        public int Id { get; set; }

        public virtual Jewelry Jewelry { get; set; }
        public virtual Users User { get; set; }
    }
}
