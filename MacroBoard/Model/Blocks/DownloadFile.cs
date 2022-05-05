﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace MacroBoard
{
    internal class DownloadFile : Block
    {
        String address;
        String fileName;

        public DownloadFile(String address)
        {
            this.address = address;
            this.fileName = @"C:\Users\user\Downloads\" + Path.GetFileName(address);
        }

        public override void Execute()
        {
           WebClient wc = new WebClient();
            wc.DownloadFile(address,fileName);
        }

    }
}
