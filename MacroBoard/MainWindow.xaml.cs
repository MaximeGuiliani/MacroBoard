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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Macro> macros = new List<Macro>();
        List<Macro> Favmacros = new List<Macro>();
        bool isEdition = false;

        Macro macro = new Macro { Name = "Test0", BackGround = "Blue", Place = 0, isFav = true };
        Macro macro1 = new Macro { Name = "Test1", BackGround = "Red", Place = 1, isFav = false };
        Macro macro2 = new Macro { Name = "Test2", BackGround = "Yellow", Place = 2, isFav = true };
        Macro macro3 = new Macro { Name = "Test3", BackGround = "White", Place = 3, isFav = false };
        Macro macro4 = new Macro { Name = "Test4", BackGround = "Pink", Place = 4, isFav = true };



        public MainWindow()
        {
            InitializeComponent();
            macros.Add(macro);
            macros.Add(macro1);
            macros.Add(macro2);
            macros.Add(macro3);
            macros.Add(macro4);
            foreach (Macro m in macros)
            {
                if (m.isFav)
                {
                    Favmacros.Add(m);
                }
            }
            ListFav.ItemsSource = Favmacros;
            ListMacro.ItemsSource = macros;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = Search.Text;
            List<Macro> macrosSearch = new List<Macro>();
            if (txt != "")
            {
                foreach (Macro m in macros)
                {
                    if (m.Name.Contains(txt))
                        macrosSearch.Add(m);
                }
            }
            else
            {
                macrosSearch = macros;
            }
            ListMacro.ItemsSource = macrosSearch;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (isEdition)
            {
                EditionWindow editionWindow = new();
                editionWindow.Show();
            }
            else
            {
                MessageBox.Show("Do Job");
            }

        }



        private void EditionMode(object sender, RoutedEventArgs e)
        {
            if (isEdition)
            {
                ButtonEdit.Background = Brushes.Green;
                isEdition = false;

            }
            else
            {
                ButtonEdit.Background = Brushes.Red;
                isEdition = true;
            }
        }
    }
}




public class Macro
{

    private string nameValue;

    public string Name
    {
        get { return nameValue; }
        set { nameValue = value; }
    }

    private string BackGroundValue;

    public string BackGround
    {
        get { return BackGroundValue; }

        set
        {
            if (value != BackGroundValue)
            {
                BackGroundValue = value;
            }
        }
    }

    private int placeValue;

    public int Place
    {
        get { return placeValue; }
        set { placeValue = value; }
    }


    private bool FavValue;

    public bool isFav
    {
        get { return FavValue; }
        set { FavValue = value; }
    }







}
