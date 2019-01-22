using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pictionary.Capsules.UtilityFiles
{
    public interface IMainWindow
    {
        void OnImageSelected(INotifier imagecontrol);
        UIElement GetView();
    }
}
