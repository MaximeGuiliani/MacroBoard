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
using System.Windows.Shapes;
using System.IO;
using static MacroBoard.Utils;
using System.Text.RegularExpressions;
using MacroBoard;



namespace MacroBoard.View
{

    public partial class BlockCreatorWindow : Window, IBlockVisitor
    {

        Func<Block> newBlock;   // lambda-expression appelée lors du click "valider", qui défini comment créer un block
        public Block res;       // nouveau block créé par la fenetre
        Block model;            // block pris en modèle pour remplir les champs de la fenetre
        Fields fields;


        //---------------------------------------------------------------------------

        public BlockCreatorWindow(Block model)
        {
            InitializeComponent();
            this.model = model;
            this.fields = new Fields(Controls, this);
            create();
        }


        //---------------------------------------------------------------------------

        public void create()
        {
            AddTitle();
            model.Accept(this);
            AddHandlerToValiderBtn();
        }

        //---------------------------------------------------------------------------

        public void Visit(BlockClickR b)
        {
            newBlock = () => new BlockClickR();
        }


        public void Visit(BlockClickL b)
        {
            newBlock = () => new BlockClickL();
        }


        public void Visit(BlockCloseDesiredApplication b)
        {
            (Label, TextBox) appName    = fields.newTextBox("App Name", b.appName);
            ComboBox         exactMatch = fields.newComboBoxBool("Exact match on app name", b.exactMatch);
            ComboBox         kill       = fields.newComboBoxBool("Kill the app ? (or soft close)", b.kill);
            newBlock = () => new BlockCloseDesiredApplication(appName.Item2.Text, (bool)exactMatch.SelectedItem, (bool)kill.SelectedItem);
        }


        public void Visit(BlockCreateTextFile b)
        {
            (TextBox, Button) filePath = fields.newSave("File Path", concatPathWithFileName(b.filePath, b.fileName));
            (Label, TextBox) text = fields.newTextBox("texte", b.text);
            newBlock = () => new BlockCreateTextFile(System.IO.Path.GetDirectoryName(filePath.Item1.Text), System.IO.Path.GetFileName(filePath.Item1.Text), text.Item2.Text);
        }


        public void Visit(BlockDeleteDirectory b)
        {
            (TextBox, Button) dirPath = fields.newFolderSelector("element à supprimer", b.path);
            newBlock = () => new BlockDeleteDirectory(dirPath.Item1.Text);
        }


        public void Visit(BlockDownloadFile b)
        {
            (Label, TextBox) address = fields.newTextBox("Web address", b.address);
            (TextBox, Button) folderPath = fields.newFolderSelector("Folder Path", b.folderPath);
            newBlock = () => new BlockDownloadFile(address.Item2.Text, folderPath.Item1.Text);
        }


        public void Visit(BlockHibernate b)
        {
            newBlock = () => new BlockHibernate();
        }


        public void Visit(BlockInvokeAutomationId b)
        {
            (TextBox, Button) automationId = fields.newAutomationIdPicker("Automation Id", b.automationID);
            //(Label, TextBox) automationId = fields.newTextBox("Automation Id", b.automationID);
            newBlock = () => new BlockInvokeAutomationId(automationId.Item1.Text);
        }


        public void Visit(BlockKeyBoard b)
        {
            (Label, TextBox) shortcut = fields.newTextBox("Shortcut", b._shortCut);
            newBlock = () => new BlockKeyBoard(shortcut.Item2.Text);
        }


