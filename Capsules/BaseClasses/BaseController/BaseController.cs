using Pictionary.Capsules.UtilityFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pictionary.Capsules
{
    public class BaseController : INotifier
    {
        #region Variables

        private UIElement _clsUIObject;
        private BaseModel _clsModel;
        private BaseViewModel _clsViewModel;

        #endregion

        #region Properties

        protected UIElement ClsUIObject
        {
            get { return _clsUIObject; }
            set { _clsUIObject = value; }
        }

        protected BaseModel ClsModel
        {
            get { return _clsModel; }
            set { _clsModel = value; }
        }

        protected BaseViewModel ClsViewModel
        {
            get { return _clsViewModel; }
            set { _clsViewModel = value; }
        }

        #endregion

        #region Constructor
        
        public BaseController()
        {
        }

        #endregion

        #region Interfaces

        #region INotifier
        public virtual void Notify(string notification, object args)
        {
            
        }
        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Method to access the xaml element of the object.
        /// </summary>
        /// <returns>The view(xaml) of the Capsule.</returns>
        public UIElement GetView()
        {
            return _clsUIObject;
        }

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public virtual void ClearData()
        {
            _clsModel = null;
            _clsViewModel = null;
        }

        #endregion

       
    }
}
