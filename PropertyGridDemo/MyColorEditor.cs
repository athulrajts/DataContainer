using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
}
