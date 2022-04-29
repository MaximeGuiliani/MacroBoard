using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for EditionWindow.xaml
    /// </summary>
    public partial class EditionWindow : Window
    {
        WorkFlow workFlow;
        private string ImagePathValue;
        public string ImagePath
        {
            get { return ImagePathValue; }
            set { ImagePathValue = ImagePath; }
        }
        




        private List<BlockView> BlockViews = new();
        // TODO : Prendra en argument une Macro (List de block) Nom
        public EditionWindow()
        {
            InitializeComponent();
            InitBlock();
            workFlow = new WorkFlow(null,null,null);
            changeImage("../Images/block.png");
            DataContext = this;
        }

        public EditionWindow(WorkFlow workFlow)
        {
            InitializeComponent();
            this.workFlow = workFlow;
            changeImage(workFlow.imagePath);
            
        }

        public void changeImage(string path)
        {
            Image img = new Image();
            BitmapImage bitmapImg = new BitmapImage();

            bitmapImg.BeginInit();
            bitmapImg.UriSource = new Uri(@"..\Images\Block.png", UriKind.Relative);
            bitmapImg.EndInit();
            
            img.Source = bitmapImg;

            TextBlock txt =new TextBlock();
            txt.Text = path;
            GridEdit.Children.Add(img);
        }

        


        private void save(object sender, RoutedEventArgs e)
        {
            // TODO add Img attribute, check existing Names
      
            ImagePath = Image_Selected.Text;
            

        }



        private void selectImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png) |JPEG Files (*.jpeg)|*.jpeg|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                Image_Selected.Text = filename;
                ImagePath = filename;
                

            }

        }

        private void Name_Box_GotFocus(object sender, RoutedEventArgs e)
        {
            Name_Box.Text = "";
        }

        private void Name_Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InitBlock()
        {
            BlockViews.Add(new BlockView("Restart Computer", new Blocks.B_Restart()));
            BlockViews.Add(new BlockView("Run Application", new Blocks.B_RunApp("Run Application", "", "", "notepad.exe")));
            BlockViews.Add(new BlockView("Wait", new Blocks.B_Wait("Wait", "", "", 0, 0, 0)));
            BlockViews.Add(new BlockView("Capture", new Capture("Capture", "", "", 1)));

            foreach (BlockView blockView in BlockViews)
            {
                blockView.Btn_Delete.Click += OnClick_Delete;
                blockView.Btn_Edit.Click += OnClick_Edit;
                blockView.Btn_Up.Click += OnClick_Up;
                blockView.Btn_Down.Click += OnClick_Down;
                listTest.Items.Add(blockView.Content);
            }
            BlockViews[0].Content.Children[3].Visibility = Visibility.Hidden;
            BlockViews[BlockViews.Count - 1].Content.Children[4].Visibility = Visibility.Hidden;
        }

        private void OnClick_Delete(object sender, RoutedEventArgs e)
        {
            int currentItemPos = listTest.Items.IndexOf(((Button)sender).Parent);
            if (listTest.Items.Count > 1 && currentItemPos == 0)
            {
                Grid nextItem = (Grid)listTest.Items.GetItemAt(currentItemPos + 1);
                nextItem.Children[3].Visibility = Visibility.Hidden;
            }

            if (listTest.Items.Count > 1 && currentItemPos == listTest.Items.Count - 1)
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
