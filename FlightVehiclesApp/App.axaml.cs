using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FlightVehiclesApp.ViewModels;
using FlightVehiclesApp.Views;

namespace FlightVehiclesApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new letatelniyapparat.Views.MainWindow
                {
                    DataContext = new MainViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }

    internal class MainViewModel
    {
        public MainViewModel()
        {
        }
    }
}