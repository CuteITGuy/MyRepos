using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GC.Programmer
{
    public partial class MainWindow : Window
    {
        #region Fields
        IDataObject dropData = null;
        #endregion


        #region Event Handlers

        private void lbDataFormats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDataContent();
        }

        private void tabDropContentDrop(object sender, DragEventArgs e)
        {
            dropData = e.Data;
            var dataFormats = GetDropDataFormats(dropData);
            lbDataFormats.ItemsSource = dataFormats;
            if (dataFormats.Count() > 0)
            {
                lbDataFormats.SelectedIndex = 0;
                ShowDataContent();
            }
        }

        private void txb_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
        #endregion


        #region Implementation
        private static IEnumerable<string> GetDropDataFormats(IDataObject data)
        {
            if (data != null)
            {
                /*return typeof(DataFormats).GetFields()
                        .Select(fieldInfo => fieldInfo.GetValue(null) as string)
                        .Where(dataFormat => e.Data.GetDataPresent(dataFormat));*/

                return from fieldInfos in typeof(DataFormats).GetFields()
                       select fieldInfos.GetValue(null) as string
                           into dataFormat
                           where data.GetDataPresent(dataFormat)
                           select dataFormat;
            }
            return null;
        }

        private static object GetDropDataContent(IDataObject data, string dataFormat)
        {
            if (data != null)
            {
                try
                {
                    return data.GetData(dataFormat, true);
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            return null;
        }

        private void ShowDataContent()
        {
            if (lbDataFormats.SelectedIndex > -1)
            {
                var dataContent = GetDropDataContent(dropData, lbDataFormats.SelectedValue as string);
                Type dataContentType = dataContent.GetType();
                txbType.Text = "Type: " + dataContentType.ToString();

                if (dataContent is string)
                {
                    txbDataContent.Text = dataContent as string;
                }
                else if (dataContent is string[])
                {
                    string content = string.Join(Environment.NewLine, dataContent as string[]);
                    txbDataContent.Text = content;
                }
                else if (dataContent is Exception)
                {
                    txbDataContent.Text = (dataContent as Exception).ToString();
                }
            }
        }
        #endregion
    }
}
