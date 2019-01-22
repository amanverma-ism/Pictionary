using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pictionary.Capsules
{
    public class ImageControlModel : BaseModel
    {
        #region Variables

        private ImageSource _ImageSource;
        private double _dblImageControlHeight;
        private double _dblImageControlWidth;
        private double _dblImageHeight;
        private double _dblImageWidth;
        private double _dblZoomValue;
        BitmapImage _bitmapImageSource;
        private double _dblImageLayoutTransformScaleX;
        private double _dblImageLayoutTransformScaleY;
        private double _dblImageLayoutTransformCenterX;
        private double _dblImageLayoutTransformCenterY;
        private double _dblImageRenderTransformAngle;
        private Point _ptImageRenderTransformOrigin;
        private byte[] _byteImagePixelArrayRGB;
        protected IMainWindow _clsMainWindowController;

        #endregion

        #region Constructor

        public ImageControlModel(ImageControlController _parent) : base(_parent)
        {
            SetDefaultModelState();
        }

        #endregion

        #region Properties

        internal IMainWindow MainWindow
        {
            get { return _clsMainWindowController; }
            set { _clsMainWindowController = value; }
        }

        private ImageControlController PclsParentController
        {
            get { return base.ClsParentController as ImageControlController; }
            set { base.ClsParentController = value; }
        }

        public ImageSource ImageSource 
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                _ImageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public BitmapSource ModifiedBitmapSource
        {
            get { return ImageSource as BitmapSource; }
            set { ImageSource = value; }
        }

        public BitmapImage OriginalBitmapImage
        {
            get { return _bitmapImageSource; }
        }


        public double ImageControlWidth 
        { 
            get
            {
                return _dblImageControlWidth;
            }
            set
            {
                _dblImageControlWidth = value;
                OnPropertyChanged("ImageControlWidth");
            }
        }

        public double ImageControlHeight 
        { 
            get
            {
                return _dblImageControlHeight;
            }
            set
            {
                _dblImageControlHeight = value;
                OnPropertyChanged("ImageControlHeight");
            }
        }

        public double ImageWidth
        {
            get
            {
                return _dblImageWidth;
            }
            set
            {
                _dblImageWidth = value;
            }
        }

        public double ImageHeight
        {
            get
            {
                return _dblImageHeight;
            }
            set
            {
                _dblImageHeight = value;
            }
        }

        public double ZoomValue
        {
            get { return _dblZoomValue; }
            set
            {
                _dblZoomValue = value;
                OnPropertyChanged("ZoomValue");
            }
        }

        public double ImageLayoutTransformScaleX
        {
            get { return _dblImageLayoutTransformScaleX; }
            set { _dblImageLayoutTransformScaleX = value; OnPropertyChanged("ImageLayoutTransformScaleX"); }
        }

        public double ImageLayoutTransformScaleY
        {
            get { return _dblImageLayoutTransformScaleY; }
            set { _dblImageLayoutTransformScaleY = value; OnPropertyChanged("ImageLayoutTransformScaleY"); }
        }

        public double ImageLayoutTransformCenterX
        {
            get { return _dblImageLayoutTransformCenterX; }
            set { _dblImageLayoutTransformCenterX = value; OnPropertyChanged("ImageLayoutTransformCenterX"); }
        }

        public double ImageLayoutTransformCenterY
        {
            get { return _dblImageLayoutTransformCenterY; }
            set { _dblImageLayoutTransformCenterY = value; OnPropertyChanged("ImageLayoutTransformCenterY"); }
        }

        public double ImageRenderTransformAngle
        {
            get { return _dblImageRenderTransformAngle; }
            set 
            {
                if (value > 360)
                {
                    value = value % 360;
                }
                else if (value < 0)
                    value = -((-value) % 360) + 360;
                _dblImageRenderTransformAngle = value; 
                OnPropertyChanged("ImageRenderTransformAngle"); 
            }
        }

        public Point ImageControlRenderTransformOrigin
        {
            get { return _ptImageRenderTransformOrigin; }
        }

        public byte[] ImageRGBPixelArray
        {
            get { return _byteImagePixelArrayRGB; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Renders the image selected in the mainWindow browse popup.
        /// </summary>
        /// <param name="imagePath"> Path of the image selected in the MainWindow browse popup.</param>
        internal void RenderImage(string imagePath)
        {
            SetDefaultModelState();
            ImageSource = null;
            _bitmapImageSource = new BitmapImage();
            _bitmapImageSource.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            _bitmapImageSource.CacheOption = BitmapCacheOption.OnLoad;
            _bitmapImageSource.BeginInit();
            _bitmapImageSource.UriSource = new Uri(imagePath);
            _bitmapImageSource.DecodePixelWidth = 1600;
            _bitmapImageSource.EndInit();
            _dblImageHeight = _bitmapImageSource.PixelHeight;
            _dblImageWidth = _bitmapImageSource.PixelWidth;
            _byteImagePixelArrayRGB = GetImageRGBPixels();
            
            double ratio = _dblImageWidth / _dblImageHeight;
            ImageControlHeight = 400;
            ImageControlWidth = _dblImageControlHeight * ratio;
            ImageSource = _bitmapImageSource;
            PclsParentController.Notify("UpdateImageControlStyle", null);
        }

        /// <summary>
        /// Resets the imagecontrol model values like zoom and scaling factor to default.
        /// </summary>
        public override void SetDefaultModelState()
        {
            _dblZoomValue = 1;
            _dblImageLayoutTransformScaleX = 1;
            _dblImageLayoutTransformScaleY = 1;
            _dblImageLayoutTransformCenterX = 0;
            _dblImageLayoutTransformCenterY = 0;
            _ptImageRenderTransformOrigin = new Point(0.5, 0.5);
            _dblImageRenderTransformAngle = 0;    
        }

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public override void ClearData()
        {
            _bitmapImageSource = null;
            _ImageSource = null;
            _byteImagePixelArrayRGB = null;
            base.ClearData();
        }

        /// <summary>
        /// Copies the RGB Image pixels from the BitmapImage.
        /// </summary>
        /// <returns> Returns the RGB pixels byte array.</returns>
        private byte[] GetImageRGBPixels()
        {
            int stride = (int)(_dblImageWidth*4);

            byte[] pixelArr = new byte[stride*(int)_dblImageHeight];
            OriginalBitmapImage.CopyPixels(pixelArr, stride, 0);
            return pixelArr;
        }

        /// <summary>
        /// Converts the RGB pixel byte array into the BitmapSource.
        /// </summary>
        /// <param name="imageData"> Image RGB Pixel array</param>
        /// <returns>Returns the BItmapSource which can be used as ImageSource of BitmapImage.</returns>
        internal BitmapSource ConvertByteDataToBitmapSource(byte[] imageData)
        {
           int stride = (int)(_dblImageWidth *4);
           var imageSrc = BitmapSource.Create((int)_dblImageWidth, (int)_dblImageHeight, ModifiedBitmapSource.DpiX, ModifiedBitmapSource.DpiY, ModifiedBitmapSource.Format, null, imageData, stride);
           return imageSrc;
        }

        /// <summary>
        /// Called when clicked on Image.
        /// </summary>
        internal void OnMouseDown()
        {
            MainWindow.OnImageSelected(PclsParentController as INotifier);
        }

        #endregion
    }
}
