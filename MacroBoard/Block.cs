using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MacroBoard{
    public abstract class Block{

        public string Name { get; set; } = "";
        public string LogoUrl { get; set; } = "";

        public abstract void Execute();



    }
}
