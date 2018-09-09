using System;
using System.Collections.Generic;

namespace colmanInternetStav1._1.Models
{
    public partial class Jewelry
    {
        public Jewelry()
        {
            Purchase = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public double? Weight { get; set; }
        public double Price { get; set; }
        public int? Cart { get; set; }
        public string Descriptiom { get; set; }
        public int? Size { get; set; }
        public int SetId { get; set; }

        public virtual ICollection<Purchase> Purchase { get; set; }
        public virtual Set Set { get; set; }
    }
}
