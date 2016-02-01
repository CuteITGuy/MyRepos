using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using CB.Win32.WindowManipulation;
using Point = System.Drawing.Point;
using Rect = CB.Win32.Common.Rect;
using Window = CB.Win32.Windows.Window;


namespace GC.Programmer
{
    public partial class MainWindow
    {
        #region Fields
        const string BEGIN_GET = "Begin get", END_GET = "End get", BEGIN_MOVE = "Begin move", END_MOVE = "End move";

        //const string MOVE_GET_STATUS = "Use {0} mouse button to move/get window.";
        string _moveGetStatus = string.Empty;
        #endregion


        #region Event Handlers
        private void btGetWindowInfo_Click(object sender, RoutedEventArgs e)
        {
            var content = btGetWindowInfo.Content as string;
            if (content == null) return;

            switch (content)
            {
                case BEGIN_GET:

                    // If button btMoveWindow is being activated
                    if (btMoveWindow.Content as string == END_MOVE)
                    {
                        // Deactivate it
                        EndMove();
                    }
                    BeginGet();
                    break;

                case END_GET:
                    EndGet();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void btMoveWindow_Click(object sender, RoutedEventArgs e)
        {
            var content = btMoveWindow.Content as string;
            if (content == null) return;

            switch (content)
            {
                case BEGIN_MOVE:

                    // If button btGetWindowInfo is being activated
                    if (btGetWindowInfo.Content as string == END_GET)
                    {
                        // Deactivate it
                        EndGet();
                    }
                    BeginMove();
                    break;

                case END_MOVE:
                    EndMove();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RadioButtonsMoveType_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(e.Source, rbFirstFound))
            {
                WindowGet.ManipulationType = WindowManipulationType.FirstWindowFound;
                WindowMove.ManipulationType = WindowManipulationType.FirstWindowFound;
            }
            else if (Equals(e.Source, rbTopLevel))
            {
                WindowGet.ManipulationType = WindowManipulationType.TopLevelWindow;
                WindowMove.ManipulationType = WindowManipulationType.TopLevelWindow;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void RadioButtonsSelectButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = e.Source as RadioButton;
            if (!(rb?.Tag is WindowManipulationButton)) return;

            var manButton = (WindowManipulationButton)rb.Tag;
            WindowGet.Button = manButton;
            WindowMove.Button = manButton;

            // Set text used on Status Bar
            var s = manButton.ToString();
            var endIndex = s.IndexOf("Button", StringComparison.Ordinal);
            _moveGetStatus = $"Use {s.Substring(0, endIndex).ToLower()} mouse button to move/get window.";

            // If txtStatus was initialized and button btGetWindowInfo or btMoveWindow is being activated
            if (txtStatus != null &&
                (btGetWindowInfo.Content as string == END_GET || btMoveWindow.Content as string == END_MOVE))
            {
                // Refresh status text
                txtStatus.Text = _moveGetStatus;
            }
        }

        void WindowGet_WindowHandleChanged(object sender, EventArgs e)
        {
            FlowDocument document;

            //string winInfo = string.Empty;
            var winHndl = WindowGet.WindowHandle;

            if (winHndl != IntPtr.Zero)
            {
                var winClass = Window.GetWindowClass(winHndl);
                var winText = Window.GetWindowText(winHndl);
                var processId = Window.GetWindowProcessId(winHndl);
                var threadId = Window.GetWindowThreadId(winHndl);

                // Get window size and absolute window position
                Rect winRect;
                Window.GetWindowRect(winHndl, out winRect);

                // Get relative window position
                var relPos = Window.GetRelativePos(winHndl);

                document = CreateInfoDocument(winClass, winText, processId, threadId, winRect, relPos);
            }
            else
            {
                document = new FlowDocument(new Paragraph(new Run("No window found.")));
            }

            txtWinHandle.Text = winHndl.ToString();
            txtWindowInfo.Document = document;

            //txtWindowInfo.Text = winInfo;
        }
        #endregion


        #region Implementation
        private void BeginGet()
        {
            WindowGet.BeginGet();
            btGetWindowInfo.Content = END_GET;
            txtStatus.Text = _moveGetStatus;
        }

        private void BeginMove()
        {
            WindowMove.BeginMove();
            btMoveWindow.Content = END_MOVE;
            txtStatus.Text = _moveGetStatus;
        }

        private static FlowDocument CreateInfoDocument(string winClass, string winText, int processId, int threadId,
            Rect winRect, Point relPos)
        {
            var xaml =
                $@"<FlowDocument xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
    <Paragraph>
        <Span><LineBreak/><Bold>Class:</Bold> {winClass}<LineBreak/></Span>
        <Span><Bold>Text:</Bold> {winText}<LineBreak/></Span>
        <Span><Bold>Process ID:</Bold> {processId}<LineBreak/></Span>
        <Span><Bold>Thread ID:</Bold> {threadId}<LineBreak/></Span>
        <Span><Bold>aX</Bold> = {winRect
                    .X}    <Bold>aY</Bold> = {winRect.Y}<LineBreak/></Span>
        <Span><Bold>rX</Bold> = {relPos.X}    <Bold>rY</Bold> = {relPos
                        .Y}<LineBreak/></Span>
        <Span><Bold>Width</Bold> = {winRect.Width}    <Bold>Height</Bold> = {winRect
                            .Height}</Span>
    </Paragraph>
</FlowDocument>";

            using (var strReader = new StringReader(xaml))
            {
                using (var xmlReader = XmlReader.Create(strReader))
                {
                    return XamlReader.Load(xmlReader) as FlowDocument;
                }
            }
        }

        private void EndGet()
        {
            WindowGet.EndGet();
            btGetWindowInfo.Content = BEGIN_GET;
            txtStatus.Text = string.Empty;
        }

        private void EndMove()
        {
            WindowMove.EndMove();
            btMoveWindow.Content = BEGIN_MOVE;
            txtStatus.Text = string.Empty;
        }

        private void SetMoveGetStatus()
        {
            var s = WindowGet.Button.ToString();
            txtStatus.Text =
                $"Use {s.Substring(0, s.IndexOf("Button", StringComparison.Ordinal)).ToLower()} mouse button to move/get window.";
        }
        #endregion
    }
}