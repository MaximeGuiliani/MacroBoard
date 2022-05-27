using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using static MacroBoard.Utils;
using System.Windows.Data;
using System.Collections.Generic;


namespace MacroBoard.View
{
    public partial class EW : Window
    {
        public  ObservableCollection<Block> RightBlocks { get; set; } // this.RightBlocks <==> this.WorkFlow.workflowList
        public ObservableCollection<Block> LeftBlocks { get; set; }
        public WorkFlow WorkFlow;
        private string placeHolderImagePath  = "Select folder";
        private string placeHolderWFName     = "Select name";
        public bool isex { get; set; }
        

        /*constructor for new workflow*/
        public EW()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DataContext = this;
            LeftBlocks  = new();

            RightBlocks = new();
            this.WorkFlow = new WorkFlow("", "", RightBlocks);

            InitializeComponent();
            setupLeftBlocks();
            setupKeyboardInteractions();
            RightBlocks.CollectionChanged += refresh;
            ((Window)this).Loaded += initRefresh;
            ((Window)this).Loaded += initExpander;
        }


        /*constructor for existing workflow */
        public EW(WorkFlow modelWorkFlow)
        {
            ((Window)this).Loaded += initRefresh;
            ((Window)this).Loaded += initExpander;
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            DataContext = this;
            LeftBlocks  = new();
            this.RightBlocks = new ObservableCollection<Block>(modelWorkFlow.workflowList);
            this.WorkFlow    = new WorkFlow(modelWorkFlow.imagePath, modelWorkFlow.workflowName, RightBlocks);
            InitializeComponent();
            setupLeftBlocks();
            setupBottom(modelWorkFlow);
            setupKeyboardInteractions();
            RightBlocks.CollectionChanged += refresh;
        }


