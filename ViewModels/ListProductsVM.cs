using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteErace.Models;

namespace WriteErace.ViewModels
{
    internal class ListProductsVM:ViewModelBase
    {
        List<Product> products;
        
        public List<Product> Products { get => products; set => this.RaiseAndSetIfChanged( ref products, value); }

        public ListProductsVM()
        {
            Products = MainWindowViewModel.MyContext.Products.ToList();
        }
    }
}
