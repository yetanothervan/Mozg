﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Interfaces;
using OutputScreen.Annotations;

namespace OutputScreen
{
    public class RegionViewModel : INotifyPropertyChanged
    {
        private readonly IWorldService _worldService;
        private double _width;
        private double _height;
        private Thickness _frontLeftMargin;
        private ILeg _leg;

        public RegionViewModel(IWorldService worldService)
        {
            _worldService = worldService;
            var bug = _worldService.GetFirstCreature() as IFourLeg;
            if (bug != null) _leg = bug.GetFrontLeftLeg();

            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Width" || args.PropertyName == "Height")
                {
                    if (_leg != null)
                    {
                        var horStep = Width/2/(_leg.MaxAngle - _leg.MinAngle);
                        var verStep = Height/2/(_leg.MaxAngle - _leg.MinAngle);
                        
                        FrontLeftMargin = new Thickness(horStep * (_leg.HorAngle - _leg.MinAngle), verStep * (_leg.VerAngle - _leg.MinAngle) , 0, 0);
                    }
                }
            };
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
