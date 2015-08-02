using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Xamtastic.Patterns.SmallestMvvm
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelTypeAttribute : Attribute
    {
        public ViewModelTypeAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; private set; }
    }
}
