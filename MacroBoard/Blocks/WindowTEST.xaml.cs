using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<Frame> BlocksFrame = new();
        
        public WindowTEST()
        {
            InitializeComponent();
            InitBlock();   
        }

        private void InitBlock()
        {
            List<Grid> grid = new();
            grid.Add(new Grid());
            grid.Add(new Grid());


            Button btn = new Button();
            btn.Content = "Bouton de fou la 1";
            btn.Click += test;
            grid[0].Children.Add(btn);

            Button btn2 = new Button();
            btn2.Content = "Bouton de fou la 2";
            btn2.Click += test;
            grid[1].Children.Add(btn2);


            listTest.Items.Add(grid[0]);
            listTest.Items.Add(grid[1]);
        }

        private void test(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("dkhgshkfjsfvjkdhs");
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            //   Block testBlock = new Blocks.B_CreateTextFile("test", "test2", "info", @"\\Mac\Home\Desktop\", "test", ".exe", "Bonsoir ceci est un text heba");
            //   testBlock.Execute();
            listTest.Items.Add("hello");
            
            listTest.Items.Add(new Button());
            
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            mainButton.Background = new SolidColorBrush(Colors.Red);
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            mainButton.Background = new SolidColorBrush(Colors.Blue);

        }
    }

    public class BlockView
    {
        private string Label_Name { get; }
        private string Button_Delete { get; }

        public BlockView(string Block_Name, string DeleteButton_Name)
        {
            this.Label_Name = Block_Name;
            this.Button_Delete = DeleteButton_Name;
        }
    }
}
