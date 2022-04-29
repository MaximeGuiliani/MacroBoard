using System;
using System.Windows;
using System.Windows.Controls;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for EditionWindow.xaml
    /// </summary>
    public partial class EditionWindow : Window
    {


        // TODO : Prendra en argument une Macro (List de block) Nom
        public EditionWindow()
        {
            InitializeComponent();
        }

        private void save(object sender, RoutedEventArgs e)
        {
            // TODO add Img attribute, check existing Names
            Macro macro = new Macro();
            macro.Name = Name_Box.Text;
            //macro.img = Image_Selected.Text;

        }



        private void selectImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png) |JPEG Files (*.jpeg)|*.jpeg|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                Image_Selected.Text = filename;
                //Macro_Image.Source = Uri.EscapeDataString(filename);


            }

        }

        private void Name_Box_GotFocus(object sender, RoutedEventArgs e)
        {
            Name_Box.Text = "";
        }

        private void Name_Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
