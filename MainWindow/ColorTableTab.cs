using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GC.Programmer
{
    public partial class MainWindow : Window
    {
        #region Fields
        PropertyInfo[] props = null;
        #endregion


        #region Properties

        #endregion


        #region Constructors

        #endregion


        #region Methods

        #endregion


        #region Events
        private void tabColorTable_Initialized(object sender, EventArgs e)
        {
            props = typeof(Colors).GetProperties();
            lbColor.DataContext = props;
            lvColor.DataContext = props;
        }
        #endregion


        #region Implementation

        #endregion
    }
}
