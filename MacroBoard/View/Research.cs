using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MacroBoard
{
    internal class Research 
    {
        private List<BlockViewModel_Left> BlockViewModels_Left = new();
        private ListBox ListBlock_Left_XAML = new();

        public Research(List<BlockViewModel_Left> blockViewModels_Left, ListBox ListBlock_Left_XAML)
        {
            BlockViewModels_Left = blockViewModels_Left;
            this.ListBlock_Left_XAML = ListBlock_Left_XAML;
        }
    }
}
