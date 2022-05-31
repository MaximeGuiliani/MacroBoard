using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using FlaUInspect.ViewModels;

namespace FlaUInspect.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _vm;

        public MainWindow(string[] res)
        {
            InitializeComponent();
            AppendVersionToTitle();
            Height = 340;
            Width = 600;
            Loaded += ToolWindow_Loaded; //romove close button 
            Loaded += MainWindow_Loaded;
            _vm = new MainViewModel(res);
            DataContext = _vm;
            this.res = res;
        }


        //retour valeur automationID
        string[] res;

        //pour cacher la croix en haut a droite
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


        private void AppendVersionToTitle()
        {
            var attr = Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;
            if (attr != null)
            {
                Title += " v" + attr.InformationalVersion;
            }
        }


        private void MainWindow_Loaded(object sender, System.EventArgs e)
        {
            if (!_vm.IsInitialized)
            {
                var dlg = new ChooseVersionWindow { Owner = this };
                if (dlg.ShowDialog() != true)
                {
                    Close();
                }
                _vm.Initialize(dlg.SelectedAutomationType);
                Loaded -= MainWindow_Loaded;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UIHoverMode.IsChecked = false;
            UITrackingMode.IsChecked = false;
            DialogResult = true;
            Close();
        }

        private void TreeViewSelectedHandler(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewItem;
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }




    }
}
