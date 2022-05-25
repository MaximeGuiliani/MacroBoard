using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;
using static MacroBoard.Utils;
using System.Collections.Generic;
using System.ComponentModel;


namespace MacroBoard.View
{
    public partial class EW : Window
    {
        public  ObservableCollection<Block> RightBlocks { get; set; } // this.RightBlocks <==> this.WorkFlow.workflowList
        public ObservableCollection<Block> LeftBlocks { get; set; }
        public WorkFlow WorkFlow;
        private string placeHolderImagePath  = "Select folder";
        private string placeHolderWFName     = "Select name";
        

//CONSTRUCTORS
//-----------------------------------------------------------------------------------------------

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
            RightBlocks.CollectionChanged += onCollectionChanged;
            ((Window)this).Loaded += initCollectionChanged;
            ((Window)this).Loaded += initSetExpanders;
        }


        /*constructor for existing workflow */
        public EW(WorkFlow modelWorkFlow)
        {
            ((Window)this).Loaded += initCollectionChanged;
            ((Window)this).Loaded += initSetExpanders;
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            DataContext = this;
            LeftBlocks  = new();
            this.RightBlocks = new ObservableCollection<Block>(modelWorkFlow.workflowList);
            this.WorkFlow    = new WorkFlow(modelWorkFlow.imagePath, modelWorkFlow.workflowName, RightBlocks);
            InitializeComponent();
            setupLeftBlocks();
            
            RightBlocks.CollectionChanged += onCollectionChanged;
            setupBottom(modelWorkFlow);
            setupKeyboardInteractions();
        }


//CONSTRUTOR METHODS
//-----------------------------------------------------------------------------------------------

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
            LeftBlocks.Add(new BlockCopyFile(@"C:\", @"C:\"));
            LeftBlocks.Add(new BlockCreateTextFile(@"C:\", "fileName", "blabla"));
            LeftBlocks.Add(new BlockDeleteDirectory(@"C:\"));
            LeftBlocks.Add(new BlockDeleteFile(@"C:\"));
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
            LeftBlocks.Add(new BlockMoveFile(@"C:\", @"C:\"));
            LeftBlocks.Add(new BlockRecognition(""));
            LeftBlocks.Add(new BlockRestart());
            LeftBlocks.Add(new BlockLaunchApp(""));
            LeftBlocks.Add(new BlockRunScript(""));
            LeftBlocks.Add(new BlockScreenshot(@"C:\", "filename", 0));
            LeftBlocks.Add(new BlockSendEmail("", "", ""));
            LeftBlocks.Add(new BlockSetCursor(0, 0));
            LeftBlocks.Add(new BlockShutdown());
            LeftBlocks.Add(new BlockWait(0, 0, 0));
            LeftBlocks.Add(new BlockWindowStyle());
            ListBlock_Left_XAML.ItemsSource = LeftBlocks;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListBlock_Left_XAML.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("category");
            view.GroupDescriptions.Add(groupDescription);

        }


        private void setupKeyboardInteractions()
        {
            ListBlock_Right_XAML.KeyDown += onKeyCopy;
            ListBlock_Right_XAML.KeyDown += onKeyPaste;
            ListBlock_Right_XAML.KeyDown += onKeyDelete;
            ListBlock_Right_XAML.KeyDown += onKeyEdit;
            ListBlock_Right_XAML.KeyDown += onKeyMoveUp;
            ListBlock_Right_XAML.KeyDown += onKeyMoveDown;
            ListBlock_Left_XAML.KeyDown  += onKeyAddBlock;
            this.KeyDown += onKeyExpandAll;
            this.KeyDown += onKeyCollapseAll;
        }


        private void initCollectionChanged(object sender, RoutedEventArgs e)
        {
            onCollectionChanged(null, null);
        }


        private void initSetExpanders(object sender, RoutedEventArgs e)
        {
            bool visibility = Config.Boolean("initExpandersEW");
            setExpanders(visibility);
        }

//HANDLERS
//-----------------------------------------------------------------------------------------------

        private void onClickUp(object sender, RoutedEventArgs e)
        {
            Block TriggerBlock      = (Block)((Button)sender).DataContext;
            int   TriggerBlockIndex = RightBlocks.IndexOf(TriggerBlock);
            MoveBlockUp(TriggerBlockIndex);
        }


        private void onClickDown(object sender, RoutedEventArgs e)
        {
            Block TriggerBlock = (Block)((Button)sender).DataContext;
            int TriggerBlockIndex = RightBlocks.IndexOf(TriggerBlock);
            MoveBlockDown(TriggerBlockIndex);
        }

        private void onClickDelete(object sender, RoutedEventArgs e)
        {
            Block TriggerBlock = (Block)((Button)sender).DataContext;
            int TriggerBlockIndex = RightBlocks.IndexOf(TriggerBlock);
            DeleteBlock(TriggerBlockIndex);
        }


        private void onClickEdit(object sender, RoutedEventArgs e)
        {
            Block model = (Block)((Button)sender).DataContext;
            EditBlock(model);
        }


        private void onClickSave(object sender, RoutedEventArgs e)
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


        private void onClickSelectImage(object sender, RoutedEventArgs e)
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

        private void onClickCollapseAll(object sender, RoutedEventArgs e)
        {
            setExpanders(false);
        }


        private void onClickExpandAll(object sender, RoutedEventArgs e)
        {
            setExpanders(true);
        }


        private void OnDoubleClickAdd(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                Block model = (Block)((TextBlock)sender).DataContext;
                addBlockOnRight(model);
            }
        }


        private void onGotFocusNameBox(object sender, RoutedEventArgs e) //TODO a utiliser
        {
            if (TextBox_WorkFlowName.Text.Equals(placeHolderWFName))
                TextBox_WorkFlowName.Text = "";
            TextBox_WorkFlowName.Foreground = new SolidColorBrush(Colors.Black);
        }


        private void onTextChangedSearch(object sender, TextChangedEventArgs e)
        {
            string searchText = Search.Text;
            ObservableCollection<Block> LeftBlocksSearch = new ObservableCollection<Block>();
            if (!searchText.Equals(""))
            {
                foreach (Block block in LeftBlocks)
                {
                    if (block.Name.Equals(searchText, StringComparison.OrdinalIgnoreCase))
                        LeftBlocksSearch.Add(block);
                }
            }
            else
            {
                LeftBlocksSearch = LeftBlocks;
            }
            ListBlock_Left_XAML.ItemsSource = LeftBlocksSearch;
        }


        private void onCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i<RightBlocks.Count; i++)
            {
                ListBoxItem listBoxItem = (ListBoxItem)ListBlock_Right_XAML.ItemContainerGenerator.ContainerFromItem(ListBlock_Right_XAML.Items[i]);
                if (listBoxItem == null)
                {
                    ListBlock_Right_XAML.UpdateLayout();
                    listBoxItem = (ListBoxItem)ListBlock_Right_XAML.ItemContainerGenerator.ContainerFromItem(ListBlock_Right_XAML.Items[i]);
                }
                if (listBoxItem == null)
                    MessageBox.Show("nulllll");
                ContentPresenter ContentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
                DataTemplate dataTemplate = ContentPresenter.ContentTemplate;
                Grid BlockGrid = (Grid)dataTemplate.FindName("RightGrid", ContentPresenter);
                if (RightBlocks[i].GetType().GetConstructor(Type.EmptyTypes) == null)
                    BlockGrid.Children[4].Visibility = Visibility.Visible; //bouton edit
                else BlockGrid.Children[4].Visibility = Visibility.Hidden;
                if (i == 0)
                    BlockGrid.Children[0].Visibility = Visibility.Hidden; //bouton up
                else
                    BlockGrid.Children[0].Visibility = Visibility.Visible;
                if (i == RightBlocks.Count - 1)
                    BlockGrid.Children[1].Visibility = Visibility.Hidden; //bouton down
                else
                    BlockGrid.Children[1].Visibility = Visibility.Visible;
            }
        }


//LOGIC
//-----------------------------------------------------------------------------------------------

        private bool MoveBlockUp(int indexBlock)
        {
            if (indexBlock < 1 || indexBlock>=RightBlocks.Count) return false;
            Block mustGoUp   = RightBlocks[indexBlock];
            Block mustGoDown = RightBlocks[indexBlock-1];
            RightBlocks[indexBlock - 1] = mustGoUp;
            RightBlocks[indexBlock]     = mustGoDown;
            return true;
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


        private bool DeleteBlock(int indexBlock)
        {
            if (indexBlock<0 || indexBlock>=RightBlocks.Count) return false;
            RightBlocks.RemoveAt(indexBlock);
            return true;
        }


        private void EditBlock(Block model)
        {
            int modelIndex = RightBlocks.IndexOf(model);
            bool mustCreateWindow = model.GetType().GetConstructor(Type.EmptyTypes) == null;
            if (!mustCreateWindow) return;

            BlockCreatorWindow blockCreatorWindow = new BlockCreatorWindow(model);
            blockCreatorWindow.ShowDialog();
            if (blockCreatorWindow.DialogResult == false) return;

            RightBlocks[modelIndex] = blockCreatorWindow.res;
        }


        private void addBlockOnRight(Block model)
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


        private void setExpanders(bool visibility)
        {
            foreach (GroupItem gi in FindVisualChildren<GroupItem>(ListBlock_Left_XAML))
                gi.Tag = visibility;
        }


//KEYBOARD HANDLERS 
//-----------------------------------------------------------------------------------------------

        private void onKeyDelete(object sender, KeyEventArgs e)
        {
            if (( e.Key == Key.Delete || e.Key == Key.E ) && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                int selectedIndex = ListBlock_Right_XAML.SelectedIndex;
                RightBlocks.RemoveAt(ListBlock_Right_XAML.SelectedIndex);
                
                if (ListBlock_Right_XAML.Items.Count <= 0)
                {
                    ListBlock_Left_XAML.Focus(); //TODO focus la barre de recherche
                    return;
                }

                int newSelectedIndex = Math.Min(ListBlock_Right_XAML.Items.Count - 1, selectedIndex);
                ListBlock_Right_XAML.SelectedIndex = newSelectedIndex;
                ListBoxItem? item = ListBlock_Right_XAML.ItemContainerGenerator.ContainerFromIndex(newSelectedIndex) as ListBoxItem;
                if(item!=null )item.Focus();
            }
        }


        private void onKeyCopy(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                string dataFormat = "Block";
                Clipboard.SetData(dataFormat, ListBlock_Right_XAML.SelectedItem);//cast en block ?
            }
        }


        private void onKeyPaste(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                string dataFormat = "Block";
                if (!Clipboard.ContainsData(dataFormat)) return;
                Block pasteBlock = (Block)Clipboard.GetData(dataFormat);
                RightBlocks.Add(pasteBlock);
            }
        }


        private void onKeyAddBlock(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ListBlock_Left_XAML.SelectedItems.Count > 0)
            {
                Block model = (Block)ListBlock_Left_XAML.SelectedItem;
                addBlockOnRight(model);
            }
        }


        private void onKeyEdit(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                Block model = (Block)ListBlock_Right_XAML.SelectedItem;
                EditBlock(model);
            }
        }


        private void onKeyExpandAll(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.H && ListBlock_Left_XAML.IsLoaded)
            {
                setExpanders(false);
            }
        }


        private void onKeyCollapseAll(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.J && ListBlock_Left_XAML.IsLoaded)
            {
                setExpanders(true);
            }
        }


        private void onKeyMoveUp(object sender, KeyEventArgs e)
        {
            if (ListBlock_Right_XAML.IsLoaded && e.Key == Key.Z && ListBlock_Right_XAML.SelectedItems.Count>0)
            {
                int indexBlock = ListBlock_Right_XAML.SelectedIndex;
                bool moved = MoveBlockUp(indexBlock);
                if (moved)
                {
                    int upperIndex = indexBlock - 1;
                    ListBlock_Right_XAML.SelectedIndex = upperIndex;
                    ListBoxItem? upperItem = ListBlock_Right_XAML.ItemContainerGenerator.ContainerFromIndex(upperIndex) as ListBoxItem;
                    if (upperItem != null) upperItem.Focus();
                }
            }
        }


        private void onKeyMoveDown(object sender, KeyEventArgs e)
        {
            if (ListBlock_Right_XAML.IsLoaded && e.Key == Key.S && ListBlock_Right_XAML.SelectedItems.Count > 0)
            {
                int indexBlock = ListBlock_Right_XAML.SelectedIndex;
                bool moved = MoveBlockDown(indexBlock);
                if (moved)
                {
                    int lowerIndex = indexBlock+1;
                    ListBlock_Right_XAML.SelectedIndex = lowerIndex;
                    ListBoxItem? lowerItem = ListBlock_Right_XAML.ItemContainerGenerator.ContainerFromIndex(lowerIndex) as ListBoxItem;
                    if (lowerItem != null) lowerItem.Focus();
                }
            }
        }














    }
}