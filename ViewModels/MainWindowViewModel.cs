using Avalonia.Controls;
using ReactiveUI;
using WriteErace.Models;

namespace WriteErace.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        UserControl _pageContent = new Auth();
        public UserControl PageContent { get => _pageContent; set => this.RaiseAndSetIfChanged( ref _pageContent , value); }

        public static _43pKolinichenkoUp2Context MyContext = new _43pKolinichenkoUp2Context();

        public static MainWindowViewModel Instance;
        public MainWindowViewModel() { Instance = this;}

    }
}
