using CB.Xaml.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace GC.Programmer
{
    public class ColorPropertyBrighnessComparer : ColorBrighnessComparer
    {
        public ColorPropertyBrighnessComparer(bool ascending)
            : base(ascending)
        {

        }

        public ColorPropertyBrighnessComparer()
            : base()
        {

        }

        public override int Compare(object x, object y)
        {
            StaticPropertyInfo propX = x as StaticPropertyInfo, propY = y as StaticPropertyInfo;
            if (propX != null && propX.Value is Color && propY != null && propY.Value is Color)
            {
                return base.Compare((Color)propX.Value, (Color)propY.Value);
            }
            return base.Compare(x, y);
        }
    }
}
