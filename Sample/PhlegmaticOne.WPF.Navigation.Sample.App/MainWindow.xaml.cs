using PhlegmaticOne.WPF.Navigation.Sample.ViewModels;
using System.Windows;

namespace PhlegmaticOne.WPF.Navigation.Sample.App;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }
}
