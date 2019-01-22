using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pictionary.Capsules
{
    public class ImageControlViewModel : BaseViewModel
    {
        #region Variables
        private bool _blnScrollVisibilityOverride;
        private bool _blnScrollVisibiltiy;
        #endregion

        #region Constructor

        public ImageControlViewModel(ImageControlModel imageControlModel) : base(imageControlModel)
        {
            PclsImageControlModel.PropertyChanged += _clsImageControlModel_PropertyChanged;
            _blnScrollVisibilityOverride = false;
        }

        #endregion

        #region Properties

        public bool ScrollVisibility
        {
            get { return _blnScrollVisibiltiy; }
            set { _blnScrollVisibiltiy = value; OnPropertyChanged("ScrollViewerVisibility"); }
        }

        public bool ScrollVisibilityOverride
        {
            get { return _blnScrollVisibilityOverride; }
            set { _blnScrollVisibilityOverride = value; }
        }

        private ImageControlModel PclsImageControlModel
        {
            get { return ClsModel as ImageControlModel; }
            set { ClsModel = value; }
        }

        public ImageSource ImageSource
        {
            get { return PclsImageControlModel.ImageSource; }
            set { PclsImageControlModel.ImageSource = value; }
        }

        public double ImageControlWidth
        {
            get
            {
                return PclsImageControlModel.ImageControlWidth;
            }
            set
            {
                PclsImageControlModel.ImageControlWidth = value;
            }
        }

        public double ImageControlHeight
        {
            get
            {
                return PclsImageControlModel.ImageControlHeight;
            }
            set
            {
                PclsImageControlModel.ImageControlHeight = value;
            }
        }

        public ScrollBarVisibility ScrollViewerVisibility
        {
            get
            {
                if(_blnScrollVisibilityOverride)
                {
                    if (_blnScrollVisibiltiy == true)
                        return ScrollBarVisibility.Visible;
                    else
                        return ScrollBarVisibility.Hidden;
                }
                else
                    return ZoomValue > 1 ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden; }
        }

        public double ZoomValue
        {
            get { return PclsImageControlModel.ZoomValue; }
            set { PclsImageControlModel.ZoomValue = value; }
        }
        
        public double ImageLayoutTransformScaleX
        {
            get { return PclsImageControlModel.ImageLayoutTransformScaleX; }
            set { PclsImageControlModel.ImageLayoutTransformScaleX = value; }
        }

        public double ImageLayoutTransformScaleY
        {
            get { return PclsImageControlModel.ImageLayoutTransformScaleY; }
            set { PclsImageControlModel.ImageLayoutTransformScaleY = value; }
        }

        public double ImageLayoutTransformCenterX
        {
            get { return PclsImageControlModel.ImageLayoutTransformCenterX; }
            set { PclsImageControlModel.ImageLayoutTransformCenterX = value; }
        }

        public double ImageLayoutTransformCenterY
        {
            get { return PclsImageControlModel.ImageLayoutTransformCenterY; }
            set { PclsImageControlModel.ImageLayoutTransformCenterY = value; }
        }

        public double ImageRenderTransformAngle
        {
            get { return PclsImageControlModel.ImageRenderTransformAngle; }
            set { PclsImageControlModel.ImageRenderTransformAngle = value; }
        }

        public Point ImageControlRenderTransformOrigin
        {
            get { return PclsImageControlModel.ImageControlRenderTransformOrigin; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Subscriber of the PropertyChanged event of ImageControlModel.
        /// </summary>
        /// <param name="sender">ImageControlModel as object</param>
        /// <param name="e">PropertyChangedEventArgs containing the PropertyName</param>
        void _clsImageControlModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ImageSource":
                    OnPropertyChanged("ImageSource");
                    OnPropertyChanged("ZoomValue");
                    OnPropertyChanged("ImageLayoutTransformScaleX");
                    OnPropertyChanged("ImageLayoutTransformScaleY");
                    OnPropertyChanged("ImageLayoutTransformCenterX");
                    OnPropertyChanged("ImageLayoutTransformCenterY");
                    OnPropertyChanged("ScrollViewerVisibility");
                    OnPropertyChanged("ImageRenderTransformAngle");
                    OnPropertyChanged("ImageControlRenderTransformOrigin");
                    break;
                case "ImageControlWidth":
                    OnPropertyChanged("ImageControlWidth");
                    break;
                case "ImageControlHeight":
                    OnPropertyChanged("ImageControlHeight");
                    break;
                case "ZoomValue":
                    OnPropertyChanged("ZoomValue");
                    OnPropertyChanged("ScrollViewerVisibility");
                    break;
                case "IsImageHitTestVisible":
                    OnPropertyChanged("IsImageHitTestVisible");
                    break;
                case "ImageLayoutTransformScaleX":
                    OnPropertyChanged("ImageLayoutTransformScaleX");
                    break;
                case "ImageLayoutTransformScaleY":
                    OnPropertyChanged("ImageLayoutTransformScaleY");
                    break;
                case "ImageLayoutTransformCenterX":
                    OnPropertyChanged("ImageLayoutTransformCenterX");
                    break;
                case "ImageLayoutTransformCenterY":
                    OnPropertyChanged("ImageLayoutTransformCenterY");
                    break;
                case "ImageRenderTransformAngle":
                    OnPropertyChanged("ImageRenderTransformAngle");
                    break;
                case "ImageControlRenderTransformOrigin":
                    OnPropertyChanged("ImageControlRenderTransformOrigin");
                    break;
            }
        }

        /// <summary>
        /// subscriber for the MouseDown event of UIImageObject.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PclsImageControlModel.OnMouseDown();
        }

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public override void ClearData()
        {
            PclsImageControlModel.PropertyChanged -= _clsImageControlModel_PropertyChanged;
            base.ClearData();
        }

        #endregion
    }
}
