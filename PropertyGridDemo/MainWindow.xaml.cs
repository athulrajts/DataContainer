using Infragistics.Controls.Editors;
using KEI.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace PropertyGridDemo
{

    public class Test
    {
        [Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
        public KEI.Infrastructure.Color Color { get; set; } = new KEI.Infrastructure.Color(234,126,67);
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SelectedItem = PropertyContainerBuilder.Create("Shape")
                .Color("Fill", "#000000", p => p
                    .SetCategory("Visualization")
                    .SetDescription("Fill color of shape")
                    .SetDisplayName("Background"))
                .Color("Stroke", "#000000", p => p
                    .SetCategory("Visualization")
                    .SetDescription("Border color of shape")
                    .SetDisplayName("Border"))
                .Number("StrokeThickness", 3.0, p => p
                    .SetCategory("Visualization")
                    .SetDescription("Border thickness of shape")
                    .SetDisplayName("Border Thickness"))
                .Number("Height", 200.0, p => p
                    .SetCategory("Definition")
                    .SetDescription("Height of shape"))
                .Number("Width", 200.0, p => p
                    .SetCategory("Definition")
                    .SetDescription("Width of shape"))
                .Number("X", 50.0, p => p
                    .SetCategory("Definition")
                    .SetDescription("X-Coordinate of top left point"))
                .Number("Y", 50.0, p => p
                    .SetCategory("Definition")
                    .SetDescription("Y-Coordinate of top left point"))
                .Build();


            SelectedItem.PropertyChanged += SelectedItem_PropertyChanged;

            formsGrid.SelectedObject = SelectedItem;
        }

        private void SelectedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            formsGrid.Refresh();
        }

        public IDataContainer SelectedItem { get; }

        public Dictionary<string, Object> Test { get; } = new Dictionary<string, object>();
    }

}
