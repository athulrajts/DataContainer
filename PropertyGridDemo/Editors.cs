using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Binding = System.Windows.Data.Binding;

namespace PropertyGridDemo
{
    public class MyColorEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() != typeof(KEI.Infrastructure.Color))
            {
                return value;
            }

            IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (svc != null)
            {
                KEI.Infrastructure.Color c = (KEI.Infrastructure.Color)value;
                var editor = new System.Drawing.Design.ColorEditor();
                System.Drawing.Color edited = (System.Drawing.Color)editor.EditValue(context, provider, System.Drawing.Color.FromArgb(c.R, c.G, c.B));

                c.R = edited.R;
                c.G = edited.G;
                c.B = edited.B;

                return c;
            }

            return value;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            KEI.Infrastructure.Color c = (KEI.Infrastructure.Color)e.Value;
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(c.R, c.G, c.B)))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            e.Graphics.DrawRectangle(Pens.Black, e.Bounds);
        }
    }

    public class ColorEditor : TypeEditor<ColorPicker>
    {
        protected override void SetValueDependencyProperty()
        {
            ValueProperty = ColorPicker.SelectedColorProperty;
        }

        protected override void SetControlProperties(PropertyItem propertyItem)
        {
            Editor.DisplayColorAndName = true;
        }

        protected override IValueConverter CreateValueConverter()
        {
            return new ColorConverter();
        }
    }

    public class SelectorEditor : ITypeEditor
    {
        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            PropertyGridEditorComboBox box = new PropertyGridEditorComboBox();

            var itemSourceBinding = new Binding("Option")
            {
                Source = propertyItem.Value,
                Mode = System.Windows.Data.BindingMode.OneTime
            };

            var selectedItembinding = new Binding("SelectedItem")
            {
                Source = propertyItem.Value,
                ValidatesOnDataErrors = true,
                ValidatesOnExceptions = true,
                Mode = propertyItem.IsReadOnly ? System.Windows.Data.BindingMode.OneWay : System.Windows.Data.BindingMode.TwoWay
            };

            BindingOperations.SetBinding(box, System.Windows.Controls.Primitives.Selector.SelectedItemProperty, selectedItembinding);
            BindingOperations.SetBinding(box, System.Windows.Controls.Primitives.Selector.ItemsSourceProperty, itemSourceBinding);

            return box;
        }
    }
}
