using System;
using System.Collections.Generic;

namespace colmanInternetStav1._1.Models
{
    public partial class Set
    {
        public Set()
        {
            Jewelry = new HashSet<Jewelry>();
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Jewelry> Jewelry { get; set; }
    }
}
