﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public interface IFileService
    {
        List<Phone> Open(string filename);
        void Save(string filename, List<Phone> phonesList);
    }
}
