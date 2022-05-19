using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        public WorkFlow WorkFlow = new WorkFlow("", "", new());

        private List<BlockViewModel_Left> BlockViewModels_Left = new();
        private List<BlockViewModel_Right> BlockViewModels_Right = new();

        private string placeHolderImagePath = "Select folder";
        private string placeHolderWFName = "Select name";

        public EditionWindow()
        {
            InitializeComponent();
            InitListBlock_All();
            initCategories();
            InitKeyboardInteractions();

            DataContext = this;
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/macro_img.png", UriKind.Absolute));
            Research reasearch = new(BlockViewModels_Left, ListBlock_Left_XAML);
        }
        public EditionWindow(WorkFlow workFlow)
        {
            InitializeComponent();
            InitKeyboardInteractions();
            InitListBlock_All();

            this.WorkFlow.workflowList = new(workFlow.workflowList);
            this.WorkFlow.imagePath = workFlow.imagePath;
            this.WorkFlow.workflowName = workFlow.workflowName;

            InitListBlock_Workflow();

            if (!WorkFlow.imagePath.Equals(""))
            {
                Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(WorkFlow.imagePath, UriKind.Absolute));

            }
            if (!WorkFlow.imagePath.Equals(""))
            {

                TextBox_WorkFlowImage.Text = WorkFlow.imagePath;
            }
            TextBox_WorkFlowName.Text = WorkFlow.workflowName;

        }

        private void InitListBlock_All()
        {
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockClickL()));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockClickR()));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockCloseDesiredApplication("")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockCopy(@"C:\", @"C:\")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockCreateTextFile(@"C:\", "fileName", "blabla")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockDeleteDirectory(@"C:\")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockDownloadFile(@"http:\\", @"C:\")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockHibernate()));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockInvokeAutomationId("")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockKeyBoard("")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockLaunchBrowserChrome(@"https:\\")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockLaunchBrowserFirefox(@"https:\\")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockLaunchBrowserEdge(@"https:\\")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockLock()));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockMessageBox("a", "b")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockMove(@"C:\", @"C:\")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockRecognition("")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockRestart()));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockLaunchApp("")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockRunScript("")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockScreenshot(@"C:\", "filename", 0)));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockSendEmail("", "", "")));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockSetCursor(0, 0)));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockShutdown()));
            BlockViewModels_Left.Add(new BlockViewModel_Left(new BlockWait(0, 0, 0)));

            foreach (BlockViewModel_Left blockView in BlockViewModels_Left)
            {
                blockView.Btn_Add.Click += OnClick_Add;
                //Categories_XAML.Items.Add(blockView.Content);
                //ListBlock_Left_XAML.Items.Add(blockView.Content);
                //ListBlock_Left_XAML.Visibility = Visibility.Hidden;
            }

        }

        private void initCategories(){

            foreach (string cat in Enum.GetNames(typeof(Block.Categories)))
            {
                TreeViewItem item = new();
                item.Header = cat;
                Categories_XAML.Items.Add(item);

                foreach (BlockViewModel_Left blockView in BlockViewModels_Left)
                {
                    if (blockView.Block.category.ToString() == cat)
                        item.Items.Add(blockView.Content);
                        
                }
            }
        }

        private void InitListBlock_Workflow()
        {
            foreach (Block Block in WorkFlow.workflowList)
            {
                BlockViewModel_Right CurrentBlockViewModel = new BlockViewModel_Right(Block);
                CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
                CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;
                CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
                CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;

                ListBlock_Right_XAML.Items.Add(CurrentBlockViewModel.Content);
                BlockViewModels_Right.Add(CurrentBlockViewModel);
            }
            if (BlockViewModels_Right.Count > 0)
            {
                BlockViewModels_Right[0].Content.Children[3].Visibility = Visibility.Hidden;
                BlockViewModels_Right[^1].Content.Children[4].Visibility = Visibility.Hidden;
            }

        }
        private void OnClick_Add(object sender, RoutedEventArgs e)
        {
            
            int currentItemPos = ListBlock_Left_XAML.Items.IndexOf(((Button)sender).Parent);

            //---------------------------------------------------------------------------

            Block model = BlockViewModels_Left[currentItemPos].Block;
            Block? newBlock = model;

            if (model.GetType().GetConstructor(Type.EmptyTypes) == null)
            {
                BlockCreatorWindow blockCreatorWindow = new BlockCreatorWindow(model);
                blockCreatorWindow.ShowDialog();
                if (blockCreatorWindow.DialogResult == false)
                    return;
                newBlock = blockCreatorWindow.res;
            }

            //---------------------------------------------------------------------------


            BlockViewModel_Right CurrentBlockViewModel = new BlockViewModel_Right(newBlock); //wrapper de block à droite

            CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
            CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;
            CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
            CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;

            WorkFlow.workflowList.Add(newBlock);
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
            //---------------------------------------------------------------------------

            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);

            //---------------------------------------------------------------------------

            Block model = BlockViewModels_Right[currentItemPos].Block;
            bool mustCreateWindow = model.GetType().GetConstructor(Type.EmptyTypes) == null;
            Block newBlock = model;

            if (!mustCreateWindow)
                return;

            BlockCreatorWindow blockCreatorWindow = new BlockCreatorWindow(model);
            blockCreatorWindow.ShowDialog();
            if (blockCreatorWindow.DialogResult == false)
                return;

            newBlock = blockCreatorWindow.res;

            BlockViewModel_Right CurrentBlockViewModel = new BlockViewModel_Right(newBlock);
            CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
            CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;
            CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
            CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;

            WorkFlow.workflowList.RemoveAt(currentItemPos);
            WorkFlow.workflowList.Insert(currentItemPos, CurrentBlockViewModel.Block);

            BlockViewModels_Right.RemoveAt(currentItemPos);
            BlockViewModels_Right.Insert(currentItemPos, CurrentBlockViewModel);

        }
        private void OnClick_Up(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);

            SetHiddenOrVisibleBtnUp(sender, BlockViewModels_Right[currentItemPos - 1].Content);
            SwitchBlockPosition(currentItemPos, currentItemPos - 1);
        }
        private void OnClick_Down(object sender, RoutedEventArgs e)
        {
            int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(((Button)sender).Parent);

            SetHiddenOrVisibleBtnDown(sender, BlockViewModels_Right[currentItemPos + 1].Content);
            SwitchBlockPosition(currentItemPos + 1, currentItemPos);
        }
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            if (!(TextBox_WorkFlowName.Text == placeHolderWFName) && TextBox_WorkFlowName.Text != "")
            {
                if (TextBox_WorkFlowImage.Text.Equals(placeHolderImagePath))
                {
                    this.WorkFlow.imagePath = "";

                }
                else
                {
                    this.WorkFlow.imagePath = TextBox_WorkFlowImage.Text;

                }
                this.WorkFlow.workflowName = TextBox_WorkFlowName.Text;
                this.DialogResult = true;
                this.Close();
            }

        }

        private void SwitchBlockPosition(int pos1, int pos2)
        {
            BlockViewModel_Right pos1BlockViewModels_Right = BlockViewModels_Right[pos1];
            BlockViewModel_Right pos2BlockViewModels_Right = BlockViewModels_Right[pos2];

            ListBlock_Right_XAML.Items.RemoveAt(pos1);
            ListBlock_Right_XAML.Items.RemoveAt(pos2);
            ListBlock_Right_XAML.Items.Insert(pos2, pos1BlockViewModels_Right.Content);
            ListBlock_Right_XAML.Items.Insert(pos1, pos2BlockViewModels_Right.Content);

            BlockViewModels_Right.RemoveAt(pos1);
            BlockViewModels_Right.RemoveAt(pos2);
            BlockViewModels_Right.Insert(pos2, pos1BlockViewModels_Right);
            BlockViewModels_Right.Insert(pos1, pos2BlockViewModels_Right);

            Block previousBlock = WorkFlow.workflowList[pos2];
            Block currentBlock = WorkFlow.workflowList[pos1];

            WorkFlow.workflowList.RemoveAt(pos1);
            WorkFlow.workflowList.RemoveAt(pos2);
            WorkFlow.workflowList.Insert(pos2, previousBlock);
            WorkFlow.workflowList.Insert(pos1, currentBlock);
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
            if (TextBox_WorkFlowName.Text.Equals(placeHolderWFName))
            {
                TextBox_WorkFlowName.Text = "";

            }
            TextBox_WorkFlowName.Foreground = new SolidColorBrush(Colors.Black);
        }


        //Shortcut
        private void InitKeyboardInteractions()
        {
            ListBlock_Right_XAML.KeyDown += ListRightCopy;
            ListBlock_Right_XAML.KeyDown += ListRightPaste;
            ListBlock_Right_XAML.KeyDown += ListRightSupp;
            ListBlock_Left_XAML.KeyDown += ListLeftAdd;
        }
        private void ListRightSupp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back || e.Key == Key.Delete) && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(ListBlock_Right_XAML.SelectedItem);
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
        }
        private void ListRightCopy(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(ListBlock_Right_XAML.SelectedItem);
                Block toCopyBlock = BlockViewModels_Right[currentItemPos].Block;
                string dataFormat = "Block";
                Clipboard.SetData(dataFormat, toCopyBlock);
            }
        }
        private void ListRightPaste(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                string dataFormat = "Block";
                int currentItemPos = ListBlock_Right_XAML.Items.IndexOf(ListBlock_Right_XAML.SelectedItem);

                if (!Clipboard.ContainsData(dataFormat)) return;
                Block newBlock = (Block)Clipboard.GetData(dataFormat);
                BlockViewModel_Right CurrentBlockViewModel = new BlockViewModel_Right(newBlock);

                CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
                CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;
                CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
                CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;

                WorkFlow.workflowList.Insert(currentItemPos + 1, newBlock);
                ListBlock_Right_XAML.Items.Insert(currentItemPos + 1, CurrentBlockViewModel.Content);
                BlockViewModels_Right.Insert(currentItemPos + 1, CurrentBlockViewModel);

                CurrentBlockViewModel.Btn_Up.Visibility = Visibility.Visible;
                CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Visible;

                if (currentItemPos + 1 == ListBlock_Right_XAML.Items.Count - 1)
                {
                    CurrentBlockViewModel.Btn_Down.Visibility = Visibility.Hidden;
                    ((Button)((Grid)ListBlock_Right_XAML.Items[^2]).Children[4]).Visibility = Visibility.Visible;
                }
                    
                    

            }
        }
        private void ListLeftAdd(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ListBlock_Left_XAML.SelectedItems.Count > 0)
            {
                int currentItemPos = ListBlock_Left_XAML.Items.IndexOf(ListBlock_Left_XAML.SelectedItem);

                Block model = BlockViewModels_Left[currentItemPos].Block;
                bool mustCreateWindow = model.GetType().GetConstructor(Type.EmptyTypes) == null;
                Block? newBlock = model;

                if (mustCreateWindow)
                {
                    BlockCreatorWindow blockCreatorWindow = new BlockCreatorWindow(model);
                    blockCreatorWindow.ShowDialog();
                    if (blockCreatorWindow.DialogResult == false)
                        return;
                    newBlock = blockCreatorWindow.res;
                }

                BlockViewModel_Right CurrentBlockViewModel = new BlockViewModel_Right(newBlock);
                CurrentBlockViewModel.Btn_Delete.Click += OnClick_Delete;
                CurrentBlockViewModel.Btn_Edit.Click += OnClick_Edit;
                CurrentBlockViewModel.Btn_Up.Click += OnClick_Up;
                CurrentBlockViewModel.Btn_Down.Click += OnClick_Down;

                WorkFlow.workflowList.Add(newBlock);
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
        }

    }
}
