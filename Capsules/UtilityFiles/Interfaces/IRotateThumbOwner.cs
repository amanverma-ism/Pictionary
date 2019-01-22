using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Pictionary.Capsules.UtilityFiles
{
    public interface IRotateThumbOwner
    {
        void WhileRotating(double angle);
        void OnRotationComplete(double angle);
    }
}
