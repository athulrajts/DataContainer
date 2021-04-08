using KEI.Infrastructure;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
