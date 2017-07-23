using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Interfaces;
using OutputScreen.Annotations;
using Prism.Commands;

namespace OutputScreen.OutputView
{
    public class OutputViewModel : INotifyPropertyChanged
    {
        private readonly IWorldService _worldService;
        private double _width;
        private double _height;
        private Thickness _frontLeftMargin;
        private List<ILeg> _legs;
        private string _frontLeftHorAngle;
        private Thickness _frontRightMargin;
        private Thickness _backLeftMargin;
        private Thickness _backRightMargin;

        public OutputViewModel(IWorldService worldService)
        {
            _worldService = worldService;
            var bug = _worldService.GetFirstCreature() as IFourLeg;

            _legs = new List<ILeg>();

            if (bug != null)
            {
                _legs.Add(bug.GetFrontLeftLeg());
                _legs.Add(bug.GetFrontRightLeg());
                _legs.Add(bug.GetBackLeftLeg());
                _legs.Add(bug.GetBackRightLeg());
            }

            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Width" || args.PropertyName == "Height")
                {
                    UpdateRegion();
                }
            };

            var doStepCommand = new DelegateCommand(UpdateRegion);
            ApplicationCommands.DoStepCommand.RegisterCommand(doStepCommand);

        }

        private void UpdateRegion()
        {
            if (_legs != null)
            {
                FrontLeftHorAngle = _legs[0].HorAngle.ToString("F3");

                FrontLeftMargin = CalcThick(_legs[0]);
                FrontRightMargin = CalcThick(_legs[1]);
                BackLeftMargin = CalcThick(_legs[2]);
                BackRightMargin = CalcThick(_legs[3]);
            }
        }

        private Thickness CalcThick(ILeg leg)
        {
            var horStep = Width / 2 / (leg.MaxAngle - leg.MinAngle);
            var verStep = Height / 2 / (leg.MaxAngle - leg.MinAngle);
            var marg = new Thickness(horStep * (leg.HorAngle - leg.MinAngle),
                verStep * (leg.VerAngle - leg.MinAngle),
                0, 0);
            return marg;
        }

        public string FrontLeftHorAngle
        {
            get { return _frontLeftHorAngle; }
            set
            {
                if (value == _frontLeftHorAngle) return;
                _frontLeftHorAngle = value;
                OnPropertyChanged();
            }
        }

        public Thickness FrontLeftMargin
        {
            get { return _frontLeftMargin; }
            set
            {
                if (value == _frontLeftMargin) return;
                _frontLeftMargin = value;
                OnPropertyChanged();
            }
        }

        public Thickness FrontRightMargin
        {
            get { return _frontRightMargin; }
            set
            {
                if (value.Equals(_frontRightMargin)) return;
                _frontRightMargin = value;
                OnPropertyChanged();
            }
        }

        public Thickness BackLeftMargin
        {
            get { return _backLeftMargin; }
            set
            {
                if (value.Equals(_backLeftMargin)) return;
                _backLeftMargin = value;
                OnPropertyChanged();
            }
        }

        public Thickness BackRightMargin
        {
            get { return _backRightMargin; }
            set
            {
                if (value.Equals(_backRightMargin)) return;
                _backRightMargin = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (Math.Abs(value - _width) < 0.1) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (Math.Abs(value - _height) < 0.1) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
   
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
