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
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class BlockCreator : Window
    {

        //public List<BlockView> l;
        //Block b;
        //Button valider;
        //public BlockCreator(List<BlockView> l, Block b)
        //{
        //    InitializeComponent();
        //    this.l = l;
        //    this.b = b;
        //    init();
        //}

        //private void init()
        //{
        //    switch (b.GetType().Name)
        //    {
        //        case "Copy":
        //            Label label = new Label();
        //            label.Content = "block copy";
        //            TextBox src = newTextBox();
        //            TextBox dest = newTextBox();
        //            Button btnSrc = newButton("OK");
        //            Button btnDest = newButton("OK");
        //            valider = newButton("valider");
        //            valider.Click += (object sender, RoutedEventArgs e) =>
        //            {
        //                MessageBox.Show("hey");
        //                Block b = new BlockCopy(src.Text, dest.Text); ;
        //                l.Add(new BlockView(b));
        //            };
        //            Controls.Items.Add(label);
        //            Controls.Items.Add(src);
        //            Controls.Items.Add(btnSrc);
        //            Controls.Items.Add(dest);
        //            Controls.Items.Add(btnDest);
        //            Controls.Items.Add(valider);
        //            break;

        //        case "Capture":
        //            TextBox FileName = newTextBox();
        //            Button btnF = newButton("OK");
        //            TextBox screenNumber = newTextBox();
        //            valider = newButton("Valider");
        //            valider.Click += (object sender, RoutedEventArgs e) =>
        //            {
        //                Block b = new Capture(FileName.Text, Int32.Parse(screenNumber.Text)); ;
        //                MessageBox.Show("hey");
        //                l.Add(new BlockView(b));
        //            };
        //            Controls.Items.Add(FileName);
        //            Controls.Items.Add(btnF);
        //            Controls.Items.Add(screenNumber);
        //            Controls.Items.Add(valider);
        //            break;

        //    }
        //}

        ////private void clickValider(Block b)
        ////{
        ////    MessageBox.Show("hey");
        ////    l.Add(new BlockView(b));
        ////}


        //private TextBox newTextBox()
        //{
        //    TextBox textBox = new TextBox();
        //    textBox.Height = 20;
        //    textBox.Width = 700;
        //    return textBox;
        //}

        //private Button newButton(string content)
        //{
        //    Button btn = new Button();
        //    btn.Content = content;
        //    btn.Height = 20;
        //    btn.Width = 50;
        //    return btn;
        //}





    }
}
