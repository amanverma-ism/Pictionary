using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pictionary.Capsules
{
    public class MainWindowModel : BaseModel
    {
        #region Variables
        private int _i32BrightnessValue;
        private int _i32BlurFactor;
        private int _i32LuminanceFactor;
        private int _i32SaturationFactor;
        private int _i32HueValue;
        private int _i32ContrastFactor;
        private bool _blnIsPerwittEdgeDetectionChecked;
        private DataTable _dtFilterStates;
        private ColorBox _selectedBackgroundColor;
        #endregion

        #region Constructor

        public MainWindowModel(MainWindowController _parent): base(_parent)
        {
            InitializeFilterStatesTable();
        }

        #endregion

        #region Properties

        private MainWindowController PclsMainWindowController
        {
            get { return ClsParentController as MainWindowController; }
            set { ClsParentController = value; }
        }

        public int BrightnessValue
        {
            get { return _i32BrightnessValue; }
            set 
            { 
                _i32BrightnessValue = value;
                PclsMainWindowController.Notify("BrightnessValueChanged", null);
            }
        }

        public int BlurFactor
        {
            get { return _i32BlurFactor; }
            set
            {
                _i32BlurFactor = value;
                PclsMainWindowController.Notify("BlurFactorChanged", null);
            }
        }

        public int LuminanceFactor
        {
            get { return _i32LuminanceFactor; }
            set
            {
                _i32LuminanceFactor = value;
                PclsMainWindowController.Notify("LuminanceFactorChanged", null);
            }
        }

        public int ContrastFactor
        {
            get { return _i32ContrastFactor; }
            set
            {
                _i32ContrastFactor = value;
                PclsMainWindowController.Notify("ContrastFactorChanged", null);
            }
        }

        public int SaturationFactor
        {
            get { return _i32SaturationFactor; }
            set
            {
                _i32SaturationFactor = value;
                PclsMainWindowController.Notify("SaturationFactorChanged", null);
            }
        }

        public int HueValue
        {
            get { return _i32HueValue; }
            set
            {
                _i32HueValue = value;
                PclsMainWindowController.Notify("HueValueChanged", null);
            }
        }

        public bool IsImageOperationsPanelEnabled
        {
            get { return PclsMainWindowController.ImageControlControllerList.Count > 0 ? true : false; }
        }

        public bool IsPerwittEdgeDetectionChecked
        {
            get { return _blnIsPerwittEdgeDetectionChecked; }
            set
            {
                _blnIsPerwittEdgeDetectionChecked = value;
                OnPropertyChanged("IsPerwittEdgeDetectionChecked");
                PclsMainWindowController.Notify("PerwittEdgeDetectionStateChanged", _blnIsPerwittEdgeDetectionChecked);
            }
        }

        public bool IsDeleteButtonEnabled
        {
            get { return PclsMainWindowController.ImageControlControllerList.Count > 0 ? true : false; }
        }

        public ColorBox SelectedBackgroundColor
        {
            get { return _selectedBackgroundColor; }
            set 
            { 
                _selectedBackgroundColor = value;
                OnPropertyChanged("SelectedBackgroundColor"); 
                OnPropertyChanged("BackgroundColor");
                PclsMainWindowController.Notify("BackgroundColorChanged", null);
            }
        }

        #endregion

        #region Methods

        #region Overridden Methods

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public override void ClearData()
        {
            _dtFilterStates.Clear();
            base.ClearData();
        }

        /// <summary>
        /// Sets the model variables to its default.
        /// </summary>
        public override void SetDefaultModelState()
        {
            _i32BrightnessValue = 0;
            _i32BlurFactor = 0;
            _i32LuminanceFactor = 0;
            _i32SaturationFactor = 0;
            _i32HueValue = 0;
            _blnIsPerwittEdgeDetectionChecked = false;
            _i32ContrastFactor = 0;
            OnPropertyChanged("RefreshView");
        }

        #endregion

        /// <summary>
        /// Sets only filter values to default.
        /// </summary>
        internal void SetFiltersToDefault()
        {
            _i32BrightnessValue = 0;
            _i32BlurFactor = 0;
            _i32LuminanceFactor = 0;
            _i32SaturationFactor = 0;
            _i32HueValue = 0;
            _i32ContrastFactor = 0;
            OnPropertyChanged("RefreshView");
        }

        /// <summary>
        /// Called when user clicks on "Delete Selected Image" button.
        /// </summary>
        internal void OnDeleteSelectedImage()
        {
            PclsMainWindowController.Notify("DeleteSelectedImage", null);
        }

        /// <summary>
        /// Subscriber of the SizeChanged event of ImageCanvas UIElement.
        /// </summary>
        /// <param name="sender">ImageCanvas UIElement</param>
        /// <param name="e">SizeChangedEventArgument containing the previous size and the new size.</param>
        internal void OnImageCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            PclsMainWindowController.Notify("ImageCanvasSizeChanged", sender);
        }

        #region FilterStateMaintenance

        /// <summary>
        /// Initializes the filterStateTable with columns needed.
        /// </summary>
        private void InitializeFilterStatesTable()
        {
            _dtFilterStates = new DataTable("FilterStates");
            _dtFilterStates.Locale = CultureInfo.InvariantCulture;
            _dtFilterStates.Columns.Add("ImageControlID", typeof(int));
            _dtFilterStates.Columns.Add("BrightnessValue", typeof(int));
            _dtFilterStates.Columns.Add("BlurFactor", typeof(int));
            _dtFilterStates.Columns.Add("LuminanceFactor", typeof(int));
            _dtFilterStates.Columns.Add("SaturationFactor", typeof(int));
            _dtFilterStates.Columns.Add("HueValue", typeof(int));
            _dtFilterStates.Columns.Add("ContrastFactor", typeof(int));
            _dtFilterStates.Columns.Add("PerwittEdgeDetectionState", typeof(bool));
            _dtFilterStates.AcceptChanges();
        }

        /// <summary>
        /// Delete the state row from DataTable whose ImageControlID is equal to parameter passed.
        /// </summary>
        /// <param name="imagecontrolID">ImageControlID value for Row to delete</param>
        internal void DeleteFilterStateFromDataTable(int imagecontrolID)
        {
            DataRow[] drRows = _dtFilterStates.Select("ImageControlID" + "=" + imagecontrolID.ToString());
            drRows[0].Delete();
            _dtFilterStates.AcceptChanges();
        }

        /// <summary>
        /// Exports the state from model to DataTable in the row corresponding to the ID passed as parameter.
        /// </summary>
        /// <param name="imagecontrolID">ID for row in which we want to export the states.</param>
        internal void ExportFilterStateToDataTable(int imagecontrolID)
        {
            DataRow[] drRows = _dtFilterStates.Select("ImageControlID" + "=" + imagecontrolID.ToString());
            if(drRows.Length > 0)
            {
                drRows[0]["BrightnessValue"] = _i32BrightnessValue;
                drRows[0]["BlurFactor"] = _i32BlurFactor;
                drRows[0]["LuminanceFactor"] = _i32LuminanceFactor;
                drRows[0]["SaturationFactor"] = _i32SaturationFactor;
                drRows[0]["HueValue"] = _i32HueValue;
                drRows[0]["ContrastFactor"] = _i32ContrastFactor;
                drRows[0]["PerwittEdgeDetectionState"] = _blnIsPerwittEdgeDetectionChecked;
            }
            else
            {
                DataRow dr = _dtFilterStates.NewRow();
                dr["ImageControlID"] = imagecontrolID;
                dr["BrightnessValue"] = _i32BrightnessValue;
                dr["BlurFactor"] = _i32BlurFactor;
                dr["LuminanceFactor"] = _i32LuminanceFactor;
                dr["SaturationFactor"] = _i32SaturationFactor;
                dr["HueValue"] = _i32HueValue;
                dr["ContrastFactor"] = _i32ContrastFactor;
                dr["PerwittEdgeDetectionState"] = _blnIsPerwittEdgeDetectionChecked;
                _dtFilterStates.Rows.Add(dr);
                _dtFilterStates.AcceptChanges();
            }
        }

        /// <summary>
        /// Imports the state to model corresponding to the ID passed as parameter.
        /// </summary>
        /// <param name="imagecontrolID">Id for which we want to import the state</param>
        internal void ImportFilterStateToModel(int imagecontrolID)
        {
            DataRow[] drRows = _dtFilterStates.Select("ImageControlID" + "=" + imagecontrolID.ToString());
            _i32BrightnessValue = (int)drRows[0]["BrightnessValue"];
            _i32BlurFactor = (int)drRows[0]["BlurFactor"];
            _i32LuminanceFactor = (int)drRows[0]["LuminanceFactor"];
            _i32SaturationFactor = (int)drRows[0]["SaturationFactor"];
            _i32HueValue = (int)drRows[0]["HueValue"];
            _i32ContrastFactor = (int)drRows[0]["ContrastFactor"];
            _blnIsPerwittEdgeDetectionChecked = (bool)drRows[0]["PerwittEdgeDetectionState"];
            OnPropertyChanged("RefreshView");
        }

        #endregion

        #endregion


        internal void SetDefaultBackgroundColorBox(ColorBox colorBox)
        {
            _selectedBackgroundColor = colorBox;
            PclsMainWindowController.PclsMainWindowView.SelectedColorViewBox.Children.Clear();
            PclsMainWindowController.PclsMainWindowView.SelectedColorViewBox.Children.Add(new ColorBox(_selectedBackgroundColor.GetColor()));
        }
    }
}
