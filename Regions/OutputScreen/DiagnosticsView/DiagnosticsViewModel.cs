using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Practices.Prism.Commands;
using OutputScreen.Annotations;

namespace OutputScreen.DiagnosticsView
{
    public class DiagnosticsViewModel : INotifyPropertyChanged
    {
        private readonly IWorldService _worldService;
        private string _flHorEffVal;
        private string _flHorAngleVal;
        public event PropertyChangedEventHandler PropertyChanged;
        private DelegateCommand DoStepCommand;

        public DiagnosticsViewModel(IWorldService worldService)
        {
            _worldService = worldService;
            DoStepCommand = new DelegateCommand(() =>
            {
                var fourLeg = _worldService.GetFirstCreature() as IFourLeg;
                if (fourLeg != null)
                {
                    var leg = fourLeg.GetFrontLeftLeg();
                    FlHorAngleVal = leg.HorAngle.ToString("F3");
                }
            });
            ApplicationCommands.DoStepCommand.RegisterCommand(DoStepCommand);
        }

        public string FlHorEffVal
        {
            get { return _flHorEffVal; }
            set
            {
                if (value == _flHorEffVal) return;
                _flHorEffVal = value;
                OnPropertyChanged();
            }
        }

        public string FlHorAngleVal
        {
            get { return _flHorAngleVal; }
            set
            {
                if (value == _flHorAngleVal) return;
                _flHorAngleVal = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
