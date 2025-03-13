using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteErace.Models
{
    public partial class Order
    {
        public decimal? TotalSumm => OrderProducts.Sum(x => x.CountProducts * x.ArticleProductsNavigation.Cost);
        public decimal? TotalDiscount => OrderProducts.Sum(x => x.ArticleProductsNavigation.CurrentDiscount);
        public decimal? TotalSummDiscount => OrderProducts.Sum(x => x.CountProducts * x.ArticleProductsNavigation.Cost - x.CountProducts * x.ArticleProductsNavigation.TotalCost);

        public string ColorOrder { 
            get {
                if (OrderProducts.Any(x=>x.ArticleProductsNavigation.Count > 3)) return "#20b2aa";
                else if (OrderProducts.Any( X=>X.ArticleProductsNavigation.Count <= 0)) return "#ff8c00";
                else return "#ffffff";
            } 
        }
        public int DeliveryDuration{ 
            get {
                if (OrderProducts.Any(x => x.ArticleProductsNavigation.Count > 3)) return 3;
                else return 6;
            } 
        }

    }
}
