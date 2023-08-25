using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public sealed class AvailableProduct
    {
        public int Id { get; set; }

        public long? ProductId { get; set; }

        public int? StockLevel { get; set; }
    }
}
