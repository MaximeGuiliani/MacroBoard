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

namespace MacroBoard.Resources.Json
{
    /// <summary>
    /// Logique d'interaction pour JSON_TESTWindow.xaml
    /// </summary>
    public partial class JSON_TESTWindow : Window
    {
        public JSON_TESTWindow()
        {
            InitializeComponent();
            ListTest.Items.Add("INIT");

            string JsonPath = @"C:\Users\dadad\Desktop\test.json";

            List<Block> Blocks = new List<Block>();
            Blocks.Add(new BlockRestart());
            Blocks.Add(new BlockClickR());
            Blocks.Add(new BlockClickL());

            WorkFlow WF = new WorkFlow(@"C:\Users\dadad\Desktop\MacroBoard\MacroBoard\Resources\macro_img.png", "WF_Name_Test", Blocks);

            Serialization serialization = new Serialization(JsonPath);

            serialization.Serialize(WF);

            WorkFlow newWF = serialization.Deserialize();

            ListTest.Items.Add(newWF.workflowName);
            ListTest.Items.Add(newWF.imagePath);
            ListTest.Items.Add(newWF.workflowList[1].BlockType);
        }
    }
}
