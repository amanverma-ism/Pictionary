using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pictionary.Capsules
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Variables

        private BaseModel _clsModel;
        private Collection<PropertyChangedEventHandler> _Handlers;

        #endregion

        #region Constructor

        public BaseViewModel(BaseModel _Model)
        {
            _clsModel = _Model;
            _Handlers = new Collection<PropertyChangedEventHandler>();
        }

        #endregion

        #region Properties

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

        public BaseModel ClsModel
        {
            get
            {
                return _clsModel;
            }
            set
            {
                _clsModel = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handler to send call to all the subscriber of PropertyChanged event.
        /// </summary>
        /// <param name="_strProperty"></param>
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
        /// Called on application exit to clear all the references.
        /// </summary>
        public virtual void ClearData()
        {
            _clsModel = null;
            _Handlers.Clear();
        }
        #endregion
    }
}
