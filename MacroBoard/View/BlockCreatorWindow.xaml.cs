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
                case nameof(BlockCopy):
                    Label label            = newLabel("block copy");
                    (TextBox, Button) src  = newBrowse("source", @"C:\");
                    (TextBox, Button) dest = newBrowse("destination", @"C:\");
                    newBlock               = ()=>new BlockCopy(src.Item1.Text, dest.Item1.Text);
                    break;

                case nameof(BlockScreenshot):
                    (TextBox, Button) filePath     = newBrowse("path to save", @"C:\");
                    (Label, TextBox)  screenNumber = newTextBox("screenNumber", "0");
                    //newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0"); newBrowse("path to save", @"C:\");newTextBox("screenNumber", "0");
                    newBlock = () => new BlockScreenshot(filePath.Item1.Text, Int32.Parse(screenNumber.Item2.Text));
                    break;
                
                default:
                    break;
            }

            AddHandlerToValiderBtn();
        }

//---------------------------------------------------------------------------

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
                             "All Files  (.jpeg .jpg .png .gif)|*.jpeg;*.jpg;*.png;*.gif|" +
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
