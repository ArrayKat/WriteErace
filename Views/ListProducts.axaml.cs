using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErace.ViewModels;

namespace WriteErace;

public partial class ListProducts : UserControl
{
    public ListProducts(int idUser)
    {
        InitializeComponent();
        DataContext = new ListProductsVM(idUser);
    }
}