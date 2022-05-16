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
using System.IO;
using static MacroBoard.Utils;



namespace MacroBoard.View
{

    public partial class BlockCreatorWindow : Window, IBlockVisitor
    {

        Func<Block> newBlock;   // lambda-expression appelée lors du click "valider", qui défini comment créer un block
        public Block res;       // nouveau block créé par la fenetre
        Block model;


        //---------------------------------------------------------------------------

        public BlockCreatorWindow(Block model)
        {
            InitializeComponent();
            this.model = model;
            create();
        }


        //---------------------------------------------------------------------------

        public void create()
        {
            AddTitle();
            model.Accept(this);
            AddHandlerToValiderBtn();
        }

        //---------------------------------------------------------------------------

        public void Visit(BlockClickR b)
        {
            newBlock = () => new BlockClickR();
        }


        public void Visit(BlockClickL b)
        {
            newBlock = () => new BlockClickL();
        }


        public void Visit(BlockCloseDesiredApplication b)
        {
            (Label, TextBox) appName = newTextBox("App Name", b.appName);
            newBlock = () => new BlockCloseDesiredApplication(appName.Item2.Text);
        }


        public void Visit(BlockCreateTextFile b)
        {
            (TextBox, Button) filePath = newSave("File Path", concatPathWithFileName(b.filePath, b.fileName) );
            (Label, TextBox) text = newTextBox("texte", b.text);
            newBlock = () => new BlockCreateTextFile(System.IO.Path.GetDirectoryName(filePath.Item1.Text), System.IO.Path.GetFileName(filePath.Item1.Text), text.Item2.Text);
        }


        public void Visit(BlockDeleteDirectory b)
        {
            (TextBox, Button) filePath = newFolderSelector("element à supprimer", b.path);
            newBlock = () => new BlockDeleteDirectory(filePath.Item1.Text);
        }


        public void Visit(BlockDownloadFile b)
        {
            (Label, TextBox)  address    = newTextBox("Web address", b.address);
            (TextBox, Button) folderPath = newFolderSelector("Folder Path", b.folderPath);
            newBlock = () => new BlockDownloadFile(address.Item2.Text, folderPath.Item1.Text);
        }


        public void Visit(BlockHibernate b)
        {
            newBlock = () => new BlockHibernate();
        }


        public void Visit(BlockInvokeAutomationId b)
        {
            (Label, TextBox) automationId = newTextBox("Automation Id", b.automationID);
            newBlock = () => new BlockInvokeAutomationId(automationId.Item2.Text);
        }


        public void Visit(BlockKeyBoard b)
        {
            (Label, TextBox) shortcut = newTextBox("Shortcut", b._shortCut);
            newBlock = () => new BlockKeyBoard(shortcut.Item2.Text);
        }


