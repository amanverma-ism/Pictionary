using ImageProcessingCppWrapper;
using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Pictionary.Capsules
{
    public class MainWindowController : BaseController, IMainWindow
    {
        #region Variables
        private List<ImageControlController> _lstImageControlController;
        private Dictionary<int, string> _dctImagePaths;
        private ImageControlController _activeImageControlController;
        #endregion

        #region Constructor
        public MainWindowController()
        {
            ClsUIObject = new UIMainWindow(this);
            ClsModel = new MainWindowModel(this);
            ClsViewModel = new MainWindowViewModel(PclsMainWindowModel);
            PclsMainWindowView.DataContext = PclsMainWindowViewModel;
            _lstImageControlController = new List<ImageControlController>();
            _dctImagePaths = new Dictionary<int,string>();
        }
        #endregion

        #region Properties

        private MainWindowModel PclsMainWindowModel
        {
            get { return ClsModel as MainWindowModel; }
            set { ClsModel = value; }
        }

        private MainWindowViewModel PclsMainWindowViewModel
        {
            get { return ClsViewModel as MainWindowViewModel; }
            set { ClsViewModel = value; }
        }

        internal UIMainWindow PclsMainWindowView
        {
            get { return ClsUIObject as UIMainWindow; }
            set { ClsUIObject = value; }
        }

        public List<ImageControlController> ImageControlControllerList
        {
            get { return _lstImageControlController; }
        }

        public ImageControlController ActiveImageControlController
        {
            get { return _activeImageControlController; }
            set { _activeImageControlController = value; }
        }

        #endregion

        #region Methods

        #region Overridden methods

        /// <summary>
        /// A way for model to communicate with controller if there is any important notification.
        /// </summary>
        /// <param name="notification">The string which is used to pass the information to notify.</param>
        /// <param name="args">If any data is required else null.</param>
        public override void Notify(string notification, object args)
        {
            switch(notification)
            {
                case "BrightnessValueChanged":
                case "BlurFactorChanged":
                case "LuminanceFactorChanged":
                case "SaturationFactorChanged":
                case "HueValueChanged":
                case "ContrastFactorChanged":
                    UpdateImageControlOnFilterApply(PclsMainWindowModel.HueValue, PclsMainWindowModel.SaturationFactor, PclsMainWindowModel.BrightnessValue, PclsMainWindowModel.BlurFactor, PclsMainWindowModel.LuminanceFactor, PclsMainWindowModel.ContrastFactor, PclsMainWindowModel.IsPerwittEdgeDetectionChecked);
                    break;
                case "PerwittEdgeDetectionStateChanged":
                    UpdateImageControlOnPerwittEdgeDetectionStateChanged();
                    break;
                case "DeleteSelectedImage":
                    OnDeleteSelectedImage();
                    break;
                case "ImageCanvasSizeChanged":
                    OnImageCanvasSizeChanged(args);
                    break;
                case "BackgroundColorChanged":
                    PclsMainWindowView.SelectedColorViewBox.Children.Clear();
                    PclsMainWindowView.SelectedColorViewBox.Children.Add(new ColorBox(PclsMainWindowViewModel.SelectedBackgroundColor.GetColor()));
                    break;
            }
        }

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public override void ClearData()
        {
            
            foreach(ImageControlController obj in ImageControlControllerList)
            {
                obj.ClearData();
            }
            ImageControlControllerList.Clear();
            _dctImagePaths.Clear();
            _activeImageControlController = null;

            PclsMainWindowView.ClearData();
            PclsMainWindowViewModel.ClearData();
            PclsMainWindowModel.ClearData();
            base.ClearData();
        }
        #endregion

        /// <summary>
        /// When user clicks on Delete selected Image button, call comes here.
        /// </summary>
        private void OnDeleteSelectedImage()
        {
            //Start- Remove the ImageControl being deleted from View, Pathdictionary, ImagecontrolControllerList, its state from model.
            PclsMainWindowView.ImageCanvas.Children.Remove(ActiveImageControlController.GetView());
            _dctImagePaths.Remove(ActiveImageControlController.GetHashCode());
            ImageControlControllerList.Remove(ActiveImageControlController);
            PclsMainWindowModel.DeleteFilterStateFromDataTable(ActiveImageControlController.GetHashCode());
            //End- Remove the ImageControl being deleted from View, Pathdictionary, ImagecontrolControllerList

            //Start- Destroy the object.
            ActiveImageControlController.ClearData();
            ActiveImageControlController = null;
            //End- Destroy the object.

            //Start- Set any other image control controller as selected or reset everything to default.
            if (ImageControlControllerList.Count > 0)
            {
                _activeImageControlController = ImageControlControllerList.Last();
                _activeImageControlController.RegisterImageControlEvents();
                _activeImageControlController.Notify("UpdateImageControlStyle", null);
                _activeImageControlController.SetSelectedState(emSelectedState.Selected);
                PclsMainWindowView.FileNameTextBox.Text = _dctImagePaths[ActiveImageControlController.GetHashCode()];
                PclsMainWindowModel.ImportFilterStateToModel(ActiveImageControlController.GetHashCode());
            }
            else
            {
                PclsMainWindowModel.SetDefaultModelState();
                PclsMainWindowView.FileNameTextBox.Text = "";
                PclsMainWindowViewModel.OnPropertyChange("IsImageOperationsPanelEnabled");
                PclsMainWindowViewModel.OnPropertyChange("IsDeleteButtonEnabled");
            }
            //End- Set any other image control controller as selected or reset everything to default.
        }

        /// <summary>
        /// Called from BrowseButtonClick method to pass the info that the image has been selected and ok is pressed.
        /// </summary>
        /// <param name="imagePath">Path of the selected image.</param>
        public void OnImageLoad(string imagePath)
        {
            //Start- Remove the style of previous imagecontrol and export its state. also set the model to default values.
            if (_activeImageControlController != null)
            {
                _activeImageControlController.RemoveStyle();
                PclsMainWindowModel.ExportFilterStateToDataTable(_activeImageControlController.GetHashCode());
                _activeImageControlController.UnRegisterImageControlEvents();
            }
            PclsMainWindowModel.SetDefaultModelState();
            //End- Remove the style of previous imagecontrol and export its state. also set the model to default values.

            //Start- Create new ImageControl and add it to Imagecontrolcontrollerlist, its view to canvas, add its path, and its state to model.
            _activeImageControlController = new ImageControlController(this);
            _lstImageControlController.Add(_activeImageControlController);
            _dctImagePaths.Add(_activeImageControlController.GetHashCode(), imagePath);
            PclsMainWindowModel.ExportFilterStateToDataTable(_activeImageControlController.GetHashCode());
            PclsMainWindowView.ImageCanvas.Children.Add(ActiveImageControlController.GetView());
            //End- Create new ImageControl and add it to Imagecontrolcontrollerlist, its view to canvas, add its path, and its state to model.

            //Render the image and set its canvas position.
            ActiveImageControlController.RenderImage(imagePath);

            //Update window states and image style.
            PclsMainWindowViewModel.OnPropertyChange("IsImageOperationsPanelEnabled");
            PclsMainWindowViewModel.OnPropertyChange("IsDeleteButtonEnabled");
            _activeImageControlController.Notify("UpdateImageControlStyle", null);
        }

        /// <summary>
        /// Launches the MainWindow.
        /// </summary>
        public void ShowWindow()
        {
            PclsMainWindowView.ShowDialog();
            PclsMainWindowView = null;
        }


        /// <summary>
        /// Called on canvas size change.
        /// </summary>
        /// <param name="args"> Image Canvas </param>
        private void OnImageCanvasSizeChanged(object canvas)
        {
            foreach(ImageControlController obj in ImageControlControllerList)
            {
                obj.CanvasSizeChanged(canvas);
            }
        }

        /// <summary>
        /// This method delivers the call to ImageControlController on save image dialog box ok click.
        /// </summary>
        /// <param name="fileName">The filename with which user wants to save the image.</param>
        internal void SaveImageButtonClicked(string fileName)
        {
            ActiveImageControlController.SaveImage(fileName);
        }

        /// <summary>
        /// Applies all the filters at once on the Image UIElement.
        /// </summary>
        /// <param name="hueValue">The current slider value of Hue.</param>
        /// <param name="saturationFactor">The current slider value of saturation.</param>
        /// <param name="brightnessFactor">The current slider value of Brightness.</param>
        /// <param name="blurFactor">The current slider value of Blur.</param>
        /// <param name="luminanceFactor">The current slider value of Luminance.</param>
        /// <param name="_isPerwittEdgeDetectionChecked">The current state of Edge detection checkbox.</param>
        private void UpdateImageControlOnFilterApply(int hueValue, int saturationFactor, int brightnessFactor, int blurFactor, int luminanceFactor, int contrastFactor, bool _isPerwittEdgeDetectionChecked)
        {
            byte[] pixelArr = ActiveImageControlController.GetImageRGBPixels();
            ImageProcessingFilterWrapper wrapper = new ImageProcessingFilterWrapper();
            wrapper.ApplyFilters(pixelArr, hueValue, saturationFactor, brightnessFactor, blurFactor, luminanceFactor, contrastFactor, _isPerwittEdgeDetectionChecked, (int)ActiveImageControlController.ImageHeight, (int)ActiveImageControlController.ImageWidth);
            wrapper = null;
            ActiveImageControlController.UpdateImageSource(pixelArr);
            pixelArr = null;
        }

        /// <summary>
        /// Whenever Perwitt Edge Detection state is changed, this method will reset the settings and update image.
        /// </summary>
        private void UpdateImageControlOnPerwittEdgeDetectionStateChanged()
        {
            PclsMainWindowModel.SetFiltersToDefault();
            UpdateImageControlOnFilterApply(PclsMainWindowModel.HueValue, PclsMainWindowModel.SaturationFactor, PclsMainWindowModel.BrightnessValue, PclsMainWindowModel.BlurFactor, PclsMainWindowModel.LuminanceFactor, PclsMainWindowModel.ContrastFactor, PclsMainWindowModel.IsPerwittEdgeDetectionChecked);
        }

        /// <summary>
        /// Set the current image control as inactive.
        /// </summary>
        internal void DoOperationsBeforeSaveCanvas()
        {
            ActiveImageControlController.RemoveStyle();
            Panel.SetZIndex(ActiveImageControlController.GetView(), 1);
            foreach (var control in ImageControlControllerList)
                control.HideScrollViewers();
        }

        /// <summary>
        /// Sets the image control as active.
        /// </summary>
        internal void DoOperationsAfterSaveCanvas()
        {
            ActiveImageControlController.Notify("UpdateImageControlStyle", null);
            ActiveImageControlController.SetSelectedState(emSelectedState.Selected);
            foreach (var control in ImageControlControllerList)
                control.ResetScrollViewerVisibilityState();
        }
        #endregion

        #region Interfaces
        #region IMainWindow

        /// <summary>
        /// When image selection is changed, this method is called from ImageControlController.
        /// </summary>
        /// <param name="imagecontrol">The current Imagecontrol being selected.</param>
        public void OnImageSelected(INotifier imagecontrol)
        {
            if(imagecontrol.GetHashCode() != ActiveImageControlController.GetHashCode())
            {
                ActiveImageControlController.RemoveStyle();
                ActiveImageControlController.UnRegisterImageControlEvents();
                PclsMainWindowModel.ExportFilterStateToDataTable(ActiveImageControlController.GetHashCode());
                ActiveImageControlController = imagecontrol as ImageControlController;
                ActiveImageControlController.RegisterImageControlEvents();
                ActiveImageControlController.Notify("UpdateImageControlStyle", null);
                PclsMainWindowView.FileNameTextBox.Text = _dctImagePaths[ActiveImageControlController.GetHashCode()];
                PclsMainWindowModel.ImportFilterStateToModel(ActiveImageControlController.GetHashCode());
            }
        }

        #endregion
        #endregion
    }
}
