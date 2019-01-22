using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pictionary.Capsules;

namespace Pictionary
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainWindowController _objMainWindow = new MainWindowController();
            _objMainWindow.ShowWindow();
            _objMainWindow = null;
        }
    }
}
