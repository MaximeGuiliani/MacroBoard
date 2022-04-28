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
        private List<BlockView> BlockViews = new();

        public WindowTEST()
        {
            InitializeComponent();
            InitBlock();
        }

        private void InitBlock()
        {
            BlockViews.Add(new BlockView("Restart Computer"));
            BlockViews.Add(new BlockView("Run App"));

            foreach(BlockView blockView in BlockViews)
            {
                blockView.Btn_Delete.Click += OnClick_Delete;
                listTest.Items.Add(blockView.Content);
            }

        }

        private void OnClick_Delete(object sender, RoutedEventArgs e)
        {
            listTest.Items.Remove(((Button)sender).Parent);
        }


    }

}
