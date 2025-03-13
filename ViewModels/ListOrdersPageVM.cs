using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteErace.Models;

namespace WriteErace.ViewModels
{
    internal class ListOrdersPageVM :ViewModelBase
    {

        int _currentIdUser;
        List <Order> showOrders;
        public List<Order> ShowOrders { get => showOrders; set => this.RaiseAndSetIfChanged(ref showOrders, value); }

        string _fioUser;
        public string FioUser { get => _fioUser; set => this.RaiseAndSetIfChanged(ref _fioUser, value); }
        

        User _currentUser;
        public User CurrentUser { get => _currentUser; set => this.RaiseAndSetIfChanged(ref _currentUser, value); }

        decimal? summOrder = 0;
        public decimal? SummOrder { get => summOrder; set => this.RaiseAndSetIfChanged(ref summOrder, value); }

        decimal? summDiscount = 0;
        public decimal? SummDiscount { get => summDiscount; set => this.RaiseAndSetIfChanged(ref summDiscount, value); }



        
        public ListOrdersPageVM(int idUser) {
            _currentIdUser = idUser;
            CurrentUser = MainWindowViewModel.MyContext.Users.FirstOrDefault(x => x.Id == idUser);
            FioUser = CurrentUser.Surname + " " + CurrentUser.Name + " " + CurrentUser.Patronymic;
            ShowOrders = MainWindowViewModel.MyContext.Orders.Include(x=>x.OrderProducts).Include(x=>x.IdStatusNavigation).Include(x=>x.IdAddressNavigation).ToList();

            CountAllRecords = ShowOrders.Count;
            CountCurrentRecords = ShowOrders.Count;

        }


        #region sort/filter + count records
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
        public int CountCurrentRecords { get => _countCurrentRecords; set => this.RaiseAndSetIfChanged(ref _countCurrentRecords, value); }
        public int CountAllRecords { get => _countAllRecords; set => this.RaiseAndSetIfChanged(ref _countAllRecords, value); }


        public void AllSort()
        {

            ShowOrders = MainWindowViewModel.MyContext.Orders.Include(x => x.OrderProducts).Include(x => x.IdStatusNavigation).Include(x => x.IdAddressNavigation).ToList();
            CountAllRecords = ShowOrders.Count;
            
            if (SortValueId != 0)
            {
                if (SortValueId == 1) ShowOrders = ShowOrders.OrderBy(x => x.TotalSumm).ToList();
                else if (SortValueId == 2) ShowOrders = ShowOrders.OrderByDescending(x => x.TotalSumm).ToList();
            }
            if (FilterValueID != 0)
            {
                switch (FilterValueID)
                {
                    case 1: ShowOrders = ShowOrders.Where(x => x.TotalDiscount > 0 && x.TotalDiscount < 10).ToList(); break;
                    case 2: ShowOrders = ShowOrders.Where(x => x.TotalDiscount >= 10 && x.TotalDiscount < 15).ToList(); break;
                    case 3: ShowOrders = ShowOrders.Where(x => x.TotalDiscount >= 15).ToList(); break;
                }
            }
            CountCurrentRecords = ShowOrders.Count;
        }
        #endregion


        public void GoBack() {
            MainWindowViewModel.Instance.PageContent = new ListProducts(_currentIdUser);
        }

        public void GoBackAuth()
        {
            MainWindowViewModel.Instance.PageContent = new Auth();
        }
    }
}
