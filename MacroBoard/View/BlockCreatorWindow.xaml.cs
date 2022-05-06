using System;
using System.Collections.Generic;
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

namespace MacroBoard.View
{

    public partial class BlockCreatorWindow : Window
    {

        Func<Block> newBlock;
        Block[] res;
        Block   model;


 //---------------------------------------------------------------------------

        public BlockCreatorWindow(Block[] res, Block model)
        {
            InitializeComponent();
            this.res   = res;
            this.model = model;
            create();

        }


        //---------------------------------------------------------------------------

        private void create()
        {
            switch (model.BlockType)
            {
                case nameof(BlockCopy)                   : create(model as BlockCopy);                    break;
                case nameof(BlockScreenshot)             : create(model as BlockScreenshot);              break;
                case nameof(BlockClickL)                 : create(model as BlockClickL);                  break;
                case nameof(BlockClickR)                 : create(model as BlockClickR);                  break;
                case nameof(BlockCloseDesiredApplication): create(model as BlockCloseDesiredApplication); break;
                case nameof(BlockCreateTextFile)         : create(model as BlockCreateTextFile);          break;
                case nameof(BlockDeleteDirectory)        : create(model as BlockDeleteDirectory);         break;
                case nameof(BlockDownloadFile)           : create(model as BlockDownloadFile);            break;
                case nameof(BlockHibernate)              : create(model as BlockHibernate);               break;
                case nameof(BlockInvokeAutomationId)     : create(model as BlockInvokeAutomationId);      break;
                case nameof(BlockKeyBoard)               : create(model as BlockKeyBoard);                break;
                case nameof(BlockLaunchBrowserChrome)    : create(model as BlockLaunchBrowserChrome);     break;
                case nameof(BlockLaunchBrowserChromex86) : create(model as BlockLaunchBrowserChromex86);  break;
                case nameof(BlockLaunchBrowserFirefox)   : create(model as BlockLaunchBrowserFirefox);    break;
                case nameof(BlockLaunchEdgeBrowser)      : create(model as BlockLaunchEdgeBrowser);       break;
                case nameof(BlockLock)                   : create(model as BlockLock);                    break;
                case nameof(BlockMessageBoxBlock)        : create(model as BlockMessageBoxBlock);         break;
                case nameof(BlockMove)                   : create(model as BlockMove);                    break;
                case nameof(BlockRecognition)            : create(model as BlockRecognition);             break;
                case nameof(BlockRestart)                : create(model as BlockRestart);                 break;
                case nameof(BlockRunApp)                 : create(model as BlockRunApp);                  break;
                case nameof(BlockRunScript)              : create(model as BlockRunScript);               break;
                case nameof(BlockSendEmail)              : create(model as BlockSendEmail);               break;
                case nameof(BlockSetCursor)              : create(model as BlockSetCursor);               break;
                case nameof(BlockShutdown)               : create(model as BlockShutdown);                break;
                case nameof(BlockWait)                   : create(model as BlockWait);                    break;





                default: break;
            }
            AddHandlerToValiderBtn();
        }

//---------------------------------------------------------------------------

        private void create(BlockClickR? blockClickR)
        {
            throw new NotImplementedException();
        }

        private void create(BlockClickL? blockClickL)
        {
            throw new NotImplementedException();
        }

        private void create(BlockCloseDesiredApplication? blockCloseDesiredApplication)
        {
            (TextBox, Button) appName = newBrowse("App Name", ((BlockScreenshot)model).Name);
            newBlock = () => new BlockCloseDesiredApplication(appName.Item1.Text);
        }

        private void create(BlockCreateTextFile? blockCreateTextFile)
        {
            (TextBox, Button) filePath = newBrowse("File Path", blockCreateTextFile._filePath); 
            (Label, TextBox) fileName = newTextBox("File Name", blockCreateTextFile._fileName);
            (Label, TextBox) fileType = newTextBox("File Type", blockCreateTextFile._fileType);
            (Label, TextBox) text = newTextBox("Text", blockCreateTextFile._text);
            newBlock = () => new BlockCreateTextFile(filePath.Item1.Text, fileName.Item2.Text, fileType.Item2.Text, text.Item2.Text);
        }

