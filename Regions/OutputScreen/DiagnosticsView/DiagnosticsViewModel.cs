using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Interfaces;
using Microsoft.Practices.Prism.Commands;
using OutputScreen.Annotations;

namespace OutputScreen.DiagnosticsView
{
    public class DiagnosticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ICnsDiagnostics _diagnostics;

        public DiagnosticsViewModel(IWorldService worldService)
        {
            _diagnostics = worldService.GetFirstCreature().CnsDiagnostics;

            var doStepCommand = new DelegateCommand(UpdateDiagnostics);
            ApplicationCommands.DoStepCommand.RegisterCommand(doStepCommand);

            UpdateDiagnostics();
        }

        private void UpdateDiagnostics()
        {
            var asd = _diagnostics.CellMemory.GetSensors();
            var lfhs = _diagnostics.CellMemory.GetSensors().First(s => s.Name == "LeftFrontHorSensor");
            var svalues = _diagnostics.CellMemory.GetSensorValues(lfhs, 2);
            switch (svalues.Count)
            {
                case 0:
                    SensorCurrent = String.Format("SensorCurrent: {0}", "NULL");
                    SensorPrevious = String.Format("SensorPrevious: {0}", "NULL");
                    break;
                case 1:
                    SensorCurrent = String.Format("SensorCurrent: {0}", svalues[0].ToString("F3"));
                    SensorPrevious = String.Format("SensorPrevious: {0}", "NULL");
                    break;
                default:
                    SensorCurrent = String.Format("SensorCurrent: {0}", svalues[0].ToString("F3"));
                    SensorPrevious = String.Format("SensorPrevious: {0}", svalues[1].ToString("F3"));
                    break;
            }

            var pred = _diagnostics.GetPredictedValueForSensor(lfhs.Id);
            SensorPredicted = pred != null
                ? String.Format("SensorPredicted: {0}", pred.Value.ToString("F3"))
                : "SensorPredicted: NULL";

            var lfh1E = _diagnostics.CellMemory.GetEffectors().First(s => s.Name == "LeftFrontHor1Eff");

            var evalues = _diagnostics.CellMemory.GetEffectorValues(lfh1E, 2);
            switch (evalues.Count)
            {
                case 0:
                    EffectorCurrent = String.Format("EffectorCurrent: {0}", "NULL");
                    EffectorPrevious = String.Format("EffectorPrevious: {0}", "NULL");
                    break;
                case 1:
                    EffectorCurrent = String.Format("EffectorPrevious: {0}", evalues[0].ToString("F3"));
                    EffectorPrevious = String.Format("EffectorPrevious: {0}", "NULL");
                    break;
                default:
                    EffectorCurrent = String.Format("EffectorCurrent: {0}", evalues[0].ToString("F3"));
                    EffectorPrevious = String.Format("EffectorPrevious: {0}", evalues[1].ToString("F3"));
                    break;
            }

            if (_diagnostics.CnsState.EffectorsNextValues.ContainsKey(lfh1E.Id))
                EffectorNext = String.Format("EffectorNext: {0}",
                    _diagnostics.CnsState.EffectorsNextValues[lfh1E.Id].ToString("F3"));
            else
                EffectorNext = String.Format("EffectorNext: {0}", "NULL");

            OnPropertyChanged("EffectorCurrent");
            OnPropertyChanged("EffectorNext");
            OnPropertyChanged("EffectorPrevious");
            OnPropertyChanged("SensorCurrent");
            OnPropertyChanged("SensorPrevious");
            OnPropertyChanged("SensorPredicted");
        }

        public string EffectorPrevious { get; set; }
        public string EffectorCurrent { get; set; }
        public string EffectorNext { get; set; }
        public string SensorPrevious { get; set; }
        public string SensorCurrent { get; set; }
        public string SensorPredicted { get; set; }
        

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
