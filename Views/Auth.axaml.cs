using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using WriteErace.ViewModels;

namespace WriteErace;

public partial class Auth : UserControl
{
    public Auth()
    {
        InitializeComponent();
        DataContext = new AuthVM();
    }
}