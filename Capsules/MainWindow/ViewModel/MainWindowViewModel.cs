using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Pictionary.Capsules
{
    public class MainWindowViewModel : BaseViewModel, IColorBoxParent
    {
        #region Variables
        private ICommand _cmdDeleteSelectedImage;
        private List<ColorBox> _lstBackgroundColor;
        #endregion

        #region Constructor

        public MainWindowViewModel(MainWindowModel _Model) : base(_Model)
        {
            PclsMainWindowModel.PropertyChanged += MainWindowModel_PropertyChanged;
            _cmdDeleteSelectedImage = new BaseCommand(OnDeleteSelectedImage);
            _lstBackgroundColor = new List<ColorBox>();
            FillBackgroundColorBox();
        }

        #endregion

        #region Properties
        public MainWindowModel PclsMainWindowModel
        {
            get { return ClsModel as MainWindowModel; }
        }

        public int BrightnessValue
        {
            get { return PclsMainWindowModel.BrightnessValue; }
            set { PclsMainWindowModel.BrightnessValue = value; }
        }

        public int BlurFactor
        {
            get { return PclsMainWindowModel.BlurFactor; }
            set { PclsMainWindowModel.BlurFactor = value; }
        }

        public int ContrastFactor
        {
            get { return PclsMainWindowModel.ContrastFactor; }
            set { PclsMainWindowModel.ContrastFactor = value; }
        }

        public int LuminanceFactor
        {
            get { return PclsMainWindowModel.LuminanceFactor; }
            set { PclsMainWindowModel.LuminanceFactor = value; }
        }

        public int SaturationFactor
        {
            get { return PclsMainWindowModel.SaturationFactor; }
            set { PclsMainWindowModel.SaturationFactor = value; }
        }

        public int HueValue
        {
            get { return PclsMainWindowModel.HueValue; }
            set { PclsMainWindowModel.HueValue = value; }
        }

        public bool IsImageOperationsPanelEnabled
        {
            get { return PclsMainWindowModel.IsImageOperationsPanelEnabled; }
        }

        public bool IsPerwittEdgeDetectionChecked
        {
            get { return PclsMainWindowModel.IsPerwittEdgeDetectionChecked; }
            set { PclsMainWindowModel.IsPerwittEdgeDetectionChecked = value; }
        }

        public ICommand DeleteSelectedImage
        {
            get { return _cmdDeleteSelectedImage; }
        }

        public bool IsDeleteButtonEnabled
        {
            get { return PclsMainWindowModel.IsDeleteButtonEnabled; }
        }

        public List<ColorBox> BackgroundColorList
        {
            get { return _lstBackgroundColor; }
        }

        public ColorBox SelectedBackgroundColor
        {
            get { return PclsMainWindowModel.SelectedBackgroundColor; }
            set { PclsMainWindowModel.SelectedBackgroundColor = value; }
        }

        public Brush BackgroundColor
        {
            get { return PclsMainWindowModel.SelectedBackgroundColor.GetColor(); }
        }
        #endregion

        #region Interfaces

        #region IColorBoxParent
        public void OnColorBoxSelected(int hashcode)
        {
            SelectedBackgroundColor = _lstBackgroundColor.Find(x => x.GetHashCode() == hashcode);
        }
        #endregion

        #endregion

        #region Methods

        #region Overridden Methods

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public override void ClearData()
        {
            foreach(ColorBox item in _lstBackgroundColor)
            {
                item.ClearData();
            }
            _lstBackgroundColor.Clear();
            PclsMainWindowModel.PropertyChanged -= MainWindowModel_PropertyChanged;
            _cmdDeleteSelectedImage = null;
            base.ClearData();
        }

        #endregion

        /// <summary>
        /// Subscriber of the PropertyChanged event of MainWindowModel.
        /// </summary>
        /// <param name="sender">MainWindowModel</param>
        /// <param name="e">PropertyChangedEventArgument containg the PropertyName.</param>
        private void MainWindowModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsImageOperationsPanelEnabled":
                    OnPropertyChanged("IsImageOperationsPanelEnabled");
                    break;
                case "IsPerwittEdgeDetectionChecked":
                    OnPropertyChanged("IsPerwittEdgeDetectionChecked");
                    break;
                case "RefreshView":
                    OnPropertyChanged("BrightnessValue");
                    OnPropertyChanged("BlurFactor");
                    OnPropertyChanged("LuminanceFactor");
                    OnPropertyChanged("HueValue");
                    OnPropertyChanged("SaturationFactor");
                    OnPropertyChanged("ContrastFactor");
                    OnPropertyChanged("IsPerwittEdgeDetectionChecked");
                    OnPropertyChanged("IsImageOperationsPanelEnabled");
                    OnPropertyChanged("IsDeleteButtonEnabled");
                    break;
                case "BrightnessValue":
                    OnPropertyChanged("BrightnessValue");
                    break;
                case "BlurFactor":
                    OnPropertyChanged("BlurFactor");
                    break;
                case "LuminanceFactor":
                    OnPropertyChanged("LuminanceFactor");
                    break;
                case "ContrastFactor":
                    OnPropertyChanged("ContrastFactor");
                    break;
                case "HueValue":
                    OnPropertyChanged("HueValue");
                    break;
                case "SaturationFactor":
                    OnPropertyChanged("SaturationFactor");
                    break;
                case "IsDeleteButtonEnabled":
                    OnPropertyChanged("IsDeleteButtonEnabled");
                    break;
                case "SelectedBackgroundColor":
                    OnPropertyChanged("SelectedBackgroundColor");
                    break;
                case "BackgroundColor":
                    OnPropertyChanged("BackgroundColor");
                    break;
            }
        }

        /// <summary>
        /// Called when user clicks on "Delete Selected Image" button.
        /// </summary>
        public void OnDeleteSelectedImage()
        {
            PclsMainWindowModel.OnDeleteSelectedImage();
        }

        /// <summary>
        /// Subscriber of the SizeChanged event of ImageCanvas UIElement.
        /// </summary>
        /// <param name="sender">ImageCanvas UIElement</param>
        /// <param name="e">SizeChangedEventArgument containing the previous size and the new size.</param>
        internal void OnImageCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            PclsMainWindowModel.OnImageCanvasSizeChanged(sender, e);
        }

        /// <summary>
        /// Added for Controller to contact viewmodel when controller wants any view to be refreshed.
        /// </summary>
        /// <param name="str">The property for which OnPropertyChanged needs to be raised.</param>
        internal void OnPropertyChange(string str)
        {
            OnPropertyChanged(str);
        }

        private void FillBackgroundColorBox()
        {
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Black));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.White));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Blue));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Green));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Red));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Yellow));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Cyan));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Turquoise));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Teal));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.LightGray));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Pink));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.DeepPink));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Magenta));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.AliceBlue));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.BlanchedAlmond));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.LightGreen));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.DarkOrchid));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.DarkOrange));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.SteelBlue));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Violet));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Purple));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Thistle));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.Indigo));
            _lstBackgroundColor.Add(new ColorBox(this, Brushes.LimeGreen));
            PclsMainWindowModel.SetDefaultBackgroundColorBox(_lstBackgroundColor.First());
        }

        #endregion

    }
}
