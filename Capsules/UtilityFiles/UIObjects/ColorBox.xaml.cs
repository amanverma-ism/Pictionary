using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for ColorBox.xaml
    /// </summary>
    public partial class ColorBox : UserControl
    {
        public ColorBox(IColorBoxParent datacontext, Brush color)
        {
            InitializeComponent();
            this.MinHeight = 20;
            this.MinWidth = 20;
            this.Height = 20;
            this.Width = 20;
            this.Background = color;
            this.BorderThickness = new Thickness(2, 2, 2, 2);
            this.BorderBrush = Brushes.Black;
            this.DataContext = datacontext;
            this.MouseDown += ColorBox_MouseDown;
        }

        public ColorBox(Brush color)
        {
            InitializeComponent();
            this.MinHeight = 20;
            this.MinWidth = 20;
            this.Height = 20;
            this.Width = 20;
            this.Background = color;
            this.BorderThickness = new Thickness(2, 2, 2, 2);
            this.BorderBrush = Brushes.Black;
        }

        private void ColorBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as IColorBoxParent).OnColorBoxSelected(this.GetHashCode());
        }

        public Brush GetColor()
        {
            return this.Background;
        }

        public void ClearData()
        {
            this.DataContext = null;
            this.MouseDown -= ColorBox_MouseDown;
        }
    }
}
