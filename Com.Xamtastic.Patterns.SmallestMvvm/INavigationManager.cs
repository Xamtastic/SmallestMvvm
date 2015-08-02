using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Xamtastic.Patterns.SmallestMvvm
{
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
}
