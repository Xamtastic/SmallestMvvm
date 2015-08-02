using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;

namespace Com.Xamtastic.Patterns.SmallestMvvm
{
    public class PageBase : ContentPage
    {
        public static PageBase CurrentPage { get; set; }

        protected PageBase()
        {
            CurrentPage = this;

            #region ViewModel injection
            var viewModelTypeAttribute = GetType().GetTypeInfo().GetCustomAttribute<ViewModelTypeAttribute>();

            if (viewModelTypeAttribute == null) throw new Exception(string.Format("Xamtastic Exception: ViewModelTypeAttribute class decoration missing in {0}, please decorate your Content Page class with [ViewModelType(typeof(YourViewModel))]", this.GetType().ToString()));

            var viewModel = Activator.CreateInstance(viewModelTypeAttribute.ViewModelType);

            BindingContext = viewModel;
            #endregion
        }

        protected object NavigationParameter { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Initialize();

            if (!ViewModel.IsInitialized)
            {
                ViewModel.Initialize(NavigationParameter).ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {

                    }

                    ViewModel.IsInitialized = true;
                });
            }
        }

        public ViewModelBase ViewModel
        {
            get { return (ViewModelBase)BindingContext; }
        }

        protected virtual void Initialize()
        {


        }
    }
}
