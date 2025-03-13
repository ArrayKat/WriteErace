using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErace.ViewModels;

namespace WriteErace;

public partial class ListOrdersPage : UserControl
{
    public ListOrdersPage(int idUser)
    {
        InitializeComponent();
        DataContext = new ListOrdersPageVM(idUser);
    }
}