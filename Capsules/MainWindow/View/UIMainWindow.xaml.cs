using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Pictionary.Capsules
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UIMainWindow : Window, IBaseView
    {
        #region Variables
        private MainWindowController _clsMainOwner;
        #endregion

        #region Constructor

        public UIMainWindow(MainWindowController _mainOwner)
        {
            _clsMainOwner = _mainOwner;
            InitializeComponent();
            UIWindow.Closed += OnApplicationClose;
            BrowseButton.Click += BrowseButton_Click;
            SaveImageButton.Click += SaveImageButton_Click;
            ImageCanvas.SizeChanged += ImageCanvas_SizeChanged;
            SaveCanvasButton.Click += SaveCanvasButton_Click;
        }

        #endregion

        #region Properties

        public MainWindowController MainOwner
        {
            get { return _clsMainOwner; }
            set { _clsMainOwner = value; }
        }

        public MainWindowViewModel PclsMainWindowViewModel
        {
            get { return this.DataContext as MainWindowViewModel; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Subscriber of the Click event of the button "Browse".
        /// </summary>
        /// <param name="sender">Button UIElement</param>
        /// <param name="e">RoutedEventArgument</param>
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Images (.jpg)|*.jpg";
            dlg.RestoreDirectory = true;

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox

            if (result == true)
            {
                string filename = dlg.FileName;
                // Open document
                FileNameTextBox.Text = filename;
                MainOwner.OnImageLoad(filename);
            }


        }

        /// <summary>
        /// Subscriber of the Click event of the button "SaveImage".
        /// </summary>
        /// <param name="sender">Button UIElement</param>
        /// <param name="e">RoutedEventArgument</param>
        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Images (.jpg)|*.jpg";
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    MainOwner.SaveImageButtonClicked(dlg.FileName);
                }
                catch
                {
                    MessageBox.Show("Error in Saving file.", "Pictionary");
                }
            }
        }

        /// <summary>
        /// Subscriber of the click event of the button SaveCanvas.
        /// </summary>
        /// <param name="sender">Button UIElement</param>
        /// <param name="e">RoutedEventArgument</param>
        void SaveCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Images (.jpg)|*.jpg";
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    MainOwner.DoOperationsBeforeSaveCanvas();
                    ImageCanvas.UpdateLayout();

                    RenderTargetBitmap bmp = new RenderTargetBitmap((int)ImageCanvas.ActualWidth,
                        (int)ImageCanvas.ActualHeight, 96, 96, PixelFormats.Default);
                    bmp.Render(ImageCanvas);
                    var encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using (var stream = File.Create(dlg.FileName))
                    {
                        encoder.Save(stream);
                    }
                }
                catch
                {
                    MessageBox.Show("Error in Saving file.", "Pictionary");
                }
                finally
                {
                    MainOwner.DoOperationsAfterSaveCanvas();
                }
            }
        }


        /// <summary>
        /// Subscriber of the SizeChanged event of ImageCanvas UIElement.
        /// </summary>
        /// <param name="sender">ImageCanvas UIElement</param>
        /// <param name="e">SizeChangedEventArgument containing the previous size and the new size.</param>
        private void ImageCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PclsMainWindowViewModel.OnImageCanvasSizeChanged(sender, e);
        }

        /// <summary>
        /// Subscriber of the Close event of MainWindow.
        /// </summary>
        /// <param name="sender">MainWindowUIElement.</param>
        /// <param name="e">EventArgument</param>
        private void OnApplicationClose(object sender, EventArgs e)
        {
            MainOwner.ClearData();
        }

        #endregion

        #region IBaseView

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public void ClearData()
        {
            UIWindow.Closed -= OnApplicationClose;
            BrowseButton.Click -= BrowseButton_Click;
            ImageCanvas.SizeChanged -= ImageCanvas_SizeChanged;
            SaveCanvasButton.Click -= SaveCanvasButton_Click;
            SaveImageButton.Click -= SaveImageButton_Click;

            this.DataContext = null;
            _clsMainOwner = null;
        }

        #endregion
    }
}