        private void create(BlockDeleteDirectory? blockDeleteDirectory)
        {
            (TextBox, Button) filePath = newBrowse("File Path", blockDeleteDirectory.path);
            newBlock = () => new BlockDeleteDirectory(filePath.Item1.Text);
        }

        //TODO

        private void create(BlockDownloadFile? blockDownloadFile)
        {
            (TextBox, Button) fileName = newBrowse("File Path", blockDownloadFile.fileName);
            (TextBox, Button) address = newBrowse("File Path", blockDownloadFile.address);
            newBlock = () => new BlockDownloadFile(address.Item1.Text,fileName.Item1.Text);
        }

        private void create(BlockHibernate? blockHibernate)
        {
            throw new NotImplementedException();
        }

        private void create(BlockInvokeAutomationId? blockInvokeAutomationId)
        {
            (Label, TextBox) automationId = newTextBox("Automation Id", blockInvokeAutomationId.automationID);
            newBlock = () => new BlockInvokeAutomationId(automationId.Item2.Text);  
        }

        private void create(BlockKeyBoard? blockKeyBoard)
        {
            (TextBox, Button) shortcut = newBrowse("Shortcut", blockKeyBoard._shortCut);
            newBlock = () => new BlockKeyBoard(shortcut.Item1.Text);
        }

        private void create(BlockLaunchBrowserChrome? blockLaunchBrowserChrome)
        {
            (TextBox, Button) address = newBrowse("Chrome address", blockLaunchBrowserChrome.address);
            newBlock = () => new BlockKeyBoard(address.Item1.Text);

        }

        private void create(BlockLaunchBrowserChromex86? blockLaunchBrowserChromex86)
        {
            (TextBox, Button) address = newBrowse("Chrome86 address", blockLaunchBrowserChromex86.address);
            newBlock = () => new BlockKeyBoard(address.Item1.Text);
        }

        private void create(BlockLaunchBrowserFirefox? blockLaunchBrowserFirefox)
        {
            (TextBox, Button) address = newBrowse("FireFox address", blockLaunchBrowserFirefox.address);
            newBlock = () => new BlockKeyBoard(address.Item1.Text);
        }

        private void create(BlockLaunchEdgeBrowser? blockLaunchEdgeBrowser)
        {
            (TextBox, Button) address = newBrowse("Edge address", blockLaunchEdgeBrowser.address);
            newBlock = () => new BlockKeyBoard(address.Item1.Text);
        }

        private void create(BlockLock? blockLock)
        {
            throw new NotImplementedException();
        }

        private void create(BlockMove? blockMove)
        {
            (Label, TextBox) src = newTextBox("Automation Id", blockMove.source);
            (Label, TextBox) dest = newTextBox("Automation Id", blockMove.destination);
            newBlock = () => new BlockMessageBoxBlock(src.Item2.Text, dest.Item2.Text);
        }

        private void create(BlockMessageBoxBlock? blockMessageBoxBlock)
        {
            (Label, TextBox) param1 = newTextBox("Automation Id", blockMessageBoxBlock.param1);
            (Label, TextBox) param2 = newTextBox("Automation Id", blockMessageBoxBlock.param2);
            newBlock = () => new BlockMessageBoxBlock(param1.Item2.Text, param2.Item2.Text);
       }
        private void create(BlockRecognition? blockRecognition)
        {
            (TextBox, Button) templatePath = newBrowse("FireFox address", blockRecognition.templatePath);
            newBlock = () => new BlockRecognition(templatePath.Item1.Text);

        }

        private void create(BlockRestart? blockRestart)
        {
        }

        private void create(BlockRunApp? blockRunApp)
        {
            (TextBox, Button) url = newBrowse("chemin de l'app", blockRunApp.url);
            newBlock = () => new BlockRunApp(url.Item1.Text);
        }

