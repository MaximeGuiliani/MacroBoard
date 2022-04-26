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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        static string name;
        public Page1()
        {
            InitializeComponent();
            name = "Hello";
        }

        public void setName(string name)
        {
            //name = "Hello";
        }

        public string getName()
        {
            return Label_Name.Content.ToString();
        }
    }
}
