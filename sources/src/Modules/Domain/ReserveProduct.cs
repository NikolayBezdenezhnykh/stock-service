using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public sealed class ReserveProduct
    {
        public ReserveProduct()
        {
            Items = new List<ReserveProductItem>();
        }

        public int Id { get ; set; }

        public DateTime? DateReserve { get; set; }

        public int? Status { get; set; }

        public ICollection<ReserveProductItem> Items { get; set; }
    }
}
