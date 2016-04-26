using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Interfaces;
using OutputScreen.Annotations;
using Prism.Commands;

namespace OutputScreen.DiagnosticsView
{
    public class DiagnosticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
