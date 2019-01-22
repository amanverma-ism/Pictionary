using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Pictionary.Capsules.UtilityFiles
{
    public interface IResizeThumbOwner
    {
        void WhileResizing(Point topleft, Point bottomright);
        void OnResizeComplete(Point topleft, Point bottomright);
        double[] GetBoundary();
        double ImageControlWidth { get; set; }
        double ImageControlHeight { get; set; }
        double ImageAngle { get; }
    }
}
