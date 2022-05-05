using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MacroBoard
{

    internal abstract class BlockViewModel
    {
        public Label Lbl_Name = new();
        public Grid Content { get; } = new();
        public Block Block { get; set; }

        public BlockViewModel(string Name, Block Block)
        {
            this.Block = Block;
            Setup_Btns();
            Setup_Name(Name);
            Setup_Grid();
        }

        public BlockView(Block block)
        {
            this.Block = block;
            //block.Execute();
            Setup_Name(block.BlockType);
            Setup_Block_Creating_Window(block);
        }

        private void Setup_Block_Creating_Window(Block block)
        {
            
        }

        private void Setup_Name(string Name)
        {
            Lbl_Name.Content = Name;
            Lbl_Name.HorizontalAlignment = HorizontalAlignment.Center;
            Lbl_Name.VerticalAlignment = VerticalAlignment.Center;
        }

        public abstract void Setup_Btns();


        public abstract void Setup_Grid();





    }
}
