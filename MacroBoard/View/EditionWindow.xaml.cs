using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        WorkFlow WorkFlow = new WorkFlow("", "", new());

        private List<BlockViewModel> BlockViewModels = new();

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
            Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(workFlow.imagePath, UriKind.Absolute));
        }

        private void InitListBlock_All()
        {
            BlockViewModels.Add(new BlockViewModel_All("Restart Computer", new Blocks.B_Restart()));
            BlockViewModels.Add(new BlockViewModel_All("Run Application", new Blocks.B_RunApp("Run Application", "", "", "notepad.exe")));
            BlockViewModels.Add(new BlockViewModel_All("Wait", new Blocks.B_Wait("Wait", "", "", 0, 0, 0)));
            BlockViewModels.Add(new BlockViewModel_All("Capture", new Capture("Capture", "", "", 1)));

            foreach (BlockViewModel_All blockView in BlockViewModels)
            {
                blockView.Btn_Add.Click += OnClick_Add;
                ListBlock_All.Items.Add(blockView.Content);
            }

        }

        private void InitListBlock_Workflow()
        {
            //BlockViews_Workflow[0].Content.Children[3].Visibility = Visibility.Hidden;
            //BlockViews_Workflow[BlockViews_Workflow.Count - 1].Content.Children[4].Visibility = Visibility.Hidden;
        }
        private void OnClick_Add(object sender, RoutedEventArgs e)
        {
            Grid CurrentBlockContent = (Grid)((Button)sender).Parent;
            Block CurrentBlock = new Blocks.B_Restart();

            ListBlock_All.Items.Remove(CurrentBlockContent);

            foreach (BlockViewModel_All blockView in BlockViewModels)
                if (blockView.Lbl_Name.Content == ((Label)CurrentBlockContent.Children[0]).Content)
                    CurrentBlock = blockView.Block;

            WorkFlow.workflowList.Add(CurrentBlock);
            
            BlockViewModel_Workflow CurrentBlockViewModel = new BlockViewModel_Workflow(CurrentBlock.Name, CurrentBlock);
            CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
            CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;
            CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
            CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;

            if (WorkFlow.workflowList.Count <= 1)
            {
                CurrentBlockViewModel.Btn_Up.Visibility = Visibility.Hidden;
                CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Hidden;
            }
            if(WorkFlow.workflowList.Count == 2)
                CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Hidden;



            ListBlock_Workflow.Items.Add(CurrentBlockViewModel.Content);
        }

        private void OnClick_Delete(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Workflow.Items.IndexOf(((Button)sender).Parent);
            if (ListBlock_Workflow.Items.Count > 1 && currentItemPos == 0)
            {
                Grid nextItem = (Grid)ListBlock_Workflow.Items.GetItemAt(currentItemPos + 1);
                nextItem.Children[3].Visibility = Visibility.Hidden;
            }

            if (ListBlock_Workflow.Items.Count > 1 && currentItemPos == ListBlock_Workflow.Items.Count - 1)
            {
                Grid previousItem = (Grid)ListBlock_Workflow.Items.GetItemAt(currentItemPos - 1);
                previousItem.Children[4].Visibility = Visibility.Hidden;
            }

            ListBlock_Workflow.Items.RemoveAt(currentItemPos);
        }
        private void OnClick_Edit(object sender, RoutedEventArgs e)
        {

        }
        private void OnClick_Up(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Workflow.Items.IndexOf(((Button)sender).Parent);
            Grid previousItem = (Grid)ListBlock_Workflow.Items.GetItemAt(currentItemPos - 1);

            SetHiddenOrVisibleBtnUp(sender, previousItem);

            ListBlock_Workflow.Items.Remove(((Button)sender).Parent);
            ListBlock_Workflow.Items.Remove(previousItem);

            ListBlock_Workflow.Items.Insert(currentItemPos - 1, ((Button)sender).Parent);
            ListBlock_Workflow.Items.Insert(currentItemPos, previousItem);
        }
        private void OnClick_Down(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Workflow.Items.IndexOf(((Button)sender).Parent);
            Grid nextItem = (Grid)ListBlock_Workflow.Items.GetItemAt(currentItemPos + 1);

            SetHiddenOrVisibleBtnDown(sender, nextItem);

            ListBlock_Workflow.Items.Remove(((Button)sender).Parent);
            ListBlock_Workflow.Items.Remove(nextItem);

            ListBlock_Workflow.Items.Insert(currentItemPos, nextItem);
            ListBlock_Workflow.Items.Insert(currentItemPos + 1, ((Button)sender).Parent);
        }

        private void SetHiddenOrVisibleBtnUp(object sender, Grid previousItem)
        {
            int currentItemPos = ListBlock_Workflow.Items.IndexOf(((Button)sender).Parent);
            Grid currentItem = (Grid)ListBlock_Workflow.Items.GetItemAt(currentItemPos);

            currentItem.Children[4].Visibility = Visibility.Visible;
            previousItem.Children[3].Visibility = Visibility.Visible;

            if (currentItemPos - 1 == 0)
            {
                currentItem.Children[3].Visibility = Visibility.Hidden;
            }

            if (currentItemPos == ListBlock_Workflow.Items.Count - 1)
            {
                previousItem.Children[4].Visibility = Visibility.Hidden;
            }
        }

        private void SetHiddenOrVisibleBtnDown(object sender, Grid nextItem)
        {
            int currentItemPos = ListBlock_Workflow.Items.IndexOf(((Button)sender).Parent);
            Grid currentItem = (Grid)ListBlock_Workflow.Items.GetItemAt(currentItemPos);

            currentItem.Children[3].Visibility = Visibility.Visible;
            nextItem.Children[4].Visibility = Visibility.Visible;

            if (currentItemPos + 1 == ListBlock_Workflow.Items.Count - 1)
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
