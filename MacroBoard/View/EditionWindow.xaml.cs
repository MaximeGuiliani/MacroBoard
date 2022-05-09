using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MacroBoard.View;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for EditionWindow.xaml
    /// </summary>
    public partial class EditionWindow : Window
    {
        WorkFlow WorkFlow = new WorkFlow("", "", new());

        private List<BlockViewModel_All> BlockViewModels_Left = new();
        private List<BlockViewModel_Workflow> BlockViewModels_Right = new();
        public EditionWindow()
        {
            InitializeComponent();
            InitListBlock_All();
            DataContext = this;
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/macro_img.png", UriKind.Absolute));
        }

        public EditionWindow(WorkFlow workFlow)
        {
            InitializeComponent();
            this.WorkFlow = workFlow;
            InitListBlock_Workflow();
            //Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(workFlow.imagePath, UriKind.Absolute));
        }

        private void InitListBlock_All()
        {
            BlockViewModels_Left.Add(new BlockViewModel_All("Restart Computer", new BlockRestart()));
            BlockViewModels_Left.Add(new BlockViewModel_All("Run Application", new BlockRunApp("notepad.exe")));
            BlockViewModels_Left.Add(new BlockViewModel_All("Wait", new BlockWait(0, 0, 0)));
            BlockViewModels_Left.Add(new BlockViewModel_All("Capture", new BlockScreenshot("", 1)));


            foreach (BlockViewModel_All blockView in BlockViewModels_Left)
            {
                blockView.Btn_Add.Click += OnClick_Add;
                ListBlock_Left_XAML.Items.Add(blockView.Content);
            }

        }

        private void InitListBlock_Workflow()
        {
            foreach(Block Block in WorkFlow.workflowList)
            {
                BlockViewModel_Workflow CurrentBlockViewModel = new BlockViewModel_Workflow(Block.Name, Block); //wrapper de block à droite
                CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
                CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;
                CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
                CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;

                ListBlock_Right_XAML.Items.Add(CurrentBlockViewModel.Content);
                BlockViewModels_Right.Add(CurrentBlockViewModel);
            }
            MessageBox.Show(BlockViewModels_Right[0].Content.Children.Count.ToString());
            BlockViewModels_Right[0].Content.Children[3].Visibility = Visibility.Hidden;
            BlockViewModels_Right[^1].Content.Children[4].Visibility = Visibility.Hidden;
        }


        private void OnClick_Add(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Left_XAML.Items.IndexOf(((Button)sender).Parent);

            //---------------------------------------------------------------------------
            Block[] res = new Block[1];
            Block model = BlockViewModels_Left[currentItemPos].Block;
            Window blockCreatorWindow = new BlockCreatorWindow(res, model);
            blockCreatorWindow.ShowDialog();
            Block? newBlock = null;

            if (blockCreatorWindow.DialogResult == false)
                return;
            newBlock= res[0];
                //MessageBox.Show($"{(newBlock as BlockScreenshot).screenNumber}");

            //---------------------------------------------------------------------------


            BlockViewModel_Workflow CurrentBlockViewModel = new BlockViewModel_Workflow(BlockViewModels_Left[currentItemPos].Block.Name, newBlock); //wrapper de block à droite
            CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
            CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;
            CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
            CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;

            WorkFlow.workflowList.Add(BlockViewModels_Left[currentItemPos].Block);
            ListBlock_Right_XAML.Items.Add(CurrentBlockViewModel.Content);
            BlockViewModels_Right.Add(CurrentBlockViewModel);

            if (WorkFlow.workflowList.Count <= 1)
            {
                CurrentBlockViewModel.Btn_Up.Visibility = Visibility.Hidden;
                CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Hidden;
            }
            else CurrentBlockViewModel.Btn_Up.Visibility = Visibility.Visible;

            if (WorkFlow.workflowList.Count == 2)
            {
                ((Grid)ListBlock_Right_XAML.Items[0]).Children[4].Visibility = Visibility.Visible;
            }

            if (WorkFlow.workflowList.Count > 2)
                ((Grid)ListBlock_Right_XAML.Items[ListBlock_Right_XAML.Items.Count - 2]).Children[4].Visibility = Visibility.Visible;

            CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Hidden;

        }



        private void OnClick_Delete(object sender, RoutedEventArgs e)
        {
            Grid CurrentBlockContent = (Grid)((Button)sender).Parent;
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);
            if (ListBlock_Right_XAML.Items.Count > 1 && currentItemPos == 0)
            {
                Grid nextItem = (Grid)ListBlock_Right_XAML.Items.GetItemAt(currentItemPos + 1);
                nextItem.Children[3].Visibility = Visibility.Hidden;
            }

            if (ListBlock_Right_XAML.Items.Count > 1 && currentItemPos == ListBlock_Right_XAML.Items.Count - 1)
            {
                Grid previousItem = (Grid)ListBlock_Right_XAML.Items.GetItemAt(currentItemPos - 1);
                previousItem.Children[4].Visibility = Visibility.Hidden;
            }

            ListBlock_Right_XAML.Items.RemoveAt(currentItemPos);
            WorkFlow.workflowList.RemoveAt(currentItemPos);
            BlockViewModels_Right.RemoveAt(currentItemPos);
        }
        private void OnClick_Edit(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);
            Block[] res = new Block[1];
            
            Block model = BlockViewModels_Right[currentItemPos].Block;


            Window blockCreatorWindow = new BlockCreatorWindow(res, model);
            blockCreatorWindow.ShowDialog();

            Block? newBlock = null;

            if (blockCreatorWindow.DialogResult == false)
                return;
          
            newBlock = res[0];

                
            BlockViewModel_Workflow CurrentBlockViewModel = new BlockViewModel_Workflow(BlockViewModels_Right[currentItemPos].Block.Name, newBlock); //wrapper de block à droite
                
            WorkFlow.workflowList.RemoveAt(currentItemPos); 
            WorkFlow.workflowList.Insert(currentItemPos, CurrentBlockViewModel.Block);

            BlockViewModels_Right.RemoveAt(currentItemPos);
            BlockViewModels_Right.Insert(currentItemPos, CurrentBlockViewModel);

            if (WorkFlow.workflowList.Count <= 1)
            {
                CurrentBlockViewModel.Btn_Up.Visibility = Visibility.Hidden;
                CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Hidden;
            }
            else CurrentBlockViewModel.Btn_Up.Visibility = Visibility.Visible;

            if (WorkFlow.workflowList.Count == 2)
            {
                ((Grid)ListBlock_Right_XAML.Items[0]).Children[4].Visibility = Visibility.Visible;
            }

            if (WorkFlow.workflowList.Count > 2)
                ((Grid)ListBlock_Right_XAML.Items[ListBlock_Right_XAML.Items.Count - 2]).Children[4].Visibility = Visibility.Visible;

            CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Hidden;


        }
        private void OnClick_Up(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);
            Grid previousItem = (Grid)ListBlock_Right_XAML.Items.GetItemAt(currentItemPos - 1);

            SetHiddenOrVisibleBtnUp(sender, previousItem);

            ListBlock_Right_XAML.Items.Remove(((Button)sender).Parent);
            ListBlock_Right_XAML.Items.Remove(previousItem);

            ListBlock_Right_XAML.Items.Insert(currentItemPos - 1, ((Button)sender).Parent);
            ListBlock_Right_XAML.Items.Insert(currentItemPos, previousItem);
        }
        private void OnClick_Down(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);
            Grid nextItem = (Grid)ListBlock_Right_XAML.Items.GetItemAt(currentItemPos + 1);

            SetHiddenOrVisibleBtnDown(sender, nextItem);

            ListBlock_Right_XAML.Items.Remove(((Button)sender).Parent);
            ListBlock_Right_XAML.Items.Remove(nextItem);

            ListBlock_Right_XAML.Items.Insert(currentItemPos, nextItem);
            ListBlock_Right_XAML.Items.Insert(currentItemPos + 1, ((Button)sender).Parent);
        }

        private void SetHiddenOrVisibleBtnUp(object sender, Grid previousItem)
        {
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);
            Grid currentItem = (Grid)ListBlock_Right_XAML.Items.GetItemAt(currentItemPos);

            currentItem.Children[4].Visibility = Visibility.Visible;
            previousItem.Children[3].Visibility = Visibility.Visible;

            if (currentItemPos - 1 == 0)
            {
                currentItem.Children[3].Visibility = Visibility.Hidden;
            }

            if (currentItemPos == ListBlock_Right_XAML.Items.Count - 1)
            {
                previousItem.Children[4].Visibility = Visibility.Hidden;
            }
        }

        private void SetHiddenOrVisibleBtnDown(object sender, Grid nextItem)
        {
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);
            Grid currentItem = (Grid)ListBlock_Right_XAML.Items.GetItemAt(currentItemPos);

            currentItem.Children[3].Visibility = Visibility.Visible;
            nextItem.Children[4].Visibility = Visibility.Visible;

            if (currentItemPos + 1 == ListBlock_Right_XAML.Items.Count - 1)
            {
                currentItem.Children[4].Visibility = Visibility.Hidden;
            }

            if (currentItemPos == 0)
            {
                nextItem.Children[3].Visibility = Visibility.Hidden;
            }

        }


        private void Button_Save(object sender, RoutedEventArgs e)
        {
            this.WorkFlow.imagePath = TextBox_WorkFlowImage.Text;
            this.WorkFlow.workflowName = TextBox_WorkFlowName.Text;

            string JsonPath = AppDomain.CurrentDomain.BaseDirectory + "/BLOCK_JSON_TEST.json";

            Serialization serialization = new Serialization(JsonPath);

            serialization.Serialize(WorkFlow);
        }

        private void selectImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".jpeg";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|" +
                "All Files (.jpeg .jpg .png .gif)|*.jpeg;*.jpg;*.png;*.gif|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(dlg.FileName, UriKind.Absolute));
                TextBox_WorkFlowImage.Text = dlg.FileName;
            }

        }

        private void Name_Box_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox_WorkFlowName.Text = "";
            TextBox_WorkFlowName.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Name_Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



    }
}