        //TODO: ajouter check nombre sur les TextBox des 3 navigateurs
        public void Visit(BlockLaunchBrowserChrome b)
        {
            (Label, TextBox) address  = newTextBox("adresse web", b.address);
            (Label, TextBox) delay    = newTextBox("Delay before applying window style (ms)", b.delay.ToString());
            ComboBox         comboBox = newComboBoxList( "Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockLaunchBrowserChrome(address.Item2.Text, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text));
        }


        public void Visit(BlockLaunchBrowserFirefox b)
        {
            (Label, TextBox) address  = newTextBox("Adresse web", b.address);
            (Label, TextBox) delay = newTextBox("Delay before applying window style (ms)", b.delay.ToString());
            ComboBox comboBox = newComboBoxList("Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockLaunchBrowserFirefox(address.Item2.Text, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text));
        }


        public void Visit(BlockLaunchBrowserEdge b)
        {
            (Label, TextBox) address  = newTextBox("Adresse web", b.address);
            (Label, TextBox) delay = newTextBox("Delay before applying window style (ms)", b.delay.ToString());
            ComboBox comboBox = newComboBoxList("Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockLaunchBrowserEdge(address.Item2.Text, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text));
        }


        public void Visit(BlockLock b)
        {
            newBlock = () => new BlockLock();
        }


        public void Visit(BlockMove b)
        {
            (TextBox, Button) source = newFolderSelector("Source directory", b.source);
            (TextBox, Button) destination = newFolderSelector("Destination directory", b.destination);
            newBlock = () => new BlockMove(source.Item1.Text, destination.Item1.Text);
        }


        public void Visit(BlockMessageBox b)
        {
            (Label, TextBox) param1 = newTextBox("param 1", b.param1);
            (Label, TextBox) param2 = newTextBox("param 2", b.param2);
            newBlock = () => new BlockMessageBox(param1.Item2.Text, param2.Item2.Text);
        }


        public void Visit(BlockRecognition b)
        {
            (TextBox, Button) templatePath = newFileSelector("Template path", b.templatePath);
            (Label, TextBox) xInterest = newTextBox("X en haut à gauche de la zone de recherche\n(0 pour tout l'écran)", b.xInterest.ToString());
            (Label, TextBox) yInterest = newTextBox("Y en haut à gauche de la zone de recherche\n(0 pour tout l'écran)", b.yInterest.ToString());
            (Label, TextBox) heightInterest = newTextBox("Hauteur de la zone de recherche\n(0 pour tout l'écran)", b.heightInterest.ToString());
            (Label, TextBox) widthInterest = newTextBox("Largeur de la zone de recherche\n(0 pour tout l'écran)", b.widthInterest.ToString());
            (Label, TextBox) screenNumber = newTextBox("Numero de l'ecran de recherche", b.screenNumber.ToString());
            (Label, TextBox) offSetX = newTextBox("Décalge horizontal de la souris", b.offSetX.ToString());
            (Label, TextBox) offSetY = newTextBox("Décalge vertical de la souris", b.offSetY.ToString());
            (Label, TextBox) scale = newTextBox("Scale de l'image\n[0,1[ rétrecir\t]1,+inf] agrandir", b.scale.ToString());
            ComboBox loop = newComboBoxBool("Essayer plusieurs scale", b.loop);
            ComboBox debugMode = newComboBoxBool("DebugMode", b.debugMode);
            newBlock = () => new BlockRecognition(templatePath.Item1.Text, xInterest: ((xInterest.Item2.Text == "*") ? 0 : int.Parse(xInterest.Item2.Text)), yInterest: int.Parse(yInterest.Item2.Text), heightInterest: int.Parse(heightInterest.Item2.Text), widthInterest: int.Parse(widthInterest.Item2.Text), screenNumber: int.Parse(screenNumber.Item2.Text), offSetX: int.Parse(offSetX.Item2.Text), offSetY: int.Parse(offSetY.Item2.Text), scale: int.Parse(scale.Item2.Text), loop: (bool)loop.SelectedItem, debugMode: (bool)debugMode.SelectedItem);
        }


        public void Visit(BlockRestart b)
        {
            newBlock = () => new BlockRestart();
        }


        public void Visit(BlockLaunchApp b)
        {
            (TextBox, Button) appPath   = newFileSelector("Chemin de l'app", b.appPath);
            (Label, TextBox)  arguments = newTextBox("arguments", b.arguments);
            (Label, TextBox) delay      = newTextBox("delay before applying window style (ms)", b.delay.ToString());
            ComboBox comboBox           = newComboBoxList("Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockLaunchApp(appPath.Item1.Text, arguments.Item2.Text, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text));
        }


        public void Visit(BlockRunScript b)
        {
            (Label, TextBox) script = newTextBox("script", b.script);
            newBlock = () => new BlockRunScript(script.Item2.Text);

        }

        public void Visit(BlockSendEmail b)
        {
            (Label, TextBox) subject = newTextBox("subject", b.subject);
            (Label, TextBox) to = newTextBox("to", b.to);
            (Label, TextBox) body = newTextBox("body", b.body);
            newBlock = () => new BlockSendEmail(body.Item2.Text, to.Item2.Text, subject.Item2.Text);
        }


        public void Visit(BlockSetCursor b)
        {
            (Label, TextBox) x = newTextBox("x", b.x.ToString());
            (Label, TextBox) y = newTextBox("y", b.y.ToString());
            newBlock = () => new BlockSetCursor(int.Parse(x.Item2.Text), int.Parse(y.Item2.Text));
        }


        public void Visit(BlockShutdown b)
        {
            newBlock = () => new BlockShutdown();
        }


        public void Visit(BlockWait b)
        {
            (Label, TextBox) mili = newTextBox("milisecondes", b.mili.ToString());
            (Label, TextBox) sec = newTextBox("secondes", b.sec.ToString());
            (Label, TextBox) min = newTextBox("minutes", b.min.ToString());
            (Label, TextBox) hour = newTextBox("heures", b.hour.ToString());
            newBlock = () => new BlockWait(int.Parse(mili.Item2.Text), int.Parse(sec.Item2.Text), int.Parse(min.Item2.Text), int.Parse(hour.Item2.Text));
        }


        public void Visit(BlockCopy b)
        {
            (TextBox, Button) src  = newFolderSelector("Source directory", b.source);
            (TextBox, Button) dest = newFolderSelector("Destination directory", b.destination);
            newBlock = () => new BlockCopy(src.Item1.Text, dest.Item1.Text);
        }


        public void Visit(BlockScreenshot b)
        {
            (TextBox, Button) filePath     = newSave("Path to save", concatPathWithFileName(b.folderPath, b.fileName) );
            ComboBox screenNumber = newComboBoxList("Screen selection", System.Windows.Forms.Screen.AllScreens , b.screenNumber, getDataTemplateForScreen());
            newBlock = () => new BlockScreenshot(System.IO.Path.GetDirectoryName(filePath.Item1.Text), System.IO.Path.GetFileName(filePath.Item1.Text), screenNumber.SelectedIndex);
        }


        //-----------------------------------------------------------------------------------

        public void AddTitle()
        {
            Label blockTitle = new Label();
            blockTitle.Content = "Block " + model.Name;
            blockTitle.FontSize = 18;
            blockTitle.HorizontalAlignment = HorizontalAlignment.Left;
            blockTitle.FontWeight = FontWeights.Bold;
            blockTitle.Margin = new Thickness(0,0,10,0);
            //blockTitle.FontStyle
            Controls.Children.Add(blockTitle);
        }


        public void AddHandlerToValiderBtn()
        {
            validerBtn.Click += (object sender, RoutedEventArgs e) => { res=newBlock(); this.DialogResult = true; /*MessageBox.Show($"{Controls.Width}");*/ };
        }


        //---------------------------------------------------------------------------

        private (Label, TextBox) newTextBox(string labelTxt, string defaultText = "")
        {
            Label label = newLabel(labelTxt); // pas de add() car fct 
            label.Margin = new Thickness(0, 10, 0, 0);
            TextBox textBox = new TextBox();
            textBox.Height = 20;
            textBox.Width = (98d / 100d) * Controls.Width;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.Text = defaultText;
            Controls.Children.Add(textBox);
            return (label, textBox);
        }


        private Button newButton(string content)
        {
            Button btn = new Button();
            btn.Content = content;
            btn.Height = 20;
            btn.Width = 50;
            Controls.Children.Add(btn);
            return btn;
        }


        private Label newLabel(string content)
        {
            Label label   = new Label();
            label.Margin  = new Thickness(0, 10, 0, 0);
            label.Content = content;
            Controls.Children.Add(label);
            return label;
        }


        private (Label, TextBox, Button) BaseBrowse(string labelTxt, string defaultPath, double ratio=73)
        {
            Label label = new Label();
            label.Margin = new Thickness(0, 10, 0, 0);
            label.Content = labelTxt;
            Controls.Children.Add(label);

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;

            TextBox textBox = new TextBox();
            textBox.Height = 20;
            textBox.Width = (ratio / 100d) * Controls.Width;
            textBox.Text = defaultPath;
            textBox.IsReadOnly = true;
            sp.Children.Add(textBox);

            Button btn = new Button();
            btn.Margin = new Thickness(5, 0, 0, 0);
            btn.Content = "Browse";
            btn.Height = 20;
            btn.Width = ((100d-ratio-3) / 100d) * Controls.Width;
            sp.Children.Add(btn);

            Controls.Children.Add(sp);

            return (label, textBox, btn);
        }


        private (TextBox, Button) newFolderSelector(string labelTxt, string defaultPath)
        {
            (Label l,TextBox tb, Button btn ) baseBrowse = BaseBrowse(labelTxt, defaultPath);
            baseBrowse.btn.Click += (object sender, RoutedEventArgs e) =>
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        baseBrowse.tb.Text = fbd.SelectedPath;
                    }
                }
            };
            baseBrowse.btn.Content = "select folder";
            return (baseBrowse.tb, baseBrowse.btn);
        }


        private (TextBox, Button) newFileSelector(string labelTxt, string defaultPath)
        {
            (Label l, TextBox tb, Button btn) baseBrowse = BaseBrowse(labelTxt, defaultPath);
            baseBrowse.btn.Click += (object sender, RoutedEventArgs e) =>
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "All Files  (*)|*|" +
                             "JPEG Files (*.jpeg)|*.jpeg|" +
                             "JPG Files  (*.jpg)|*.jpg|" +
                             "PNG Files  (*.png)|*.png|" +
                             "GIF Files  (*.gif)|*.gif";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    baseBrowse.tb.Text = dlg.FileName;
                }
            };
            baseBrowse.btn.Content = "select file";
            return (baseBrowse.tb, baseBrowse.btn);
        }


