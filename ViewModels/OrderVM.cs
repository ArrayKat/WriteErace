using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using WriteErace.Models;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WriteErace.Another;

namespace WriteErace.ViewModels
{
    internal class OrderVM : ViewModelBase
    {
        int _currentIdUser;
        User _currentUser;
        List<Product> currentProducts;
        public List<Product> CurrentProducts { get => currentProducts; set => this.RaiseAndSetIfChanged(ref currentProducts, value); }

        Order _currentOrder;

        Address _objAdress;
        public Address ObjAdress { get => _objAdress; set => this.RaiseAndSetIfChanged(ref _objAdress, value); }

        List<Address> adresses;
        public List<Address> Adresses { get => adresses; set => this.RaiseAndSetIfChanged(ref adresses, value); }

        List<PartialOrderProduct> listProduct;
        public List<PartialOrderProduct> ListProduct { get => listProduct; set { this.RaiseAndSetIfChanged(ref listProduct, value); CalculateSum(); } }

        decimal? summOrder = 0;
        public decimal? SummOrder { get => summOrder; set => this.RaiseAndSetIfChanged(ref summOrder, value); }

        decimal? summDiscount = 0;
        public decimal? SummDiscount { get => summDiscount; set => this.RaiseAndSetIfChanged(ref summDiscount, value); }

        decimal? totalSummOrder = 0;
        public decimal? TotalSummOrder { get => totalSummOrder; set => this.RaiseAndSetIfChanged(ref totalSummOrder, value); }


        public void CalculateSum() {
            SummOrder = ListProduct.Sum(x => x.CountProducts * x.ArticleProductsNavigation.Cost);
            SummDiscount = ListProduct.Sum(x => x.CountProducts * x.ArticleProductsNavigation.Cost - x.CountProducts * x.ArticleProductsNavigation.TotalCost);
            TotalSummOrder = SummOrder - SummDiscount;
            this.RaisePropertyChanged(nameof(SummOrder));
            this.RaisePropertyChanged(nameof(SummDiscount));
            this.RaisePropertyChanged(nameof(TotalSummOrder));
        }

        public OrderVM(List<Product> products, int idUser)
        {
            _currentIdUser = idUser;
            CurrentProducts = products;

            if (idUser != 0)
                _currentUser = MainWindowViewModel.MyContext.Users.FirstOrDefault(x => x.Id == idUser);
            else
                _currentUser = new User() { IdRole = 1, IdRoleNavigation = MainWindowViewModel.MyContext.Roles.FirstOrDefault(x => x.Id == 1) };

            Adresses = MainWindowViewModel.MyContext.Addresses.ToList();

            // Инициализация ListProduct
            ListProduct = ProcessProductsAndCreateOrderProducts(products);

            foreach (var orderProductWrapper in ListProduct)
            {
                orderProductWrapper.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(PartialOrderProduct.CountProducts))
                    {
                        CalculateSum();
                    }
                };
            }
        }

        private List<PartialOrderProduct> ProcessProductsAndCreateOrderProducts(List<Product> products)
        {
            var productDictionary = new Dictionary<string, (Product Product, int Quantity)>();

            // Удаляем дубликаты и считаем количество
            foreach (var product in products)
            {
                if (productDictionary.ContainsKey(product.Article))
                {
                    var entry = productDictionary[product.Article];
                    entry.Quantity++;
                    productDictionary[product.Article] = entry;
                }
                else
                {
                    productDictionary[product.Article] = (product, 1);
                }
            }

            // Создаем список OrderProductWrapper
            var orderProducts = new List<PartialOrderProduct>();
            foreach (var kvp in productDictionary)
            {
                var orderProduct = new OrderProduct
                {
                    ArticleProducts = kvp.Key,
                    CountProducts = kvp.Value.Quantity,
                    ArticleProductsNavigation = kvp.Value.Product
                };

                // Обертываем OrderProduct в OrderProductWrapper
                orderProducts.Add(new PartialOrderProduct(orderProduct, this));
            }

            return orderProducts;
        }

        public async void CreateOrder()
        {
            if (ObjAdress == null || ObjAdress.Id == 0)
            {
                await MessageBoxManager.GetMessageBoxStandard("Сообщение", "Вы не заполнили адрес доставки", ButtonEnum.Ok).ShowAsync();
                return;
            }
            Order order = new Order()
            {
                DateOrder = DateOnly.FromDateTime(DateTime.Now),
                DateDelivery = DateOnly.FromDateTime(DateTime.Now.AddDays(6)),
                IdAddress = ObjAdress.Id,
                IdAddressNavigation = ObjAdress,
                IdClient = _currentUser.Id,
                IdClientNavigation = _currentUser,
                IdStatus = 1,
                IdStatusNavigation = MainWindowViewModel.MyContext.Statuses.FirstOrDefault(x => x.Id == 1),
                Code = new Random().Next(100, 999),
            };
            _currentOrder = order;
            MainWindowViewModel.MyContext.Orders.Add(order);
            MainWindowViewModel.MyContext.SaveChanges();

            // Добавляем OrderProduct в заказ
            foreach (var orderProductWrapper in ListProduct)
            {
                if (orderProductWrapper.CountProducts > 0)
                {
                    var orderProduct = orderProductWrapper.GetOrderProduct();
                    orderProduct.IdOrder = order.Id;
                    MainWindowViewModel.MyContext.OrderProducts.Add(orderProduct);
                }
                else {
                    await MessageBoxManager.GetMessageBoxStandard("Сообщение", $"Продукт {orderProductWrapper.ArticleProductsNavigation.NameProducts} был удален из списка продуктов, потому что количество меньше 0.", ButtonEnum.Ok).ShowAsync();
                }
            }
            MainWindowViewModel.MyContext.SaveChanges();
            await MessageBoxManager.GetMessageBoxStandard("Сообщение", "Заказ успешно сформирован", ButtonEnum.Ok).ShowAsync();
            GenertePDF();
            await MessageBoxManager.GetMessageBoxStandard("Сообщение", $"Талон '{_currentOrder.Id}.pdf' успешно сохранен", ButtonEnum.Ok).ShowAsync();
            MainWindowViewModel.Instance.PageContent = new ListProducts(_currentIdUser);
        }

        public async Task DeleteProduct(PartialOrderProduct del)
        {
            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение", "Хотите удалить продукт из заказа?", ButtonEnum.YesNo).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                // Удаляем продукт из списка
                ListProduct.Remove(del);

                // Обновляем ListProduct в главном потоке
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    this.RaisePropertyChanged(nameof(ListProduct));
                    CalculateSum();
                });

                // Показываем уведомление об успешном удалении
                await MessageBoxManager.GetMessageBoxStandard("Сообщение", "Продукт успешно удален", ButtonEnum.Ok).ShowAsync();
            }
        }

        public void GoBack()
        {
            MainWindowViewModel.Instance.PageContent = new ListProducts(_currentUser.Id);
        }

        public void GenertePDF() {
            PDFTicket pdf = new PDFTicket(_currentOrder);
            pdf.CreatePDF();
        }
    }
}