        private void create(BlockRunScript? blockRunScript)
        {
            (Label, TextBox) script = newTextBox("script", blockRunScript.script);
            newBlock = () => new BlockRunScript(script.Item2.Text);

        }

        private void create(BlockSendEmail? blockSendEmail)
        {
            (Label, TextBox) body    = newTextBox("body", blockSendEmail.body);
            (Label, TextBox) to      = newTextBox("to", blockSendEmail.to);
            (Label, TextBox) subject = newTextBox("subject", blockSendEmail.subject);
            newBlock = () => new BlockSendEmail(body.Item2.Text, to.Item2.Text, subject.Item2.Text);
        }


        private void create(BlockSetCursor? blockSetCursor)
        {
            (Label, TextBox) x = newTextBox("x", blockSetCursor.x.ToString());
            (Label, TextBox) y = newTextBox("y", blockSetCursor.y.ToString());
            newBlock = () => new BlockSetCursor(int.Parse(x.Item2.Text), int.Parse(y.Item2.Text) );
        }


        private void create(BlockShutdown? blockShutdown)
        {
        }

        private void create(BlockWait? blockWait)
        {
            (Label, TextBox) mili = newTextBox("milisecondes", blockWait.mili.ToString());
            (Label, TextBox) sec  = newTextBox("secondes", blockWait.sec.ToString());
            (Label, TextBox) min  = newTextBox("minutes", blockWait.min.ToString());
            (Label, TextBox) hour = newTextBox("heures", blockWait.hour.ToString());
            newBlock = () => new BlockWait(int.Parse(mili.Item2.Text), int.Parse(sec.Item2.Text), int.Parse(min.Item2.Text), int.Parse(hour.Item2.Text)) ;
        }



        private void create(BlockCopy? b)
        {
            Label label = newLabel("block copy");
            (TextBox, Button) src = newBrowse("source", ((BlockCopy)model).source);
            (TextBox, Button) dest = newBrowse("destination", ((BlockCopy)model).destination);
            newBlock = () => new BlockCopy(src.Item1.Text, dest.Item1.Text);
        }

        private void create(BlockScreenshot? b)
        {
            (TextBox, Button) filePath = newBrowse("path to save", ((BlockScreenshot)model).fileName);
            (Label, TextBox) screenNumber = newTextBox("screenNumber", ((BlockScreenshot)model).screenNumber.ToString());
            //newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0");
            newBlock = () => new BlockScreenshot(filePath.Item1.Text, Int32.Parse(screenNumber.Item2.Text));
        }



//-----------------------------------------------------------------------------------

        private void AddHandlerToValiderBtn()
        {
            validerBtn.Click += (object sender, RoutedEventArgs e) => {  res[0]=newBlock(); this.DialogResult = true; /*MessageBox.Show($"{Controls.Width}");*/ };
        }


        private (Label,TextBox) newTextBox(string labelTxt, string defaultText = "")
        {
            Label label = newLabel(labelTxt); // pas de add() car fct 
            TextBox textBox = new TextBox();
            textBox.Height = 20;
            textBox.Width = (98d/100d)*Controls.Width;
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
            Label label = new Label();
            label.Content = content;
            Controls.Children.Add(label);
            return label;
        }

        private (TextBox, Button) newBrowse(string labelTxt, string defaultPath)
        {
            (Label label, TextBox txtBox) = newTextBox(labelTxt, defaultPath);
            Button  browse   = newButton("browse"); //pas de Controls.Items.Add() car fct le fait deja
            browse.Click += (object sender, RoutedEventArgs e) =>
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.DefaultExt = ".jpeg";
                dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|" +
                             "JPG Files  (*.jpg)|*.jpg|"   +
                             "All Files  (*)|*|" +
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

        private void validerBtn_Click(object sender, RoutedEventArgs e)
        {

        }


        //---------------------------------------------------------------------------







    }
}