        public void Visit(BlockLaunchBrowserChrome b)
        {
            (Label, TextBox) address = fields.newTextBox("adresse web", b.address);
            (Label, TextBox) delay = fields.newTextBox("Delay before applying window style (ms)", b.delay.ToString(), fields.CheckDigits);
            ComboBox comboBox = fields.newComboBoxList("Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockLaunchBrowserChrome(address.Item2.Text, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text));
        }


        public void Visit(BlockLaunchBrowserFirefox b)
        {
            (Label, TextBox) address = fields.newTextBox("Adresse web", b.address);
            (Label, TextBox) delay = fields.newTextBox("Delay before applying window style (ms)", b.delay.ToString(), fields.CheckDigits);
            ComboBox comboBox = fields.newComboBoxList("Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockLaunchBrowserFirefox(address.Item2.Text, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text));
        }


        public void Visit(BlockLaunchBrowserEdge b)
        {
            (Label, TextBox) address = fields.newTextBox("Adresse web", b.address);
            (Label, TextBox) delay = fields.newTextBox("Delay before applying window style (ms)", b.delay.ToString(), fields.CheckDigits);
            ComboBox comboBox = fields.newComboBoxList("Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockLaunchBrowserEdge(address.Item2.Text, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text));
        }


        public void Visit(BlockLock b)
        {
            newBlock = () => new BlockLock();
        }


        public void Visit(BlockMove b)
        {
            (TextBox, Button) source = fields.newFolderSelector("Source directory", b.source);
            (TextBox, Button) destination = fields.newFolderSelector("Destination directory", b.destination);
            newBlock = () => new BlockMove(source.Item1.Text, destination.Item1.Text);
        }


        public void Visit(BlockMessageBox b)
        {
            (Label, TextBox) param1 = fields.newTextBox("param 1", b.param1);
            (Label, TextBox) param2 = fields.newTextBox("param 2", b.param2);
            newBlock = () => new BlockMessageBox(param1.Item2.Text, param2.Item2.Text);
        }


        public void Visit(BlockRecognition b)
        {
            (Label, TextBox, Button, Button) templatePath = fields.newScreenShotPicker("Template Path", b.templatePath);
            (Label, TextBox)  xInterest      = fields.newTextBox("X en haut à gauche de la zone de recherche\n(0 pour tout l'écran)", b.xInterest.ToString(), fields.CheckDigits);
            (Label, TextBox)  yInterest      = fields.newTextBox("Y en haut à gauche de la zone de recherche\n(0 pour tout l'écran)", b.yInterest.ToString(), fields.CheckDigits);
            (Label, TextBox)  heightInterest = fields.newTextBox("Hauteur de la zone de recherche\n(0 pour tout l'écran)", b.heightInterest.ToString(), fields.CheckDigits);
            (Label, TextBox)  widthInterest  = fields.newTextBox("Largeur de la zone de recherche\n(0 pour tout l'écran)", b.widthInterest.ToString(), fields.CheckDigits);
            ComboBox          screenNumber   = fields.newComboBoxList("Screen selection", System.Windows.Forms.Screen.AllScreens, b.screenNumber, fields.dataTemplateForScreen());
            (Label, TextBox)  offSetX        = fields.newTextBox("Décalge horizontal de la souris", b.offSetX.ToString(), fields.CheckDigits);
            (Label, TextBox)  offSetY        = fields.newTextBox("Décalge vertical de la souris", b.offSetY.ToString(), fields.CheckDigits);
            (Label, TextBox)  scale          = fields.newTextBox("Scale de l'image\n[0,1[ rétrecir\t]1,+inf] agrandir", b.scale.ToString(), fields.CheckDigits);
            ComboBox          loop           = fields.newComboBoxBool("Essayer plusieurs scale", b.loop);
            ComboBox          debugMode      = fields.newComboBoxBool("DebugMode", b.debugMode);
            newBlock = () => new BlockRecognition(templatePath.Item2.Text, xInterest: ((xInterest.Item2.Text == "*") ? 0 : int.Parse(xInterest.Item2.Text)), yInterest: int.Parse(yInterest.Item2.Text), heightInterest: int.Parse(heightInterest.Item2.Text), widthInterest: int.Parse(widthInterest.Item2.Text), screenNumber: screenNumber.SelectedIndex, offSetX: int.Parse(offSetX.Item2.Text), offSetY: int.Parse(offSetY.Item2.Text), scale: int.Parse(scale.Item2.Text), loop: (bool)loop.SelectedItem, debugMode: (bool)debugMode.SelectedItem);          
        }


        public void Visit(BlockRestart b)
        {
            newBlock = () => new BlockRestart();
        }


        public void Visit(BlockLaunchApp b) //TODO
        {
            (TextBox, Button) appPath = fields.newFileSelector("Chemin de l'app", b.appPath);
            (Label, TextBox) arguments = fields.newTextBox("arguments", b.arguments);
            /*(Label, TextBox) delay = fields.newTextBox("delay before applying window style (ms)", b.delay.ToString());
            ComboBox comboBox = fields.newComboBoxList("Affichage", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);*/
            newBlock = () => new BlockLaunchApp(appPath.Item1.Text, arguments.Item2.Text/*, (WindowStyle)comboBox.SelectedItem, int.Parse(delay.Item2.Text)*/);
        }


        public void Visit(BlockRunScript b)
        {
            (Label, TextBox) script = fields.newTextBox("script", b.script);
            newBlock = () => new BlockRunScript(script.Item2.Text);
        }


        public void Visit(BlockSendEmail b)
        {
            (Label, TextBox) subject = fields.newTextBox("subject", b.subject);
            (Label, TextBox) to = fields.newTextBox("to", b.to);
            (Label, TextBox) body = fields.newTextBox("body", b.body);
            newBlock = () => new BlockSendEmail(body.Item2.Text, to.Item2.Text, subject.Item2.Text);
        }


        public void Visit(BlockSetCursor b)
        {
            (Label, TextBox) x = fields.newTextBox("x", b.x.ToString(), fields.CheckDigits);
            (Label, TextBox) y = fields.newTextBox("y", b.y.ToString(), fields.CheckDigits);
            newBlock = () => new BlockSetCursor(int.Parse(x.Item2.Text), int.Parse(y.Item2.Text));
        }


        public void Visit(BlockShutdown b)
        {
            newBlock = () => new BlockShutdown();
        }


        public void Visit(BlockWait b)
        {
            (Label, TextBox) mili = fields.newTextBox("milisecondes", b.mili.ToString(), fields.CheckDigits);
            (Label, TextBox) sec = fields.newTextBox("secondes", b.sec.ToString(), fields.CheckDigits);
            (Label, TextBox) min = fields.newTextBox("minutes", b.min.ToString(), fields.CheckDigits);
            (Label, TextBox) hour = fields.newTextBox("heures", b.hour.ToString(), fields.CheckDigits);
            newBlock = () => new BlockWait(int.Parse(mili.Item2.Text), int.Parse(sec.Item2.Text), int.Parse(min.Item2.Text), int.Parse(hour.Item2.Text));
        }


        public void Visit(BlockCopy b)
        {
            (TextBox, Button) src = fields.newFolderSelector("Source directory", b.source);
            (TextBox, Button) dest = fields.newFolderSelector("Destination directory", b.destination);
            newBlock = () => new BlockCopy(src.Item1.Text, dest.Item1.Text);
        }


        public void Visit(BlockScreenshot b)
        {
            (TextBox, Button) filePath = fields.newSave("Path to save", concatPathWithFileName(b.folderPath, b.fileName));
            ComboBox screenNumber = fields.newComboBoxList("Screen selection", System.Windows.Forms.Screen.AllScreens, b.screenNumber, fields.dataTemplateForScreen());
            newBlock = () => new BlockScreenshot(System.IO.Path.GetDirectoryName(filePath.Item1.Text), System.IO.Path.GetFileName(filePath.Item1.Text), screenNumber.SelectedIndex);
        }


        public void Visit(BlockWindowStyle b)
        {
            ComboBox comboBox = fields.newComboBoxList("Foreground App Window Style", Enum.GetValues(typeof(WindowStyle)), b.windowStyle);
            newBlock = () => new BlockWindowStyle((WindowStyle)comboBox.SelectedItem);
        }


        public void Visit(BlockCopyFile b)
        {
            (TextBox, Button) src  = fields.newFileSelector("File to copy", b.source);
            (TextBox, Button) dest = fields.newFolderSelector("Destination directory", b.destination);
            newBlock = () => new BlockCopyFile(src.Item1.Text, dest.Item1.Text);
        }


        public void Visit(BlockDeleteFile b)
        {
            (TextBox, Button) filePath = fields.newFileSelector("file to delete", b.path);
            newBlock = () => new BlockDeleteFile(filePath.Item1.Text);
        }


        public void Visit(BlockMoveFile b)
        {
            (TextBox, Button) source = fields.newFileSelector("Select file to move", b.source);
            (TextBox, Button) destination = fields.newFolderSelector("Destination directory", b.destination);
            newBlock = () => new BlockMoveFile(source.Item1.Text, destination.Item1.Text);
        }


        //-----------------------------------------------------------------------------------

        public void AddTitle()
        {
            Label blockTitle = new Label();
            blockTitle.Content = "Block " + model.Name;
            blockTitle.FontSize = 18;
            blockTitle.HorizontalAlignment = HorizontalAlignment.Left;
            blockTitle.FontWeight = FontWeights.Bold;
            blockTitle.Margin = new Thickness(0, 0, 10, 0);
            //blockTitle.FontStyle
            Controls.Children.Add(blockTitle);
        }


        public void AddHandlerToValiderBtn()
        {
            validerBtn.Click += (object sender, RoutedEventArgs e) => {
                if (!allTextBoxesAreOk())
                {
                    MessageBox.Show("Please fill the fields");
                    return;
                }
                try
                {
                res = newBlock();
                }catch(Exception ex)
                {
                    MessageBox.Show("Please fill fields with correct values :\n" + ex.Message);
                    return;
                }
                this.DialogResult = true;
            };
        }


        /*
         * Call the recurvive Fonc textBoxesAreOk() on all element on this.Controls
         */
        private bool allTextBoxesAreOk()
        {
            //TODO: créer une Class/Enum PlaceHolder pour etre sur de pas en rater 
            List<string> placeHolders = new() { "", @"c:\", @"http:\\", @"https:\\", "filename", "blabla", @"c:\filename", "filePath" };
            bool ok = true;
            int i = 0;
            foreach (object child in Controls.Children)
            {
                bool res = textBoxesAreOk(child, placeHolders); //TODO: pourquoi je peux pas le faire une seule ligne !?
                ok = ok && res;
            }
            return ok;
        }


        /*
         * Recursive fonc, if element is TextBox -> check it, if element is a panel explore it recursively
         */
        private bool textBoxesAreOk(Object o, List<string> placeHolders)
        {
            bool ok = true;
            if (o is TextBox)
            {
                TextBox textBox = (TextBox)o;
                foreach(string placeHolder in placeHolders)
                {
                    if (placeHolder.ToLower().Equals(textBox.Text.ToLower()))
                    {
                        textBox.BorderBrush = Brushes.Red;
                        ok = false;
                    }
                }
            }
            else if(o is Panel)
            {
                Panel panel = (Panel)o;
                foreach(Object child in panel.Children)
                {
                    ok = ok && textBoxesAreOk(child, placeHolders);
                }
            }
            return ok;
        }


 








    }
}
