using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


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
            BlockViews.Add( new BlockView("Restart Computer", new Blocks.B_Restart()) );
            BlockViews.Add( new BlockView("Run Application", new Blocks.B_RunApp("Run Application", "", "", "notepad.exe")) );
            BlockViews.Add( new BlockView("Wait", new Blocks.B_Wait("Wait", "", "", 0, 0, 0)) );
            BlockViews.Add( new BlockView("Capture", new Capture("Capture", "", "", 1)) );

            foreach (BlockView blockView in BlockViews)
            {
                blockView.Btn_Delete.Click += OnClick_Delete;
                blockView.Btn_Edit.Click += OnClick_Edit;
                blockView.Btn_Up.Click += OnClick_Up;
                blockView.Btn_Down.Click += OnClick_Down;
                listTest.Items.Add(blockView.Content);
            }
            BlockViews[0].Content.Children[3].Visibility = Visibility.Hidden;
            BlockViews[BlockViews.Count-1].Content.Children[4].Visibility = Visibility.Hidden;
        }

        private void OnClick_Delete(object sender, RoutedEventArgs e)
        {
            int currentItemPos = listTest.Items.IndexOf(((Button)sender).Parent);
            if(listTest.Items.Count > 1 && currentItemPos == 0)
            {
                Grid nextItem = (Grid)listTest.Items.GetItemAt(currentItemPos + 1);
                nextItem.Children[3].Visibility = Visibility.Hidden;
            }

            if(listTest.Items.Count > 1 && currentItemPos == listTest.Items.Count-1)
            {
                Grid previousItem = (Grid)listTest.Items.GetItemAt(currentItemPos - 1);
                previousItem.Children[4].Visibility = Visibility.Hidden;
            }

            listTest.Items.RemoveAt(currentItemPos);
        }
        private void OnClick_Edit(object sender, RoutedEventArgs e)
        {
            
        }
        private void OnClick_Up(object sender, RoutedEventArgs e)
        {
            int currentItemPos = listTest.Items.IndexOf(((Button)sender).Parent);
            Grid previousItem = (Grid)listTest.Items.GetItemAt(currentItemPos - 1);

            SetHiddenOrVisibleBtnUp(sender, previousItem);

            listTest.Items.Remove(((Button)sender).Parent);
            listTest.Items.Remove(previousItem);

            listTest.Items.Insert(currentItemPos - 1, ((Button)sender).Parent);
            listTest.Items.Insert(currentItemPos, previousItem);
        }
        private void OnClick_Down(object sender, RoutedEventArgs e)
        {
            int currentItemPos = listTest.Items.IndexOf(((Button)sender).Parent);
            Grid nextItem = (Grid)listTest.Items.GetItemAt(currentItemPos + 1);

            SetHiddenOrVisibleBtnDown(sender, nextItem);

            listTest.Items.Remove(((Button)sender).Parent);
            listTest.Items.Remove(nextItem);

            listTest.Items.Insert(currentItemPos, nextItem);
            listTest.Items.Insert(currentItemPos + 1, ((Button)sender).Parent);
        }

        private void SetHiddenOrVisibleBtnUp(object sender, Grid previousItem)
        {
            int currentItemPos = listTest.Items.IndexOf(((Button)sender).Parent);
            Grid currentItem = (Grid)listTest.Items.GetItemAt(currentItemPos);

            currentItem.Children[4].Visibility = Visibility.Visible;
            previousItem.Children[3].Visibility = Visibility.Visible;

            if (currentItemPos - 1 == 0)
            {
                currentItem.Children[3].Visibility = Visibility.Hidden;
            }

            if (currentItemPos == listTest.Items.Count - 1)
            {
                previousItem.Children[4].Visibility = Visibility.Hidden;
            }
        }

        private void SetHiddenOrVisibleBtnDown(object sender, Grid nextItem)
        {
            int currentItemPos = listTest.Items.IndexOf(((Button)sender).Parent);
            Grid currentItem = (Grid)listTest.Items.GetItemAt(currentItemPos);

            currentItem.Children[3].Visibility = Visibility.Visible;
            nextItem.Children[4].Visibility = Visibility.Visible;

            if (currentItemPos + 1 == listTest.Items.Count - 1)
            {
                currentItem.Children[4].Visibility = Visibility.Hidden;
            }

            if (currentItemPos == 0)
            {
                nextItem.Children[3].Visibility = Visibility.Hidden;
            }

        }


    }

}
