using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Pictionary.Capsules
{
    /// <summary>
    /// Interaction logic for ImageControl.xaml
    /// </summary>
    public partial class UIImageControl : UserControl, IBaseView, IResizeThumbOwner, IRotateThumbOwner
    {
        #region Variables
        private Image _imgMainImage;
        private ScrollViewer _scrollviewer;
        private emSelectedState _emSelectedState;
        private Point topleft;
        private Point bottomright;
        private bool _blnIsDragEnabled;
        #endregion

        #region Constructor
        public UIImageControl()
        {
            InitializeComponent();
            DragThumb.ApplyTemplate();
            ControlTemplate template = DragThumb.Template;
            _imgMainImage = (Image)template.FindName("uiObjImage", DragThumb);
            _scrollviewer = (ScrollViewer)template.FindName("UIScrollViewer", DragThumb);
            RegisterEvents();
            _imgMainImage.MouseDown += Image_MouseDown;
            _emSelectedState = emSelectedState.Selected;
            topleft = new Point();
            bottomright = new Point();
        }

        #endregion

        #region Properties
        public ScrollViewer ImageScrollViewer
        {
            get { return _scrollviewer; }
        }

        public Image ImageUIElement
        {
            get { return _imgMainImage; }
        }

        private ImageControlViewModel PclsImageControlViewModel
        {
            get
            {
                return this.DataContext as ImageControlViewModel;
            }
        }

        public emSelectedState SelectedState
        {
            get { return _emSelectedState; }
            set { _emSelectedState = value; }
        }

        public double ZoomValue
        {
            get { return PclsImageControlViewModel.ZoomValue; }
            set { PclsImageControlViewModel.ZoomValue = value; }
        }

        #endregion

        #region Interfaces
        #region IResizeThumbOwner

        public double ImageAngle
        { get { return PclsImageControlViewModel.ImageRenderTransformAngle; } }

        public double ImageControlWidth
        {
            get { return PclsImageControlViewModel.ImageControlWidth; }
            set { PclsImageControlViewModel.ImageControlWidth = value; }
        }

        public double ImageControlHeight
        {
            get { return PclsImageControlViewModel.ImageControlHeight; }
            set { PclsImageControlViewModel.ImageControlHeight = value; }
        }

        public double[] GetBoundary()
        {
            return new double[] { topleft.X, topleft.Y, bottomright.X, bottomright.Y };
        }

        /// <summary>
        /// Called When any of the six Image Resize thumb is dragged to resize and reposition the imagecontrol.
        /// </summary>
        /// <param name="thumbPosition"> Specifies which one of the six thumbs is dragged. </param>
        /// <param name="e"> Event argument which contains the horizontal and vertical delta change. </param>
        public void WhileResizing(Point newTopLeft, Point newBottomRight)
        {
            topleft = newTopLeft;
            bottomright = newBottomRight;
            Canvas.SetLeft(this, topleft.X);
            Canvas.SetRight(this, bottomright.X);
            Canvas.SetTop(this, topleft.Y);
            Canvas.SetBottom(this, bottomright.Y);
        }

        /// <summary>
        /// Called on drag complete of any of the six Image resize thumbs.
        /// </summary>
        /// <param name="thumbPosition"> Specifies which one of the six thumbs is dragged. </param>
        /// <param name="e"> Event argument which contains the horizontal and vertical delta change. </param>
        public void OnResizeComplete(Point newTopLeft, Point newBottomRight)
        {
            topleft = newTopLeft;
            bottomright = newBottomRight;
            Canvas.SetLeft(this, topleft.X);
            Canvas.SetRight(this, bottomright.X);
            Canvas.SetTop(this, topleft.Y);
            Canvas.SetBottom(this, bottomright.Y);
        }
        #endregion
        #endregion


        #region MouseEvents

        /// <summary>
        /// Subscriber for the MouseEnter event of UIImageObject.
        /// </summary>
        /// <param name="sender"> Publisher UIElement </param>
        /// <param name="e"> MouseEventArgs </param>
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeAll;
            e.Handled = true;
        }

        /// <summary>
        /// Subscriber for the MouseLeave event of UIImageObject.
        /// </summary>
        /// <param name="sender"> Publisher UIElement </param>
        /// <param name="e"> MouseEventArgs </param>
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            e.Handled = true;
        }

        /// <summary>
        /// Subscriber for the MouseWheel event of UIImageObject.
        /// </summary>
        /// <param name="sender"> Publisher UIElement </param>
        /// <param name="e"> EventArg which specifies the wheel rotation amount and direction. </param>
        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Canvas parent = (Canvas)VisualTreeHelper.GetParent(this);
            if (parent.IsMouseOver == true)
            {
                if (e.Delta > 0)
                {
                    ZoomValue += 0.1;
                }
                else
                {
                    ZoomValue -= 0.1;
                }
                if (ZoomValue >= 1 && ZoomValue <= 4)
                {
                    Point position = e.GetPosition(sender as UIElement);
                    PclsImageControlViewModel.ImageLayoutTransformScaleX = PclsImageControlViewModel.ImageLayoutTransformScaleY = ZoomValue;
                    PclsImageControlViewModel.ImageLayoutTransformCenterX = position.X;
                    PclsImageControlViewModel.ImageLayoutTransformCenterY = position.Y;
                    double offsetx = Math.Max(Math.Min((Math.Abs((sender as Image).LayoutTransform.Value.OffsetX)), _scrollviewer.ExtentWidth), 0);
                    double offsety = Math.Max(Math.Min((Math.Abs((sender as Image).LayoutTransform.Value.OffsetY)), _scrollviewer.ExtentHeight), 0);
                    _scrollviewer.ScrollToHorizontalOffset(offsetx);
                    _scrollviewer.ScrollToVerticalOffset(offsety);
                }
                else
                {
                    if (ZoomValue < 1)
                        ZoomValue = 1;
                    else
                        ZoomValue = 4;
                }
            }
            e.Handled = true;

        }

        /// <summary>
        /// subscriber for the MouseDown event of UIImageObject.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(_emSelectedState == emSelectedState.Unselected)
            {
                PclsImageControlViewModel.OnMouseDown(sender, e);
                _emSelectedState = emSelectedState.Selected;
                e.Handled = true;
            }
        }

        #endregion

        #region ImageDragThumb Events

        /// <summary>
        /// Subscriber of DragStarted Event of the Thumb in which the image is contained.
        /// </summary>
        /// <param name="sender"> MainImageThumb </param>
        /// <param name="e"> DragStartedEventArgument </param>
        private void OnDragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if(_blnIsDragEnabled)
                Mouse.OverrideCursor = Cursors.SizeAll;
        }

        private void _ValidateMove(ref Point ptDragDelta)
        {
            System.Windows.Media.GeneralTransform transform = this.TransformToVisual((FrameworkElement)this.Parent);
            Rect R = transform.TransformBounds(new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            double _dblHorizontalChange = ptDragDelta.X;
            double _dblVerticalChange = ptDragDelta.Y;
            Canvas parent = (Canvas)VisualTreeHelper.GetParent(this);

            double _dblParentWidth = parent.ActualWidth;
            double _dblParentHeight = parent.ActualHeight;
            Point centerPt = new Point((R.Left + R.Right) / 2, (R.Top + R.Bottom) / 2);
            if ((centerPt.X + _dblHorizontalChange) < 0)
            {
                ptDragDelta.X = centerPt.X * -1;
            }
            if ((centerPt.Y + _dblVerticalChange) < 0)
            {
                ptDragDelta.Y = centerPt.Y * -1;
            }

            if ((centerPt.X + _dblHorizontalChange) > _dblParentWidth)
            {
                double _dbldiff = ((centerPt.X + _dblHorizontalChange) - _dblParentWidth);
                ptDragDelta.X -= _dbldiff;
            }

            if ((centerPt.Y + _dblVerticalChange) > _dblParentHeight)
            {
                double _dbldiff = ((centerPt.Y + _dblVerticalChange) - _dblParentHeight);
                ptDragDelta.Y -= _dbldiff;
            }
        }

        /// <summary>
        /// Subscriber of DragDelta Event of the Thumb in which the image is contained.
        /// </summary>
        /// <param name="sender"> MainImageThumb </param>
        /// <param name="e"> DragDeltaEventArgument which contains the horizontal and vertical change. </param>
        private void OnDragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_blnIsDragEnabled)
            {
                Point ptDragDelta = new Point(e.HorizontalChange * Math.Cos(PclsImageControlViewModel.ImageRenderTransformAngle * Math.PI / 180) - e.VerticalChange * Math.Sin(PclsImageControlViewModel.ImageRenderTransformAngle * Math.PI / 180), e.HorizontalChange * Math.Sin(PclsImageControlViewModel.ImageRenderTransformAngle * Math.PI / 180) + e.VerticalChange * Math.Cos(PclsImageControlViewModel.ImageRenderTransformAngle * Math.PI / 180));
                _ValidateMove(ref ptDragDelta);

                Canvas parent = (Canvas)VisualTreeHelper.GetParent(this);

                topleft.Offset(ptDragDelta.X, ptDragDelta.Y);
                bottomright.Offset(ptDragDelta.X, ptDragDelta.Y);

                Canvas.SetLeft(this, topleft.X);
                Canvas.SetRight(this, bottomright.X);
                Canvas.SetTop(this, topleft.Y);
                Canvas.SetBottom(this, bottomright.Y);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Subscriber of DragCompleted Event of the Thumb in which the image is contained.
        /// </summary>
        /// <param name="sender"> MainImageThumb </param>
        /// <param name="e"> DragDeltaEventArgument which contains the horizontal and vertical change. </param>
        private void OnDragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (_blnIsDragEnabled)
            {
                topleft.X = Canvas.GetLeft(this);
                topleft.Y = Canvas.GetTop(this);
                bottomright.X = Canvas.GetRight(this);
                bottomright.Y = Canvas.GetBottom(this);
                e.Handled = true;
            }
        }
        #endregion

        #region ScrollViewer Events

        /// <summary>
        /// Subscriber of the MouseEnter event of ImageScrollViewer.
        /// </summary>
        /// <param name="sender"> ScrollViewer containing the image. </param>
        /// <param name="e"> MouseEventArgs. </param>
        void _ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _blnIsDragEnabled = false;
        }

        /// <summary>
        /// Subscriber of the MouseLeave event of ImageScrollViewer.
        /// </summary>
        /// <param name="sender"> ScrollViewer containing the image. </param>
        /// <param name="e"> MouseEventArgs. </param>
        private void ScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            _blnIsDragEnabled = true;
        }
        #endregion

        #region IBaseView

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public void ClearData()
        {
            DragThumb.DragDelta -= OnDragThumb_DragDelta;
            DragThumb.DragStarted -= OnDragThumb_DragStarted;
            DragThumb.DragCompleted -= OnDragThumb_DragCompleted;
            _scrollviewer.MouseLeave -= ScrollViewer_MouseLeave;
            _imgMainImage.MouseEnter -= Image_MouseEnter;
            _imgMainImage.MouseLeave -= Image_MouseLeave;
            _imgMainImage.MouseWheel -= Image_MouseWheel;
            _imgMainImage.MouseDown -= Image_MouseDown;
            _imgMainImage = null;
            _scrollviewer = null;
            this.DataContext = null;
        }
        #endregion

        internal void RegisterEvents()
        {
            DragThumb.DragDelta += OnDragThumb_DragDelta;
            DragThumb.DragStarted += OnDragThumb_DragStarted;
            DragThumb.DragCompleted += OnDragThumb_DragCompleted;
            _scrollviewer.ScrollChanged += _ScrollViewerScrollChanged;
            _scrollviewer.MouseLeave += ScrollViewer_MouseLeave;
            _imgMainImage.MouseEnter += Image_MouseEnter;
            _imgMainImage.MouseLeave += Image_MouseLeave;
            _imgMainImage.MouseWheel += Image_MouseWheel;
        }

        internal void UnRegisterEvents()
        {
            DragThumb.DragDelta -= OnDragThumb_DragDelta;
            DragThumb.DragStarted -= OnDragThumb_DragStarted;
            DragThumb.DragCompleted -= OnDragThumb_DragCompleted;
            _scrollviewer.ScrollChanged -= _ScrollViewerScrollChanged;
            _scrollviewer.MouseLeave -= ScrollViewer_MouseLeave;
            _imgMainImage.MouseEnter -= Image_MouseEnter;
            _imgMainImage.MouseLeave -= Image_MouseLeave;
            _imgMainImage.MouseWheel -= Image_MouseWheel;
        }

        /// <summary>
        /// Called on the render of the image to UIImageObject.
        /// </summary>
        internal void OnImageLoad()
        {
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            Canvas.SetRight(this, PclsImageControlViewModel.ImageControlWidth);
            Canvas.SetBottom(this, PclsImageControlViewModel.ImageControlHeight);
            topleft = new Point(0, 0);
            bottomright = new Point(PclsImageControlViewModel.ImageControlWidth, PclsImageControlViewModel.ImageControlHeight);
        }


        /// <summary>
        /// Save the loaded image to file.
        /// </summary>
        /// <param name="fileName">Path where to save the image.</param>
        internal void SaveImage(string fileName)
        {
            var encoder = new JpegBitmapEncoder();
            BitmapFrame frame = BitmapFrame.Create(ImageUIElement.Source as BitmapSource);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }

        /// <summary>
        /// When CanvasSize of MainWindow is changed, the call will come here to reposition the imagecontrol considering the bounds.
        /// </summary>
        /// <param name="ImageCanvas"> the actual canvas in which image is contained. </param>
        internal void CanvasSizeChanged(object ImageCanvas)
        {

            double CanvasHeight = ((Canvas)ImageCanvas).ActualHeight;
            double CanvasWidth = ((Canvas)ImageCanvas).ActualWidth;

            Point _topleft = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            Point _bottomright = new Point(Canvas.GetRight(this), Canvas.GetBottom(this));
            Point centerPt = new Point((_topleft.X + _bottomright.X) / 2, (_topleft.Y + _bottomright.Y) / 2);

            if (centerPt.X > CanvasWidth)
            {
                _topleft.X -= (centerPt.X - CanvasWidth);
                _bottomright.X -= (centerPt.X - CanvasWidth);
            }

            if (centerPt.Y > CanvasHeight)
            {
                _topleft.Y -= (centerPt.Y - CanvasWidth);
                _bottomright.Y = (centerPt.Y - CanvasWidth);
            }
            Canvas.SetLeft(this, _topleft.X);
            Canvas.SetTop(this, _topleft.Y);
            Canvas.SetRight(this, _bottomright.X);
            Canvas.SetBottom(this, _bottomright.Y);
            topleft = _topleft;
            bottomright = _bottomright;
        }

        #region IRotateThumbOwner
        public void WhileRotating(double angle)
        {
            PclsImageControlViewModel.ImageRenderTransformAngle = angle;
        }

        public void OnRotationComplete(double angle)
        {
            topleft.X = Canvas.GetLeft(this);
            topleft.Y = Canvas.GetTop(this);
            bottomright.X = Canvas.GetRight(this);
            bottomright.Y = Canvas.GetBottom(this);
        }
        #endregion

        private void UIScrollViewer_Scroll(object sender, ScrollEventArgs e)
        {
            DragThumb.DragDelta -= OnDragThumb_DragDelta;
            DragThumb.DragStarted -= OnDragThumb_DragStarted;
            DragThumb.DragCompleted -= OnDragThumb_DragCompleted;
            e.Handled = true;
        }
    }
}
