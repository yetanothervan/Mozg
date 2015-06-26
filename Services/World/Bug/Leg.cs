using System;
using Interfaces;

namespace WorldService.Bug
{
    public class Leg : ILeg
    {
        public const double AngleMax = 100.0;
        public const double AngleMin = 0 - AngleMax;
        public const double EffMax = 100.0;
        public const double EffMin = 0 - EffMax;
        public const double TouchMax = 1.0;
        public const double TouchMin = 0.0;

        public BugSensor HorAngleSensor { get; private set; }
        public BugSensor VerAngleSensor { get; private set; }
        public BugSensor TouchSensor  { get; private set; }

        public LegEffector Hor1Effector  { get; private set; }
        public LegEffector Hor2Effector  { get; private set; }
        public LegEffector Ver1Effector  { get; private set; }
        public LegEffector Ver2Effector { get; private set; }

        private readonly Bug _owner;
        private readonly string _name;
        
        private const double TireAffectsPart = 0.9;
        private const double TireNonAffectPart = 1 - TireAffectsPart;
        public const double EffectorScaleEffect = 0.1; 

        public void ApplyEffectors()
        {
            double horizontalEff = Hor1Effector.NextValue + Hor2Effector.NextValue;
            double verticalEff = Ver1Effector.NextValue + Ver2Effector.NextValue;
            
            var tirePerc = _owner.TireLevel / (Bug.ComMax - Bug.ComMin);
            
            var horResult = horizontalEff * tirePerc * TireAffectsPart + horizontalEff * TireNonAffectPart;
            horResult *= EffectorScaleEffect;

            var verResult = verticalEff * tirePerc * TireAffectsPart + verticalEff * TireNonAffectPart;
            verResult *= EffectorScaleEffect;

            double newHorAngle = HorAngleSensor.Value + horResult;
            if (newHorAngle > HorAngleSensor.MaxValue) newHorAngle = HorAngleSensor.MaxValue;
            if (newHorAngle < HorAngleSensor.MinValue) newHorAngle = HorAngleSensor.MinValue;

            double newVerAngle = VerAngleSensor.Value + verResult;
            if (newVerAngle > VerAngleSensor.MaxValue) newVerAngle = VerAngleSensor.MaxValue;
            if (newVerAngle < VerAngleSensor.MinValue) newVerAngle = VerAngleSensor.MinValue;


            MovedThisStep = Math.Abs(VerAngleSensor.Value - VerAngleSensor.MinValue) < 0.01 //if leg touches earth
                ? newHorAngle - HorAngleSensor.Value
                : 0.0;

            HorAngleSensor.Value = newHorAngle;
            VerAngleSensor.Value = newVerAngle;

            Hor1Effector.Value = Hor1Effector.NextValue;
            Hor2Effector.Value = Hor2Effector.NextValue;
            Ver1Effector.Value = Ver1Effector.NextValue;
            Ver2Effector.Value = Ver2Effector.NextValue;
        }

        public double MovedThisStep { get; private set; }
      
        public double MinAngle { get { return AngleMin ; } }
        public double MaxAngle { get { return AngleMax; } }
        public string Name { get { return _name; } }

        public double HorAngle
        {
            get { return HorAngleSensor.Value; }
        }

        public double VerAngle
        {
            get { return VerAngleSensor.Value; }
        }

        public Leg(Bug owner, string name)
        {
            _owner = owner;
            _name = name;
            HorAngleSensor = new BugSensor(_name + "HorSensor", AngleMin, AngleMax);
            VerAngleSensor = new BugSensor(_name + "VerSensor", AngleMin, AngleMax);
            TouchSensor = new BugSensor(_name + "TouchSensor", TouchMin, TouchMax);
            Hor1Effector = new LegEffector(_name + "Hor1Eff", EffMin, EffMax);
            Hor2Effector = new LegEffector(_name + "Hor2Eff", EffMin, EffMax);
            Ver1Effector = new LegEffector(_name + "Ver1Eff", EffMin, EffMax);
            Ver2Effector = new LegEffector(_name + "Ver2Eff", EffMin, EffMax);

        }
    }
}