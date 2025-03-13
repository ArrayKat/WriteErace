using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteErace.ViewModels;

namespace WriteErace.Models
{
    internal class PartialOrderProduct :ReactiveObject
    {
        private readonly OrderProduct _orderProduct;
        private readonly OrderVM _orderVM; // Ссылка на OrderVM

        public PartialOrderProduct(OrderProduct orderProduct, OrderVM orderVM)
        {
            _orderProduct = orderProduct;
            _orderVM = orderVM; // Инициализация ссылки на OrderVM
        }

        // Делегирование свойств оригинальной модели
        public int Id => _orderProduct.Id;
        public int IdOrder => _orderProduct.IdOrder;
        public string ArticleProducts => _orderProduct.ArticleProducts;

        // Свойство CountProducts с уведомлением об изменении
        public int CountProducts
        {
            get => _orderProduct.CountProducts;
            set
            {
                if (_orderProduct.CountProducts != value)
                {
                    _orderProduct.CountProducts = value;
                    this.RaisePropertyChanged(nameof(CountProducts)); // Уведомление об изменении

                    // Если количество стало 0, вызываем метод удаления
                    if (value == 0)
                    {
                        Task.Run(() => _orderVM.DeleteProduct(this)); // Асинхронный вызов без блокировки UI
                    }
                }
            }
        }

        public Product ArticleProductsNavigation => _orderProduct.ArticleProductsNavigation;
        public Order IdOrderNavigation => _orderProduct.IdOrderNavigation;

        // Метод для получения оригинального OrderProduct
        public OrderProduct GetOrderProduct() => _orderProduct;
    }
}
