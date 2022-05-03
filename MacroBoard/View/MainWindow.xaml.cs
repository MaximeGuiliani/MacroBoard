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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<WorkFlow> macros = new();
        List<WorkFlow> Favmacros = new();
        bool isEdition = false;
        bool isRemove = false;
        bool isAddFav = false;
        WorkFlow macro0 = new WorkFlow("Blue", "Test0", new List<Block>());
        WorkFlow macro1 = new WorkFlow("Blue", "Test1", new List<Block>());
        WorkFlow macro2 = new WorkFlow("Blue", "Test2", new List<Block>());
        WorkFlow macro3 = new WorkFlow("Blue", "Test3", new List<Block>());
        WorkFlow macro4 = new WorkFlow("Blue", "Test4", new List<Block>());
        WorkFlow macro5 = new WorkFlow("Blue", "Test5", new List<Block>());


        public MainWindow()
        {
            InitializeComponent();
            macros.Add(macro0);
            macros.Add(macro1);
            macros.Add(macro2);
            macros.Add(macro3);
            macros.Add(macro4);
            macros.Add(macro5);
            Favmacros.Add(macro0);
            Favmacros.Add(macro4);
            ListFav.ItemsSource = Favmacros;
            ListMacro.ItemsSource = macros;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = Search.Text;
            List<WorkFlow> macrosSearch = new List<WorkFlow>();
            if (txt != "")
            {
                foreach (WorkFlow m in macros)
                {
                    if (m.workflowName.Contains(txt))
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
            string buttonName = (sender as Button).Content.ToString();
            if (buttonName != null)
            {
                if (isEdition)
                {
                    EditionWindow editionWindow = new();
                    editionWindow.Show();
                }
                else if (isAddFav)
                {
                    AddFav(buttonName);
                }

                else if (isRemove)
                {
                    RemoveMacro(buttonName);
                }
                else
                {
                    MessageBox.Show("Do Job");
                }
            }
            UpdateAllMacro();
            UpdateFav();


        }


        private void Button_Fav_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Content.ToString();

            if (buttonName != null)
            {
                if (isEdition)
                {
                    EditionWindow editionWindow = new();
                    editionWindow.Show();
                }
                else if (isAddFav)
                {
                    AddFav(buttonName);
                }
                else if (isRemove)
                {
                    RemoveFav(buttonName);
                }
                else
                {
                    MessageBox.Show("Do Job");
                }
            }
            UpdateAllMacro();
            UpdateFav();
        }


        private void AddFav(string buttonName)
        {
            if (Favmacros.Count < 5)
            {
                foreach (WorkFlow macro in macros)
                {
                    if (macro.workflowName.Equals(buttonName))
                    {
                        if (!IsInList(Favmacros, buttonName))
                        {
                            Favmacros.Add(macro);
                        }

                    }
                }


            }


        }



        private bool IsInList(List<WorkFlow> lm, string content)
        {
            bool result = false;
            foreach (WorkFlow m in lm)
            {
                if (m.workflowName.Equals(content))
                {
                    result = true;
                }
            }
            return result;
        }


        private void RemoveFav(string content)
        {

            foreach (WorkFlow favmacro in Favmacros)
            {
                if (favmacro.workflowName.Equals(content))
                {
                    Favmacros.Remove(favmacro);
                    break;
                }
            }

        }
        private void RemoveMacro(string content)
        {
            WorkFlow toremove = null;
            foreach (WorkFlow macro in macros)
            {
                if (macro.workflowName.Equals(content))
                {
                    toremove = macro;
                    break;
                }
            }
            if (toremove != null)
            {
                Favmacros.Remove(toremove);
                macros.Remove(toremove);
            }
        }






        private void EditionMode(object sender, RoutedEventArgs e)
        {
            if (isEdition)
            {
                ButtonEdit.Background = Brushes.Green;
                ButtonRemove.Visibility = Visibility.Visible;
                ButtonAddFav.Visibility = Visibility.Visible;
                isEdition = false;
            }
            else
            {
                ButtonEdit.Background = Brushes.Red;
                ButtonRemove.Visibility = Visibility.Hidden;
                ButtonAddFav.Visibility = Visibility.Hidden;
                isEdition = true;
            }
        }

        private void AddMode(object sender, RoutedEventArgs e)
        {
            if (isAddFav)
            {
                ButtonAddFav.Background = Brushes.Green;
                ButtonEdit.Visibility = Visibility.Visible;
                ButtonRemove.Visibility = Visibility.Visible;
                isAddFav = false;
            }
            else
            {
                ButtonAddFav.Background = Brushes.Yellow;
                ButtonEdit.Visibility = Visibility.Hidden;
                ButtonRemove.Visibility = Visibility.Hidden;
                isAddFav = true;
            }
        }




        private void RemoveMode(object sender, RoutedEventArgs e)
        {
            if (isRemove)
            {
                ButtonRemove.Background = Brushes.Red;
                ButtonEdit.Visibility = Visibility.Visible;
                ButtonAddFav.Visibility = Visibility.Visible;
                isRemove = false;
            }
            else
            {
                ButtonRemove.Background = Brushes.Yellow;
                ButtonEdit.Visibility = Visibility.Hidden;
                ButtonAddFav.Visibility = Visibility.Hidden;
                isRemove = true;
            }
        }

        private void UpdateFav()
        {
            List<WorkFlow> macrosListtest = new List<WorkFlow>();
            foreach (WorkFlow m in Favmacros)
            {
                macrosListtest.Add(m);
            }
            Favmacros = macrosListtest;
            ListFav.ItemsSource = Favmacros;
        }
        private void UpdateAllMacro()
        {
            List<WorkFlow> macrosListtest = new();
            foreach (WorkFlow m in macros)
            {
                macrosListtest.Add(m);
            }
            macros = macrosListtest;
            ListMacro.ItemsSource = macrosListtest;
        }
    }
}
