using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Xamtastic.Patterns.SmallestMvvm
{
    public abstract class ViewModelBase<TNavigationParameter> : ViewModelBase
    {
        public new TNavigationParameter NavigationParameter
        {
            get { return (TNavigationParameter)base.NavigationParameter; }
        }
    }

    public class ViewModelBase : PropertyChangedBase, IDisposable
    {
        private INavigationManager _navigationManager;
        public INavigationManager NavigationManager
        {
            get { return _navigationManager; }
            set { _navigationManager = value; }
        }

        #region IDisposable members
        public void Dispose()
        {

        }
        #endregion

        public ViewModelBase()
        {

        }

        public virtual Task Initialize()
        {
            return Task.FromResult(true);
        }

        public virtual Task Initialize(object navigationParameter)
        {
            NavigationParameter = navigationParameter;
            return Initialize();
        }

        public object NavigationParameter { get; private set; }

        public bool IsInitialized { get; set; }

        public virtual void OnAppear()
        {

        }


    }
}
