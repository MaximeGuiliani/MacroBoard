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
