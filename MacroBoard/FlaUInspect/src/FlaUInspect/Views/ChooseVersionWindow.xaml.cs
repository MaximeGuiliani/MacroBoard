using System;
using System.Windows;
using FlaUI.Core;

namespace FlaUInspect.Views
{
    /// <summary>
    /// Interaction logic for ChooseVersionWindow.xaml
    /// </summary>
    public partial class ChooseVersionWindow
    {
        public ChooseVersionWindow()
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded; //romove close button 

        }


        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }



        public AutomationType SelectedAutomationType { get; private set; }

        private void UIA2ButtonClick(object sender, RoutedEventArgs e)
        {
            SelectedAutomationType = AutomationType.UIA2;
            DialogResult = true;
        }

        private void UIA3ButtonClick(object sender, RoutedEventArgs e)
        {
            SelectedAutomationType = AutomationType.UIA3;
            DialogResult = true;
        }
    }
}
