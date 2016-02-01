using CB.Wpf.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace GC.Programmer
{
    public class ColorBrighnessComparer : IComparer<Color>, IComparer<string>, IComparer
    {
        #region Fields
        protected bool ascending;
        #endregion


        #region Constructors
        public ColorBrighnessComparer()
            : this(true)
        {

        }

        public ColorBrighnessComparer(bool ascending)
        {
            this.ascending = ascending;
        }
        #endregion


        #region Methods
        public virtual int Compare(string x, string y)
        {
            // Call the Compare(Color, Color) overload
            return Compare((Color)ColorConverter.ConvertFromString(x), (Color)ColorConverter.ConvertFromString(y));
        }

        public virtual int Compare(object x, object y)
        {
            if (x is Color && y is Color)
            {
                // Call the Compare(Color, Color) overload
                return Compare((Color)x, (Color)y);
            }

            if (x is string && y is string)
            {
                // Call the Compare(string, string) overload
                return Compare(x as string, y as string);
            }

            throw new NotSupportedException();
        }

        public virtual int Compare(Color x, Color y)
        {
            // Do the real work
            var comp = ColorHelper.CalculateBrightness(x).CompareTo(ColorHelper.CalculateBrightness(y));
            return ascending ? comp : -comp;
        }
        #endregion
    }
}
