﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pictionary.Capsules.UtilityFiles
{
    public interface IColorBoxParent
    {
        void OnColorBoxSelected(int hashcode);
    }
}
