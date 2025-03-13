using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
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
        User _currentUser;
        string _fioUser;
        public string FioUser { get => _fioUser; set => this.RaiseAndSetIfChanged (ref _fioUser, value); }

        List<Product> products;
        public List<Product> Products { get => products; set => this.RaiseAndSetIfChanged( ref products, value); }

       

        string _search;
        public string Search { get => _search; set { this.RaiseAndSetIfChanged(ref _search, value); AllSort(); } }

        private List<string> _sortValue = new List<string>() { 
            "Без сортировки по стоимости",
            "По возрастанию",
            "По убыванию"
        };
        public List<string> SortValue { get => _sortValue; set => _sortValue = value; }
        
        int _sortValueId = 0;
        public int SortValueId { get => _sortValueId; set { this.RaiseAndSetIfChanged(ref _sortValueId, value); AllSort(); } }

        private List<string> _filterValue = new List<string>()
        {
            "Все диапазоны скидки",
            "0-9,99%",
            "10-14,99%",
            "15% и более"
        };
        public List<string> FilterValue { get => _filterValue; set => _filterValue = value; }
        

        int _filterValueID;
        public int FilterValueID { get => _filterValueID; set { this.RaiseAndSetIfChanged(ref _filterValueID, value); AllSort(); } }

        int _countCurrentRecords;
        int _countAllRecords;
        public int CountCurrentRecords { get => _countCurrentRecords; set => this.RaiseAndSetIfChanged( ref _countCurrentRecords, value); }
        public int CountAllRecords { get => _countAllRecords; set => this.RaiseAndSetIfChanged( ref _countAllRecords, value); }

       

        public ListProductsVM(int idUser)
        {
            if (idUser == 0)
            {
                _currentUser = new User();
                FioUser = "Гость";
            }
            else { 
                List <User> users = MainWindowViewModel.MyContext.Users.ToList();
                _currentUser = users.FirstOrDefault(x => x.Id == idUser);
                FioUser = _currentUser.Surname + " " + _currentUser.Name + " " + _currentUser.Patronymic;
            }
            Products = MainWindowViewModel.MyContext.Products.Include(x =>x.IdManufacturerNavigation).ToList();
            CountCurrentRecords = Products.Count;
            CountAllRecords = Products.Count;
            if (_currentUser.IdRole > 1) {
                IsVisibleListOrders = true;
            }
        }
        public async Task Hello() {
            await MessageBoxManager.GetMessageBoxStandard("Сообщение", "Hello", ButtonEnum.Ok).ShowAsync();
        }

        public void AllSort() {
            Products = MainWindowViewModel.MyContext.Products.Include(x => x.IdManufacturerNavigation).ToList();
            CountAllRecords = Products.Count;
            if (!string.IsNullOrEmpty(Search)) { 
                Products = Products.Where(x=> x.NameProducts.ToLower().Contains(_search.ToLower())).ToList();
            }
            if (SortValueId != 0) { 
                if(SortValueId==1) Products = Products.OrderBy(x=>x.Cost).ToList();
                else if(SortValueId==2) Products = Products.OrderByDescending(x => x.Cost).ToList();
            }
            if (FilterValueID != 0) {
                switch (FilterValueID) {
                    case 1: Products = Products.Where(x=>x.CurrentDiscount>0 && x.CurrentDiscount<10).ToList(); break;
                    case 2: Products = Products.Where(x => x.CurrentDiscount >= 10 && x.CurrentDiscount < 15).ToList(); break;
                    case 3: Products = Products.Where(x => x.CurrentDiscount >= 15).ToList(); break;
                }
            }
            CountCurrentRecords = Products.Count;
        }

        public async Task GoBack() { 
            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard("Сообщение", "Вы уверены что хотите выйти из аккаунта?", ButtonEnum.YesNo).ShowAsync();
            if (result == ButtonResult.Yes) MainWindowViewModel.Instance.PageContent = new Auth();
        }


        List<Product> order = new List<Product>();
        public List<Product> Order { get => order; set => this.RaiseAndSetIfChanged( ref order, value); }
        

        bool _isVisibleBtnOrders = false;
        public bool IsVisibleBtnOrders { get => _isVisibleBtnOrders; set => this.RaiseAndSetIfChanged( ref _isVisibleBtnOrders, value); }

        bool isVisibleListOrders;
        public bool IsVisibleListOrders { get => isVisibleListOrders; set => isVisibleListOrders = value; }

        public void AddProductToOrder(Product product) {
            if (product != null) {
                order.Add(product);
                if (Order.Count != 0)
                {
                    IsVisibleBtnOrders = true;
                }
            }
            
        }

        public void GoPageListOrders() {
            MainWindowViewModel.Instance.PageContent = new ListOrdersPage(_currentUser.Id);
        }

        public void AddOrder() {
            MainWindowViewModel.Instance.PageContent = new OrderPage(Order, _currentUser.Id);
        }
    }
}
