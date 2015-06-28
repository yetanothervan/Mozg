using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Action = Interfaces.Action;

namespace WorldService.Bug
{
    public class Bug : ICreature, IFourLeg
    {
        private readonly string _name;
        private readonly WorldService _worldService;
        private readonly Dictionary<string, Leg> _legs;
        private const string LeftFront = "LeftFront";
        private const string RightFront = "RightFront";
        private const string LeftBack = "LeftBack";
        private const string RightBack = "RightBack";

       
        public const double ComMax = 1000.0;
        public const double ComMin = 0.0;
        public const double FoodAngleMax = Math.PI;
        public const double FoodAngleMin = -Math.PI;
        
        private readonly ICns _cns;
        
        private readonly BugSensor _tireSensor;
        private readonly BugSensor _hungSensor;
        private readonly BugSensor _foodDistSensor;
        private readonly BugSensor _foodAngleSensor;
        
        public Bug(string name, WorldService worldService, ICnsService cnsService)
        {
            _name = name;
            _worldService = worldService;
            _legs = new Dictionary<string, Leg>
            {
                {LeftFront, new Leg(this, LeftFront)},
                {RightFront, new Leg(this, RightFront)},
                {LeftBack, new Leg(this, LeftBack)},
                {RightBack, new Leg(this, RightBack)}
            };
            
            _cns = cnsService.CreateCnc();

            foreach (var leg in _legs)
            {
                _cns.AddSensor(leg.Value.HorAngleSensor);
                _cns.AddSensor(leg.Value.VerAngleSensor);
                _cns.AddSensor(leg.Value.TouchSensor);
                _cns.AddEffector(leg.Value.Hor1Effector);
                _cns.AddEffector(leg.Value.Hor2Effector);
                _cns.AddEffector(leg.Value.Ver1Effector);
                _cns.AddEffector(leg.Value.Ver2Effector);
            }

            _tireSensor = new BugSensor("TireSensor", ComMin, ComMax) {Value = ComMax};
            _hungSensor = new BugSensor("HungSensor", ComMin, ComMax) { Value = ComMax };
            _foodDistSensor = new BugSensor("FoodDistSens", ComMin, ComMax);
            _foodAngleSensor = new BugSensor("HungSensor", FoodAngleMin, FoodAngleMax);

            _cns.AddSensor(_tireSensor);
            _cns.AddTargetSensor(_hungSensor);
            _cns.AddSensor(_foodDistSensor);
            _cns.AddSensor(_foodAngleSensor);
        }

        //for nonlinear tire level
        public static double Normalize(double val, double minVal, double maxVal)
        {
            // =(-1600/(A101-125)-12,8)/51,2
            double onePercent = (maxVal - minVal) / 100.0;
            double percents = (val - minVal) / onePercent;
            return (val - minVal) * (-1600.0 / (percents - 125.0) - 12.8) / 51.2 + minVal;
        }

        public List<Action> GetActions()
        {
            UpdateSensorsFromWorld();
            _cns.SetEffectors();
            _cns.DoPrediction();

            foreach (var leg in _legs)
                leg.Value.ApplyEffectors();

            var leftCom = _legs[LeftFront].MovedThisStep + _legs[LeftBack].MovedThisStep;
            var rightCom = _legs[RightFront].MovedThisStep + _legs[RightBack].MovedThisStep;
            
            //tire level calculate 
            var tire = _legs.Sum(leg => GetTired(leg.Value));
            _tireSensor.Value -= tire;
            
            var result = new List<Action>();
            var angle = rightCom - leftCom;
            if (Math.Abs(angle) > 0)
            {
                var rotate = new Action
                {
                    ActionType = ActionType.Rotate,
                    Value = Math.PI/(Leg.EffectorScaleEffect*Leg.AngleMax*4)*angle
                };
                result.Add(rotate);
            }

            var dist = rightCom + leftCom;
            if (Math.Abs(dist) > 0)
            {
                var move = new Action
                {
                    ActionType = ActionType.Move,
                    Value = dist
                };
                result.Add(move);
            }

            return result;
        }

        public string Name
        {
            get { return _name; }
        }

        public double TireLevel { get { return _tireSensor.Value; } }

        private static double GetTired(Leg leg)
        {
            var tire = 0.0;
            tire += Normalize(Math.Abs(leg.Hor1Effector.Value), 0.0, leg.Hor1Effector.MaxValue);
            tire += Normalize(Math.Abs(leg.Hor2Effector.Value), 0.0, leg.Hor2Effector.MaxValue);
            tire += Normalize(Math.Abs(leg.Ver1Effector.Value), 0.0, leg.Ver1Effector.MaxValue);
            tire += Normalize(Math.Abs(leg.Ver2Effector.Value), 0.0, leg.Ver2Effector.MaxValue);
            return tire;
        }

        public void AdvantageMoment()
        {
            _cns.AdvantageMoment();
        }

        private void UpdateSensorsFromWorld()
        {
            if (_worldService.IsCreatureOnFood(Name))
            {
                _worldService.EatFoodUnderMe(Name);
                _hungSensor.Value = _hungSensor.MaxValue;
            }

            _foodAngleSensor.Value = _worldService.GetNearestFoodAngle(Name);
            _foodDistSensor.Value = _worldService.GetNearestFoodDist(Name);
        }

        public ILeg GetFrontLeftLeg()
        {
            return _legs[LeftFront];
        }
    }
}