        private void setupBottom(WorkFlow modelWorkFlow)
        {
            if (!WorkFlow.imagePath.Equals(""))
            {
                Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(WorkFlow.imagePath, UriKind.Absolute));
                TextBox_WorkFlowImage.Text = WorkFlow.imagePath;

            }
            TextBox_WorkFlowName.Text = "name";//this.WorkFlow.workflowName; // TODOOOOO
        }


        private void setupLeftBlocks()
        {
            LeftBlocks.Add(new BlockClickL());
            LeftBlocks.Add(new BlockClickR());
            LeftBlocks.Add(new BlockCloseDesiredApplication(""));
            LeftBlocks.Add(new BlockCopy(@"C:\", @"C:\"));
            LeftBlocks.Add(new BlockCreateTextFile(@"C:\", "fileName", "blabla"));
            LeftBlocks.Add(new BlockDeleteDirectory(@"C:\"));
            LeftBlocks.Add(new BlockDownloadFile(@"http:\\", @"C:\"));
            LeftBlocks.Add(new BlockHibernate());
            LeftBlocks.Add(new BlockInvokeAutomationId(""));
            LeftBlocks.Add(new BlockKeyBoard(""));
            LeftBlocks.Add(new BlockLaunchBrowserChrome(@"https:\\"));
            LeftBlocks.Add(new BlockLaunchBrowserFirefox(@"https:\\"));
            LeftBlocks.Add(new BlockLaunchBrowserEdge(@"https:\\"));
            LeftBlocks.Add(new BlockLock());
            LeftBlocks.Add(new BlockMessageBox("a", "b"));
            LeftBlocks.Add(new BlockMove(@"C:\", @"C:\"));
            LeftBlocks.Add(new BlockRecognition(""));
            LeftBlocks.Add(new BlockRestart());
            LeftBlocks.Add(new BlockLaunchApp(""));
            LeftBlocks.Add(new BlockRunScript(""));
            LeftBlocks.Add(new BlockScreenshot(@"C:\", "filename", 0));
            LeftBlocks.Add(new BlockSendEmail("", "", ""));
            LeftBlocks.Add(new BlockSetCursor(0, 0));
            LeftBlocks.Add(new BlockShutdown());
            LeftBlocks.Add(new BlockWait(0, 0, 0));
            ListBlock_Left_XAML.ItemsSource = LeftBlocks;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListBlock_Left_XAML.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("category");
            view.GroupDescriptions.Add(groupDescription);

        }


        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("category");
            string searchText = Search.Text;
            ObservableCollection<Block> LeftBlocksSearch = new ObservableCollection<Block>();
            if (!searchText.Equals(""))
            {
                foreach (Block block in LeftBlocks)
                {
                    if (block.Name.ToLower().Contains(searchText.ToLower()))
                        LeftBlocksSearch.Add(block);

                }
                ListBlock_Left_XAML.ItemsSource = LeftBlocksSearch;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListBlock_Left_XAML.ItemsSource);
                view.GroupDescriptions.Add(groupDescription);
            }
            else
            {
                LeftBlocksSearch = LeftBlocks;

                ListBlock_Left_XAML.ItemsSource = LeftBlocksSearch;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListBlock_Left_XAML.ItemsSource);
                view.GroupDescriptions.Remove(groupDescription);


            }
            

        }

       





        private void setupKeyboardInteractions()
        {
            ListBlock_Right_XAML.KeyDown += ListRightCopy;
            ListBlock_Right_XAML.KeyDown += ListRightPaste;
            ListBlock_Right_XAML.KeyDown += ListRightSupp;
            ListBlock_Left_XAML.KeyDown  += LeftListPlus;
        }


        private void onClickUp(object sender, RoutedEventArgs e)
        {
            Block TriggerBlock      = (Block)((Button)sender).DataContext;
            int   TriggerBlockIndex = RightBlocks.IndexOf(TriggerBlock);
            MoveBlockUp(TriggerBlockIndex);
        }

        private bool MoveBlockUp(int indexBlock)
        {
            if (indexBlock < 1 || indexBlock>=RightBlocks.Count) return false;
            Block mustGoUp   = RightBlocks[indexBlock];
            Block mustGoDown = RightBlocks[indexBlock-1];
            RightBlocks[indexBlock - 1] = mustGoUp;
            RightBlocks[indexBlock]     = mustGoDown;
            return true;
        }


        private void onClickDown(object sender, RoutedEventArgs e)
        {
            Block TriggerBlock      = (Block)((Button)sender).DataContext;
            int   TriggerBlockIndex = RightBlocks.IndexOf(TriggerBlock);
            MoveBlockDown(TriggerBlockIndex);
        }

        private bool MoveBlockDown(int indexBlock)
        {
            if (indexBlock >= RightBlocks.Count - 1 || indexBlock < 0) return false;
            Block mustGoUp = RightBlocks[indexBlock + 1];
            Block mustGoDown = RightBlocks[indexBlock];
            RightBlocks[indexBlock] = mustGoUp;
            RightBlocks[indexBlock + 1] = mustGoDown;
            return true;
        }


        private void onClickDelete(object sender, RoutedEventArgs e)
        {
            Block TriggerBlock = (Block)((Button)sender).DataContext;
            int TriggerBlockIndex = RightBlocks.IndexOf(TriggerBlock);
            DeleteBlock(TriggerBlockIndex);
        }

        private bool DeleteBlock(int indexBlock)
        {
            if (indexBlock<0 || indexBlock>=RightBlocks.Count) return false;
            RightBlocks.RemoveAt(indexBlock);
            return true;
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
            //MessageBox.Show($"remplissez tout"); //TODO
        }


        private void selectImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All Images (.jpeg .jpg .png .gif)|*.jpeg;*.jpg;*.png;*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Img_WorkFlowImage.ImageSource = new BitmapImage(new Uri(dlg.FileName, UriKind.Absolute));
                TextBox_WorkFlowImage.Text = dlg.FileName;
            }
        }


       
        private void refresh(object? sender, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < RightBlocks.Count; i++)
            {
                //MessageBox.Show(ListBlock_Right_XAML.Items.Count.ToString());
                ListBoxItem myListBoxItem = (ListBoxItem)ListBlock_Right_XAML.ItemContainerGenerator.ContainerFromItem(ListBlock_Right_XAML.Items[i]);
                if (myListBoxItem == null)
                {
                    ListBlock_Right_XAML.UpdateLayout();
                    myListBoxItem = (ListBoxItem)ListBlock_Right_XAML.ItemContainerGenerator.ContainerFromItem(ListBlock_Right_XAML.Items[i]);
                }
                if (myListBoxItem == null) MessageBox.Show("nulllll");
                ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);
                DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                Grid BlockGrid = (Grid)myDataTemplate.FindName("RightGrid", myContentPresenter);
                if (RightBlocks[i].GetType().GetConstructor(Type.EmptyTypes) == null)
                    BlockGrid.Children[4].Visibility = Visibility.Visible;
                else BlockGrid.Children[4].Visibility = Visibility.Hidden;
                if (i == 0)
                    BlockGrid.Children[0].Visibility = Visibility.Hidden;
                else
                    BlockGrid.Children[0].Visibility = Visibility.Visible;
                if (i == RightBlocks.Count - 1)
                    BlockGrid.Children[1].Visibility = Visibility.Hidden;
                else
                    BlockGrid.Children[1].Visibility = Visibility.Visible;
            }
        }


        private void initRefresh(object sender, RoutedEventArgs e)
        {
            refresh(null, null);
        }


        private void addRightBlock(Block model)
        {
            bool mustCreateWindow = model.GetType().GetConstructor(Type.EmptyTypes) == null;
            
            if (mustCreateWindow)
            {
                BlockCreatorWindow blockCreatorWindow = new BlockCreatorWindow(model);
                blockCreatorWindow.ShowDialog();
                if (blockCreatorWindow.DialogResult == false) return;
                RightBlocks.Add(blockCreatorWindow.res);
                return;
            }
            Block newBlock = (Block)model.GetType().GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            RightBlocks.Add(newBlock);
        }




        private void Name_Box_GotFocus(object sender, RoutedEventArgs e) //TODO a utiliser
        {
            if (TextBox_WorkFlowName.Text.Equals(placeHolderWFName))
                TextBox_WorkFlowName.Text = "";
            TextBox_WorkFlowName.Foreground = new SolidColorBrush(Colors.Black);
        }


        private void ListRightSupp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back || e.Key == Key.Delete) && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                RightBlocks.RemoveAt(ListBlock_Right_XAML.SelectedIndex);
            }
        }


        private void ListRightCopy(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                string dataFormat = "Block";
                Clipboard.SetData(dataFormat, ListBlock_Right_XAML.SelectedItem);//cast en block ?
            }
        }


        private void ListRightPaste(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                string dataFormat = "Block";
                if (!Clipboard.ContainsData(dataFormat)) return;
                Block pasteBlock = (Block)Clipboard.GetData(dataFormat);
                RightBlocks.Add(pasteBlock);
            }
        }


        private void LeftListPlus(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ListBlock_Left_XAML.SelectedItems.Count > 0)
            {
                Block model = (Block)ListBlock_Left_XAML.SelectedItem;
                addRightBlock(model);
            }
        }


        private void debug_Click(object sender, RoutedEventArgs e)
        {
            isex = !isex;
            expandAll(isex);
        }

        private void expandAll(bool visibility)
        {
            foreach (GroupItem gi in FindVisualChildren<GroupItem>(ListBlock_Left_XAML))
                gi.Tag = visibility;
        }

        private void initExpander(object sender, RoutedEventArgs e)
        {
            expandAll(true);
        }


       

        private void OnDoubleClickAdd(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                Block model = (Block)((TextBlock)sender).DataContext;
                addRightBlock(model);
            
            }

        }

        private void OnClickInfo(object sender, RoutedEventArgs e)
        {
            Block model = (Block)((Button)sender).DataContext;
            MessageBox.Show(model.info);
            

        }

        private void OnDoubleClickEdit(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                Block model = (Block)((TextBlock)sender).DataContext;
                int modelIndex = RightBlocks.IndexOf(model);
                bool mustCreateWindow = model.GetType().GetConstructor(Type.EmptyTypes) == null;
                if (!mustCreateWindow) return;

                BlockCreatorWindow blockCreatorWindow = new BlockCreatorWindow(model);
                blockCreatorWindow.ShowDialog();
                if (blockCreatorWindow.DialogResult == false) return;

                RightBlocks[modelIndex] = blockCreatorWindow.res;


            }
            
            

        }
    }
}        // il n'y a que 2 fleches a supprimer: celle toute en haut, celle tout en bas (update a delete et insert)
