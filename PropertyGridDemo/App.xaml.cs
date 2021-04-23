using System.Configuration;
using System.Windows;
using Application = System.Windows.Application;

namespace PropertyGridDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            PropertyGridHelper.RegisterEditor<MyColorEditor>(DataObjectType.Color);

            PropertyGridHelper.RegisterEditor<SelectorEditor>(DataObjectType.Selectable);
            base.OnStartup(e);
        }


    }
}
