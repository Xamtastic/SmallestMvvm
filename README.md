# SmallestMvvm for Xamarin Forms API

A 'Hello SmallestMvvm' demonstration of SmallestMvvm for Xamarin Forms is available at https://github.com/Xamtastic/SmallestMvvmDemo

The Nuget Package is at the following link:
https://www.nuget.org/packages/Com.Xamtastic.Patterns.SmallestMvvm

The Nuget Package Manager command is:
```
PM> Install-Package Com.Xamtastic.Patterns.SmallestMvvm
```

The Nuget package currently has a dependency on Xamarin.Forms 1.4.4.6392 however if you want to alter this, just download the code, alter it yourself, and recompile.

ViewModel injection is achieved via a page class decoration, where the page implements PageBase, which itself implements ContentPage:

```
    [ViewModelType(typeof(MainPageViewModel))]
    public partial class MainPage : PageBase
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }
```

Changing a page's 'ContentPage' declaration to 'PageBase' means that the Xaml must reflect the same change, as follows:

```
<?xml version="1.0" encoding="utf-8" ?>
<mvvm:PageBase xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:mvvm="clr-namespace:Com.Xamtastic.Patterns.SmallestMvvm;assembly=Com.Xamtastic.Patterns.SmallestMvvm"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Com.Xamtastic.SmallestMvvmDemo.Pages.MainPage">
  <Label Text="{Binding Message}" VerticalOptions="Center" HorizontalOptions="Center" />
</mvvm:PageBase>
```

The page's binding to a View Model is as per usual:

```
    public class MainPageViewModel : ViewModelBase
    {
        private string _message = "Hello SmallestMvvm!";
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged("Message"); }
        }
    }
```

The View Model injection into the page isn't magical, the ViewModelType attribute is declared in the Nuget package as follows:

```
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelTypeAttribute : Attribute
    {
        public ViewModelTypeAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; private set; }
    }
```

This allows it to be detected, instantiated, and bound, in the PageBase, which in the Nuget package is declared as follows:

```
    public class PageBase : ContentPage
    {
        public static PageBase CurrentPage { get; set; }

        protected PageBase()
        {
            CurrentPage = this;

            #region ViewModel injection
            var viewModelTypeAttribute = GetType().GetTypeInfo().GetCustomAttribute<ViewModelTypeAttribute>();

            if (viewModelTypeAttribute == null) throw new Exception(string.Format("Xamtastic Exception: ViewModelTypeAttribute class decoration missing in {0}", this.GetType().ToString()));

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
```

Veterans will realise that for an absolutely minimal solution, the View Model could have implemented the PropertyChangedBase, however for scalability the ViewModelBase caters for Page Navigation Parameters, hence a ViewModelBase is utilised to allow for an INavigationManager to be injected, cf PageBase and ViewModelBase, however it is intended that an Implementation of INavigationManager will be made available in another Nuget package so that this remains the smallest but scalable MVVM framework.

However the ViewModelBase is implemented in the Nuget package as follows (two classes):

```
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
```

To support Scalability, the INavigationManager interface is declared as follows:
```
    public interface INavigationManager
    {
        Task<bool> ShowMessageBox(string title, string message, string accept, string cancel = null);
        Task<string> ShowActionSheet(string title, string cancel, string[] options, string destroy = null);

        Task Show<TPage>(object navigationParameter = null, bool shouldNavigationBarVisible = true) where TPage : PageBase;
        Task ShowDialog<TPage>(object navigationParameter = null) where TPage : PageBase;
        Task Close(object navigationParameter = null);
        Task CloseDialog(object navigationParameter = null);
        Task Home(object navigationParameter = null);
        event EventHandler Navigating;
    }
```

The code for the demo is at the following link:
https://github.com/Xamtastic/SmallestMvvmDemo
