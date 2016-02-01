using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CB.Application.ContextMenuCommands;
using CB.Win32.WindowManipulation;


namespace GC.Programmer
{
    public partial class MainWindow
    {
        #region  Constructors & Destructor
        public MainWindow()
        {
            InitializeComponent();
            InitializeMore();
        }
        #endregion


        #region Override
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            WindowGet.EndGet();
            WindowMove.EndMove();
        }
        #endregion


        #region Event Handlers
        private void btAccept_Click(object sender, RoutedEventArgs e) { }

        private void btInputFileTypes_Click(object sender, RoutedEventArgs e)
        {
            lbCategoriesFileTypes.Items.Add(txbInputFileTypes.Text);
            txbInputFileTypes.Text = string.Empty;
        }

        private void btRemoveFileTypes_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = lbCategoriesFileTypes.SelectedItems.OfType<string>().ToArray();
            foreach (var item in selectedItems)
            {
                lbCategoriesFileTypes.Items.Remove(item);
            }
        }

        private void lbCategoriesFileTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btRemoveFileTypes.Visibility = lbCategoriesFileTypes.SelectedIndex > -1
                                               ? Visibility.Visible : Visibility.Hidden;
        }

        private void RadioButtonCommandTypes_Checked(object sender, RoutedEventArgs e)
        {
            if (rbAppCommand.IsChecked == true) { }
            else if (rbCommonCommand.IsChecked == true) { }
        }

        private void RadioButtonFileTypesOrCategories_Checked(object sender, RoutedEventArgs e)
        {
            if (rbFileTypes.IsChecked == true)
            {
                dpFileTypes.Visibility = Visibility.Visible;
                txtCategoriesFileTypes.Text = "Add one or more file types:";
                lbCategoriesFileTypes.ItemsSource = null;
            }
            else if (rbCategories.IsChecked == true)
            {
                dpFileTypes.Visibility = Visibility.Collapsed;
                txtCategoriesFileTypes.Text = "Choose one or more categories:";
                lbCategoriesFileTypes.ItemsSource = Enum.GetValues(typeof(CommandCategories));
            }
        }

        private void RadioButtonsAddOrRemoveCommand_Checked(object sender, RoutedEventArgs e)
        {
            if (rbAddCommand.IsChecked == true)
            {
                spChooseCommandType.Visibility = dpInputCommandContent.Visibility = Visibility.Visible;
                txtCommand.Text = "Add a command of:";
            }
            else if (rbRemoveCommand.IsChecked == true)
            {
                spChooseCommandType.Visibility = dpInputCommandContent.Visibility = Visibility.Collapsed;
                txtCommand.Text = "Remove commands of:";
            }
        }

        private void tiContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            rbAddCommand.IsChecked = rbFileTypes.IsChecked = rbAppCommand.IsChecked = true;
        }

        private void txbInputFileTypes_TextChanged(object sender, TextChangedEventArgs e)
        {
            btInputFileTypes.IsEnabled = !string.IsNullOrWhiteSpace(txbInputFileTypes.Text) &&
                                         txbInputFileTypes.Text != string.Empty;
        }
        #endregion


        #region Implementation
        private void InitializeMore()
        {
            btGetWindowInfo.Content = BEGIN_GET;
            WindowGet.Button = WindowManipulationButton.MiddleButton;
            WindowGet.WindowHandleChanged += WindowGet_WindowHandleChanged;

            btMoveWindow.Content = BEGIN_MOVE;
            WindowMove.Button = WindowManipulationButton.MiddleButton;
            WindowMove.WindowHandleChanged += delegate { txtWinHandle.Text = WindowMove.WindowHandle.ToString(); };
        }
        #endregion
    }
}