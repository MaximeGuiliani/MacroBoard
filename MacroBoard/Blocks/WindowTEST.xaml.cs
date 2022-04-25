using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MacroBoard
{
    /// <summary>
    /// Logique d'interaction pour WindowTEST.xaml
    /// </summary>
    public partial class WindowTEST : Window
    {
        public WindowTEST()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Block testBlock = new Blocks.B_CreateTextFile("test", "test2", "info", @"C:\Users\Alien\Desktop", "test", ".exe", "Bonsoir ceci est un text heba");
            testBlock.Execute();
        }
    }
}
