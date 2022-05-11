using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MacroBoard.View
{

    public partial class BlockCreatorWindow : Window
    {

        Func<Block> newBlock;   // lambda-expression appelée lors du click "valider", qui défini comment créer un block
        Block[] res;        // tableau de taille=1 pour renvoyer le nouveau Block
        Block model;

        //---------------------------------------------------------------------------

        public BlockCreatorWindow(Block[] res, Block model)
        {
            InitializeComponent();
            this.res = res;
            this.model = model;
            this.newBlock = () => null;
            create();

        }


        //---------------------------------------------------------------------------

        private void create()
        {
            switch (model.BlockType)
            {
                case nameof(BlockCopy): create(model as BlockCopy); break;
                case nameof(BlockScreenshot): create(model as BlockScreenshot); break;
                case nameof(BlockClickL): create(model as BlockClickL); break;
                case nameof(BlockClickR): create(model as BlockClickR); break;
                case nameof(BlockCloseDesiredApplication): create(model as BlockCloseDesiredApplication); break;
                case nameof(BlockCreateTextFile): create(model as BlockCreateTextFile); break;
                case nameof(BlockDeleteDirectory): create(model as BlockDeleteDirectory); break;
                case nameof(BlockDownloadFile): create(model as BlockDownloadFile); break;
                case nameof(BlockHibernate): create(model as BlockHibernate); break;
                case nameof(BlockInvokeAutomationId): create(model as BlockInvokeAutomationId); break;
                case nameof(BlockKeyBoard): create(model as BlockKeyBoard); break;
                case nameof(BlockLaunchBrowserChrome): create(model as BlockLaunchBrowserChrome); break;
                case nameof(BlockLaunchBrowserChromex86): create(model as BlockLaunchBrowserChromex86); break;
                case nameof(BlockLaunchBrowserFirefox): create(model as BlockLaunchBrowserFirefox); break;
                case nameof(BlockLaunchEdgeBrowser): create(model as BlockLaunchEdgeBrowser); break;
                case nameof(BlockLock): create(model as BlockLock); break;
                case nameof(BlockMessageBoxBlock): create(model as BlockMessageBoxBlock); break;
                case nameof(BlockMove): create(model as BlockMove); break;
                case nameof(BlockRecognition): create(model as BlockRecognition); break;
                case nameof(BlockRestart): create(model as BlockRestart); break;
                case nameof(BlockRunApp): create(model as BlockRunApp); break;
                case nameof(BlockRunScript): create(model as BlockRunScript); break;
                case nameof(BlockSendEmail): create(model as BlockSendEmail); break;
                case nameof(BlockSetCursor): create(model as BlockSetCursor); break;
                case nameof(BlockShutdown): create(model as BlockShutdown); break;
                case nameof(BlockWait): create(model as BlockWait); break;
                default: MessageBox.Show("block pas impléménté"); break;
            }
            AddHandlerToValiderBtn();
        }

        //---------------------------------------------------------------------------

        private void create(BlockClickR? blockClickR)
        {
            newBlock = () => new BlockClickR();
        }


        private void create(BlockClickL? blockClickL)
        {
            newBlock = () => new BlockClickL();
        }


        private void create(BlockCloseDesiredApplication? blockCloseDesiredApplication)
        {
            (Label, TextBox) appName = newTextBox("App Name", blockCloseDesiredApplication.appName,null);
            newBlock = () => new BlockCloseDesiredApplication(appName.Item2.Text);
        }


        private void create(BlockCreateTextFile? blockCreateTextFile)
        {
            (TextBox, Button) filePath = newBrowseFolder("File Path", blockCreateTextFile._filePath);
            (Label, TextBox) fileName = newTextBox("File Name", blockCreateTextFile._fileName,null);
            (Label, TextBox) fileType = newTextBox("extension", blockCreateTextFile._fileType,null);
            (Label, TextBox) text = newTextBox("texte", blockCreateTextFile._text,null);
            newBlock = () => new BlockCreateTextFile(filePath.Item1.Text, fileName.Item2.Text, fileType.Item2.Text, text.Item2.Text);
        }


        private void create(BlockDeleteDirectory? blockDeleteDirectory)
        {
            (TextBox, Button) filePath = newBrowseFolder("element à supprimer", blockDeleteDirectory.path);
            newBlock = () => new BlockDeleteDirectory(filePath.Item1.Text);
        }


        private void create(BlockDownloadFile? blockDownloadFile)
        {
            (Label, TextBox) fileName = newTextBox("File name", blockDownloadFile.fileName,null);
            (TextBox, Button) address = newBrowse("File Path", blockDownloadFile.address);
            newBlock = () => new BlockDownloadFile(address.Item1.Text, fileName.Item2.Text);
        }


        private void create(BlockHibernate? blockHibernate)
        {
            newBlock = () => new BlockHibernate();
        }


        private void create(BlockInvokeAutomationId? blockInvokeAutomationId)
        {
            (Label, TextBox) automationId = newTextBox("Automation Id", blockInvokeAutomationId.automationID,null);
            newBlock = () => new BlockInvokeAutomationId(automationId.Item2.Text);
        }


        private void create(BlockKeyBoard? blockKeyBoard)
        {
            (Label, TextBox) shortcut = newTextBox("shortcut", blockKeyBoard._shortCut,null);
            newBlock = () => new BlockKeyBoard(shortcut.Item2.Text);
        }


        private void create(BlockLaunchBrowserChrome? blockLaunchBrowserChrome)
        {
            (Label, TextBox) address = newTextBox("adresse web", blockLaunchBrowserChrome.address,null);
            newBlock = () => new BlockLaunchBrowserChrome(address.Item2.Text);
        }


        private void create(BlockLaunchBrowserChromex86? blockLaunchBrowserChromex86)
        {
            (Label, TextBox) address = newTextBox("adresse web", blockLaunchBrowserChromex86.address,null);
            newBlock = () => new BlockLaunchBrowserChromex86(address.Item2.Text);
        }


        private void create(BlockLaunchBrowserFirefox? blockLaunchBrowserFirefox)
        {
            (Label, TextBox) address = newTextBox("adresse web", blockLaunchBrowserFirefox.address,null);
            newBlock = () => new BlockLaunchBrowserFirefox(address.Item2.Text);
        }


        private void create(BlockLaunchEdgeBrowser? blockLaunchEdgeBrowser)
        {
            (Label, TextBox) address = newTextBox("adresse web", blockLaunchEdgeBrowser.address,null);
            newBlock = () => new BlockLaunchEdgeBrowser(address.Item2.Text);
        }


        private void create(BlockLock? blockLock)
        {
            newBlock = () => new BlockLock();
        }


        private void create(BlockMove? blockMove)
        {
            (TextBox, Button) source = newBrowseFolder("source", blockMove.source);
            (TextBox, Button) destination = newBrowseFolder("source", blockMove.destination);
            newBlock = () => new BlockMove(source.Item1.Text, destination.Item1.Text);
        }


        private void create(BlockMessageBoxBlock? blockMessageBoxBlock)
        {
            (Label, TextBox) param1 = newTextBox("param 1", blockMessageBoxBlock.param1,null);
            (Label, TextBox) param2 = newTextBox("param 2", blockMessageBoxBlock.param2,null);
            newBlock = () => new BlockMessageBoxBlock(param1.Item2.Text, param2.Item2.Text);
        }


        private void create(BlockRecognition? blockRecognition)
        {
            (TextBox, Button) templatePath = newBrowse("Template path", blockRecognition.templatePath);
            (Label, TextBox) xInterest = newTextBox("X en haut à gauche de la zone de recherche\n(0 pour tout l'écran)", blockRecognition.xInterest.ToString(), CheckDigits);
            (Label, TextBox) yInterest = newTextBox("Y en haut à gauche de la zone de recherche\n(0 pour tout l'écran)", blockRecognition.yInterest.ToString(), CheckDigits);
            (Label, TextBox) heightInterest = newTextBox("Hauteur de la zone de recherche\n(0 pour tout l'écran)", blockRecognition.heightInterest.ToString(), CheckDigits);
            (Label, TextBox) widthInterest = newTextBox("Largeur de la zone de recherche\n(0 pour tout l'écran)", blockRecognition.widthInterest.ToString(), CheckDigits);
            (Label, TextBox) screenNumber = newTextBox("Numero de l'ecran de recherche", blockRecognition.screenNumber.ToString(), CheckDigits);
            (Label, TextBox) offSetX = newTextBox("Décalge horizontal de la souris", blockRecognition.offSetX.ToString(), CheckDigits);
            (Label, TextBox) offSetY = newTextBox("Décalge vertical de la souris", blockRecognition.offSetY.ToString(), CheckDigits);
            (Label, TextBox) scale = newTextBox("Scale de l'image\n[0,1[ rétrecir\t]1,+inf] agrandir", blockRecognition.scale.ToString(), CheckDigits);
            ComboBox loop = newComboBoxBool("Essayer plusieurs scale", blockRecognition.loop);
            ComboBox debugMode = newComboBoxBool("DebugMode", blockRecognition.debugMode);
            newBlock = () => new BlockRecognition(templatePath.Item1.Text, xInterest: ((xInterest.Item2.Text == "*") ? 0 : int.Parse(xInterest.Item2.Text)), yInterest: int.Parse(yInterest.Item2.Text), heightInterest: int.Parse(heightInterest.Item2.Text), widthInterest: int.Parse(widthInterest.Item2.Text), screenNumber: int.Parse(screenNumber.Item2.Text), offSetX: int.Parse(offSetX.Item2.Text), offSetY: int.Parse(offSetY.Item2.Text), scale: int.Parse(scale.Item2.Text), loop: (bool)loop.SelectedItem, debugMode: (bool)debugMode.SelectedItem);
        }


        private void create(BlockRestart? blockRestart)
        {
            newBlock = () => new BlockRestart();
        }


        private void create(BlockRunApp? blockRunApp)
        {
            (TextBox, Button) url = newBrowse("chemin de l'app", blockRunApp.url);
            newBlock = () => new BlockRunApp(url.Item1.Text);
        }


        private void create(BlockRunScript? blockRunScript)
        {
            (Label, TextBox) script = newTextBox("script", blockRunScript.script,null);
            newBlock = () => new BlockRunScript(script.Item2.Text);

        }


        private void create(BlockSendEmail? blockSendEmail)
        {
            (Label, TextBox) body = newTextBox("body", blockSendEmail.body, null);
            (Label, TextBox) to = newTextBox("to", blockSendEmail.to,null);
            (Label, TextBox) subject = newTextBox("subject", blockSendEmail.subject, null);
            newBlock = () => new BlockSendEmail(body.Item2.Text, to.Item2.Text, subject.Item2.Text);
        }


        private void create(BlockSetCursor? blockSetCursor)
        {
            (Label, TextBox) x = newTextBox("x", blockSetCursor.x.ToString(), CheckDigits);
            (Label, TextBox) y = newTextBox("y", blockSetCursor.y.ToString(), CheckDigits);
            newBlock = () => new BlockSetCursor(int.Parse(x.Item2.Text), int.Parse(y.Item2.Text));
        }


        private void create(BlockShutdown? blockShutdown)
        {
            newBlock = () => new BlockShutdown();
        }


        private void create(BlockWait? blockWait)
        {
            (Label, TextBox) mili = newTextBox("milisecondes", blockWait.mili.ToString(),CheckDigits);
            (Label, TextBox) sec = newTextBox("secondes", blockWait.sec.ToString(), CheckDigits);
            (Label, TextBox) min = newTextBox("minutes", blockWait.min.ToString(), CheckDigits);
            (Label, TextBox) hour = newTextBox("heures", blockWait.hour.ToString(), CheckDigits);
            newBlock = () => new BlockWait(int.Parse(mili.Item2.Text), int.Parse(sec.Item2.Text), int.Parse(min.Item2.Text), int.Parse(hour.Item2.Text));
        }


        private void create(BlockCopy? b)
        {
            Label label = newLabel("block copy");
            (TextBox, Button) src = newBrowseFolder("source", ((BlockCopy)model).source);
            (TextBox, Button) dest = newBrowseFolder("destination", ((BlockCopy)model).destination);
            newBlock = () => new BlockCopy(src.Item1.Text, dest.Item1.Text);
        }


        private void create(BlockScreenshot? b)
        {
            (TextBox, Button) filePath = newBrowseFolder("path to save", ((BlockScreenshot)model).fileName);
            (Label, TextBox) screenNumber = newTextBox("screenNumber", ((BlockScreenshot)model).screenNumber.ToString(),CheckDigits);

            newBlock = () => {
                return new BlockScreenshot(filePath.Item1.Text, Int32.Parse(screenNumber.Item2.Text));
            };


        }

        


        //-----------------------------------------------------------------------------------

        private void AddHandlerToValiderBtn()
        {
           
            validerBtn.Click += (object sender, RoutedEventArgs e) => {
                
                foreach(Control c in Controls.Children)
                    if (c is TextBox)
                        if (((TextBox)c).Text.Length <= 0)
                        {
                            MessageBox.Show("Remplissez tous les champs");
                            return;
                        }      

                res[0] = newBlock();
                this.DialogResult = true; /*MessageBox.Show($"{Controls.Width}");*/
            };
        }
        //---------------------------------------------------------------------------


        private (Label, TextBox) newTextBox(string labelTxt, string defaultText, TextCompositionEventHandler checkFormat)
        {
            Label label = newLabel(labelTxt); // pas de add() car fct 
            label.Margin = new Thickness(0, 10, 0, 0);
            TextBox textBox = new TextBox();
            textBox.Height = 20;
            textBox.Width = (98d / 100d) * Controls.Width;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.Text = defaultText;
            if (checkFormat!= null)
                textBox.PreviewTextInput += checkFormat;
            Controls.Children.Add(textBox);
            return (label, textBox);

           
        }

        private void CheckDigits(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

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
            Label label = new Label();
            label.Margin = new Thickness(0, 10, 0, 0);
            label.Content = content;
            Controls.Children.Add(label);
            return label;
        }


        private (TextBox, Button) newBrowseFolder(string labelTxt, string defaultPath)
        {
            (Label label, TextBox txtBox) = newTextBox(labelTxt, defaultPath,null);
            label.Margin = new Thickness(0, 10, 0, 0);
            Button browse = newButton("browse");
            browse.Margin = new Thickness(0, 5, 0, 0);
            browse.Click += (object sender, RoutedEventArgs e) =>
            {

                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();


                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string sSelectedPath = fbd.SelectedPath;
                    txtBox.Text = fbd.SelectedPath;
                }

            };
            return (txtBox, browse);



        }



        private (TextBox, Button) newBrowse(string labelTxt, string defaultPath)
        {
            (Label label, TextBox txtBox) = newTextBox(labelTxt, defaultPath,null);
            label.Margin = new Thickness(0, 10, 0, 0);
            Button browse = newButton("browse"); //pas de Controls.Items.Add() car fct le fait deja
            browse.Margin = new Thickness(0, 5, 0, 0);
            browse.Click += (object sender, RoutedEventArgs e) =>
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.Filter = "All Files  (*)|*|" +
                             "EXE        (*.exe)|*.exe|"+   
                             "JPEG Files (*.jpeg)|*.jpeg|" +
                             "JPG Files  (*.jpg)|*.jpg|" +
                             "PNG Files  (*.png)|*.png|" +
                             "GIF Files  (*.gif)|*.gif";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    txtBox.Text = dlg.FileName;
                }
            };
            return (txtBox, browse);
        }


        private ComboBox newComboBoxBool(string labelTxt, bool value)
        {
            Label label = newLabel(labelTxt);
            label.Margin = new Thickness(0, 10, 0, 0);
            ComboBox cb = new ComboBox();
            cb.ItemsSource = new bool[] { true, false };
            cb.SelectedIndex = (value) ? 0 : 1;
            //cb.Height = 20;
            cb.Width = (98d / 100d) * Controls.Width;
            Controls.Children.Add(cb);
            return cb;
        }


        //---------------------------------------------------------------------------











    }
}
