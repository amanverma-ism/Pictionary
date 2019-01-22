using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pictionary.Capsules
{
    public class BaseModel : INotifyPropertyChanged
    {
        #region Variables

        private BaseController _clsParentController;
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();

        #endregion

        #region Constructor

        public BaseModel(BaseController _parent)
        {
            _clsParentController = _parent;
        }

        #endregion

        #region Properties

        protected BaseController ClsParentController
        {
            get { return _clsParentController; }
            set { _clsParentController = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _Handlers.Add(value);
            }
            remove
            {
                _Handlers.Remove(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// PropertyChanged handler to send call to all the subscribers.
        /// </summary>
        /// <param name="_strProperty">PropertyName to be included in PropertyChangedEventArgs</param>
        protected void OnPropertyChanged(string _strProperty)
        {
            if (_Handlers != null && _Handlers.Count != 0)
            {
                for (int i = 0; i < _Handlers.Count; i++)
                {
                    _Handlers[i].Invoke(this, new PropertyChangedEventArgs(_strProperty));
                }
            }
        }

        /// <summary>
        /// Resets the model values to default.
        /// </summary>
        public virtual void SetDefaultModelState() { }

        /// <summary>
        /// Called on application exit to clear all the references.
        /// </summary>
        public virtual void ClearData()
        {
            _clsParentController = null;
        }
        #endregion
    }
}