        private (TextBox, Button) newSave(string labelTxt, string defaultPath)
        {
            (Label l, TextBox tb, Button btn) baseBrowse = BaseBrowse(labelTxt, defaultPath);
            baseBrowse.btn.Click += (object sender, RoutedEventArgs e) =>
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

                saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    baseBrowse.tb.Text = saveFileDialog.FileName;
                }
            };
            baseBrowse.btn.Content = "save";
            return (baseBrowse.tb, baseBrowse.btn);
        }


        private ComboBox newComboBoxBool(string labelTxt, bool value)
        {
            Label label = newLabel(labelTxt);
            label.Margin = new Thickness(0, 10, 0, 0);
            ComboBox cb = new ComboBox();
            cb.ItemsSource = new bool[] { true, false };
            cb.SelectedIndex = (value) ? 0 : 1;
            cb.Width = (98d / 100d) * Controls.Width;
            Controls.Children.Add(cb);
            return cb;
        }


        private ComboBox newComboBoxList(string labelTxt, System.Collections.IEnumerable values, object SelectedItem)
        {
            Label label = newLabel(labelTxt);
            label.Margin = new Thickness(0, 10, 0, 0);
            ComboBox cb = new ComboBox();
            cb.ItemsSource = values;
            cb.SelectedItem = SelectedItem;
            cb.Width = (98d / 100d) * Controls.Width;
            Controls.Children.Add(cb);
            return cb;
        }

        private ComboBox newComboBoxList(string labelTxt, System.Collections.IEnumerable values, int SelectedIndex, DataTemplate template)
        {
            Label label = newLabel(labelTxt);
            label.Margin = new Thickness(0, 10, 0, 0);
            ComboBox cb = new ComboBox();
            cb.ItemsSource = values;
            cb.ItemTemplate = template;
            cb.SelectedIndex = SelectedIndex;
            cb.Width = (98d / 100d) * Controls.Width;
            Controls.Children.Add(cb);
            return cb;
        }


        //---------------------------------------------------------------------------

        private DataTemplate getDataTemplateForScreen()
        {
            DataTemplate template = new DataTemplate();
            template.DataType = typeof(System.Windows.Forms.Screen);
            FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
            spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            FrameworkElementFactory DeviceName = new FrameworkElementFactory(typeof(TextBlock));
            DeviceName.SetBinding(TextBlock.TextProperty, new Binding("DeviceName"));
            spFactory.AppendChild(DeviceName);
            FrameworkElementFactory Bounds = new FrameworkElementFactory(typeof(TextBlock));
            Bounds.SetValue(TextBlock.MarginProperty, new Thickness(15, 0, 0, 0));
            Bounds.SetBinding(TextBlock.TextProperty, new Binding("Bounds"));
            spFactory.AppendChild(Bounds);
            template.VisualTree = spFactory;
            return template;
        }


        //---------------------------------------------------------------------------







    }
}
