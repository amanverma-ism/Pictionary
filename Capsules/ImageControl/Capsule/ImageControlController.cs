using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Pictionary.Capsules
{
    public class ImageControlController : BaseController
    {
        #region Variables
        protected ImageStyleResource _ImageStyleResource;
        #endregion

        #region Constructor

        public ImageControlController(IMainWindow _mainWindowController)
        {
            ClsModel = new ImageControlModel(this);
            PclsImageControlModel.MainWindow = _mainWindowController;
            ClsViewModel = new ImageControlViewModel(PclsImageControlModel);
            ClsUIObject = new UIImageControl();
            _ImageStyleResource = new ImageStyleResource();
            PclsImageControlView.DataContext = PclsImageControlViewModel;
            PclsImageControlViewModel.PropertyChanged += PclsImageControlViewModel_PropertyChanged;
        }

        #endregion

        #region Properties

        private UIImageControl PclsImageControlView
        {
            get { return ClsUIObject as UIImageControl; }
            set { ClsUIObject = value; }
        }

        private ImageControlModel PclsImageControlModel
        {
            get { return ClsModel as ImageControlModel; }
            set { ClsModel = value; }
        }

        private ImageControlViewModel PclsImageControlViewModel
        {
            get { return ClsViewModel as ImageControlViewModel; }
            set { ClsViewModel = value; }
        }

        public double ImageWidth
        {
            get { return PclsImageControlModel.ImageWidth; }
        }

        public double ImageHeight
        {
            get { return PclsImageControlModel.ImageHeight; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Subscriber of the PropertyChanged event of ImageControlViewModel.
        /// </summary>
        /// <param name="sender">ImageControlViewModel as object</param>
        /// <param name="e">PropertyChangedEventArgs containing the PropertyName</param>
        void PclsImageControlViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //switch(e.PropertyName)
            //{
            //}
        }

        /// <summary>
        /// Sends call to model to render the image selected in the mainWindow browse popup.
        /// </summary>
        /// <param name="imagePath"> Path of the image selected in the MainWindow browse popup.</param>
        public void RenderImage(string path)
        {
            PclsImageControlModel.RenderImage(path);
            PclsImageControlView.OnImageLoad();
        }

        /// <summary>
        /// A way for model to communicate with controller if there is any important notification.
        /// </summary>
        /// <param name="notification">The string which is used to pass the information to notify.</param>
        /// <param name="args">If any data is required else null.</param>
        public override void Notify(string notification, object args)
        {
            base.Notify(notification, args);
            switch(notification)
            {
                case "UpdateImageControlStyle":
                    Style _localStyle = _ImageStyleResource.FindResource("ImageContentControlStyle") as Style;
                    PclsImageControlView.Style = _localStyle;
                    _ImageStyleResource.AssignDataContext(PclsImageControlView);
                    Panel.SetZIndex(PclsImageControlView, 1);
                    break;
            }
        }

        /// <summary>
        /// When CanvasSize of MainWindow is changed, the call will come here to controller to pass it to the view to reposition the imagecontrol.
        /// </summary>
        /// <param name="ImageCanvas"> the actual canvas in which image is contained. </param>
        public void CanvasSizeChanged(object ImageCanvas)
        {
            PclsImageControlView.CanvasSizeChanged(ImageCanvas);
        }

        /// <summary>
        /// Gets the RGB Image pixels from the BitmapImage.
        /// </summary>
        /// <returns> Returns the copy of RGB pixels byte array.</returns>
        public byte[] GetImageRGBPixels()
        {
            return PclsImageControlModel.ImageRGBPixelArray.ToArray();
        }

        /// <summary>
        /// To update the ImageSource of Image UI Element
        /// </summary>
        /// <param name="pixelarr">The image RGB pixel array.</param>
        public void UpdateImageSource(byte[] pixelarr)
        {
            PclsImageControlModel.ImageSource = PclsImageControlModel.ConvertByteDataToBitmapSource(pixelarr);
        }

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public override void ClearData()
        {
            _ImageStyleResource = null;
            PclsImageControlViewModel.PropertyChanged -= PclsImageControlViewModel_PropertyChanged;
            PclsImageControlView.ClearData();
            PclsImageControlViewModel.ClearData();
            PclsImageControlModel.ClearData();
            PclsImageControlView = null;
            base.ClearData();
        }

        /// <summary>
        /// Save the Image to specific location on harddrive.
        /// </summary>
        /// <param name="fileName">The location where Image is to be stored.</param>
        public void SaveImage(string fileName)
        {
            PclsImageControlView.SaveImage(fileName);
        }

        /// <summary>
        /// Removes the Resize Style from the image. 
        /// </summary>
        public void RemoveStyle()
        {
            PclsImageControlView.Style = null;
            PclsImageControlView.SelectedState = emSelectedState.Unselected;
            Panel.SetZIndex(PclsImageControlView, 0);
        }

        /// <summary>
        /// Sets the selected state of the view object.
        /// </summary>
        /// <param name="state">State to be set.</param>
        public void SetSelectedState(emSelectedState state)
        {
            PclsImageControlView.SelectedState = state;
        }

        /// <summary>
        /// Hides the scroll viewers irrespective of zoom.
        /// </summary>
        public void HideScrollViewers()
        {
            PclsImageControlViewModel.ScrollVisibilityOverride = true;
            PclsImageControlViewModel.ScrollVisibility = false;
        }

        /// <summary>
        /// Resets the scrollviewer visibility state.
        /// </summary>
        public void ResetScrollViewerVisibilityState()
        {
            PclsImageControlViewModel.ScrollVisibilityOverride = false;
            PclsImageControlViewModel.ScrollVisibility = true;
        }

        public void RegisterImageControlEvents()
        {
            PclsImageControlView.RegisterEvents();
        }

        public void UnRegisterImageControlEvents()
        {
            PclsImageControlView.UnRegisterEvents();
        }
        #endregion
    }
}
