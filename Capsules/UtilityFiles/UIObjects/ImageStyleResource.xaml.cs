using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pictionary.Capsules.UtilityFiles
{
    /// <summary>
    /// Interaction logic for ImageStyleResource.xaml
    /// </summary>
    public partial class ImageStyleResource : UserControl
    {
        public ImageStyleResource()
        {
            InitializeComponent();
        }

        public void AssignDataContext(ContentControl contentControl)
        {
            contentControl.ApplyTemplate();

            ControlTemplate controlTemplate = this.FindResource("ImageStyleTemplate") as ControlTemplate;
            Control resizeControl = controlTemplate.FindName("ResizeControl", contentControl) as Control;
            resizeControl.ApplyTemplate();
            ControlTemplate resizeTemplate = this.FindResource("ResizeDecoratorTemplate") as ControlTemplate;

            Thumb resizeThumbTopLeft = resizeTemplate.FindName("ThumbTopLeft", resizeControl) as Thumb;
            resizeThumbTopLeft.DataContext = contentControl;

            Thumb resizeThumbTopRight = resizeTemplate.FindName("ThumbTopRight", resizeControl) as Thumb;
            resizeThumbTopRight.DataContext = contentControl;

            Thumb resizeThumbBottomLeft = resizeTemplate.FindName("ThumbBottomLeft", resizeControl) as Thumb;
            resizeThumbBottomLeft.DataContext = contentControl;

            Thumb resizeThumbBottomRight = resizeTemplate.FindName("ThumbBottomRight", resizeControl) as Thumb;
            resizeThumbBottomRight.DataContext = contentControl;

            Thumb resizeThumbTopCenter = resizeTemplate.FindName("ThumbTopCenter", resizeControl) as Thumb;
            resizeThumbTopCenter.DataContext = contentControl;

            Thumb resizeThumbBottomCenter = resizeTemplate.FindName("ThumbBottomCenter", resizeControl) as Thumb;
            resizeThumbBottomCenter.DataContext = contentControl;

            Thumb resizeThumbCenterLeft = resizeTemplate.FindName("ThumbLeftCenter", resizeControl) as Thumb;
            resizeThumbCenterLeft.DataContext = contentControl;

            Thumb resizeThumbCenterRight = resizeTemplate.FindName("ThumbRightCenter", resizeControl) as Thumb;
            resizeThumbCenterRight.DataContext = contentControl;

            Thumb rotateThumb = resizeTemplate.FindName("ThumbRotate", resizeControl) as Thumb;
            rotateThumb.DataContext = contentControl;
        }
    }
}
