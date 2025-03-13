using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using WriteErace.Models;
using WriteErace.ViewModels;

namespace WriteErace;

public partial class OrderPage : UserControl
{
    public OrderPage(List<Product> products, int idUser)
    {
        InitializeComponent();
        DataContext = new OrderVM(products, idUser);
    }

}