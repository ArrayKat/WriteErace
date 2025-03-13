using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteErace.Models
{
    public partial class Product
    {
        public string Color => CurrentDiscount > 15 ? "#7fff00" : "#FFFFFF";

        public decimal? TotalCost => Cost != 0 ? Cost * (1- CurrentDiscount / 100) : Convert.ToDecimal(0);
        public bool IsVisibleCost => TotalCost==Cost ? false : true;

        
    }
}
