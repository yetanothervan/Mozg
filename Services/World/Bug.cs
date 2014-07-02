using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Practices.ServiceLocation;
using Action = Interfaces.Action;

namespace WorldService
{
    public class Bug : ICreature
    {
        private Dictionary<string, Leg> _legs;
        private const string LeftFront = "LeftFront";
        private const string RightFront = "RightFront";
        private const string LeftBack = "LeftBack";
        private const string RightBack = "RightBack";

        public const double ANGLE_MAX = 100.0;
        public const double ANGLE_MIN = 0 - ANGLE_MAX;
        public const double COM_MAX = 1000.0;
        public const double COM_MIN = 0.0;
        public const double EFF_MAX = 100.0;
        public const double EFF_MIN = 0 - EFF_MAX;
        public const double TOUCH_MAX = 1.0;
        public const double TOUCH_MIN = 0.0;
        public const double FOOD_ANGLE_MAX = Math.PI;
        public const double FOOD_ANGLE_MIN = 0.0;

        private ICns _cns;

        public double TireLevel;

        public Bug()
        {
            TireLevel = 1000;
            _legs = new Dictionary<string, Leg>
            {
                {LeftFront, new Leg(this, LeftFront, 0.0, 0.0)},
                {RightFront, new Leg(this, RightFront, 0.0, 0.0)},
                {LeftBack, new Leg(this, LeftBack, 0.0, 0.0)},
                {RightBack, new Leg(this, RightBack, 0.0, 0.0)}
            };

            var cnsService =
                (ICnsService)
                    ServiceLocator.Current.GetService(typeof(ICnsService));

            _cns = cnsService.CreateCnc();

            foreach (var leg in _legs)
            {
                _cns.AddSensor(leg.Value.GetHorizontalAngleSensorName(), ANGLE_MIN, ANGLE_MAX);
                _cns.AddSensor(leg.Value.GetVerticalAngleSensorName(), ANGLE_MIN, ANGLE_MAX);
                _cns.AddSensor(leg.Value.GetTouchSensorName(), TOUCH_MIN, TOUCH_MAX);
                _cns.AddEffector(leg.Value.GetFirstHorEffectorName(), EFF_MIN, EFF_MAX);
                _cns.AddEffector(leg.Value.GetSecondHorEffectorName(), EFF_MIN, EFF_MAX);
                _cns.AddEffector(leg.Value.GetFirstVerEffectorName(), EFF_MIN, EFF_MAX);
                _cns.AddEffector(leg.Value.GetSecondVerEffectorName(), EFF_MIN, EFF_MAX);
            }

            _cns.AddSensor("TireSens", COM_MIN, COM_MAX);
            _cns.AddTargetSensor("HungSens", COM_MIN, COM_MAX);
            _cns.AddSensor("FoodDistSens", COM_MIN, COM_MAX);
            _cns.AddSensor("FoodAngleSens", FOOD_ANGLE_MIN, FOOD_ANGLE_MAX);
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
            var leftCom = ApplyEffsForLeg(LeftFront);
            var rightCom = ApplyEffsForLeg(RightFront);
            leftCom += ApplyEffsForLeg(LeftBack);
            rightCom += ApplyEffsForLeg(RightBack);
            
            //tire level calculate 
            var tire = GetTired(LeftFront);
            tire += GetTired(RightFront);
            tire += GetTired(LeftBack);
            tire += GetTired(RightBack);
            
            if (TireLevel - tire < 0) TireLevel = 0;
            else TireLevel -= tire;
            
            var result = new List<Action>();
            var angle = rightCom - leftCom;
            if (Math.Abs(angle) > 0.01)
            {
                var rotate = new Action
                {
                    ActionType = ActionType.Rotate,
                    Value = Math.PI/(Leg.EffPart*Bug.ANGLE_MAX*4)*angle
                };
                result.Add(rotate);
            }

            var dist = rightCom + leftCom;
            if (Math.Abs(dist) > 0.01)
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

        private double GetTired(string legName)
        {
            var tire = 0.0;
            tire += Normalize(Math.Abs(_cns.GetEffector(
                _legs[legName].GetFirstHorEffectorName())), 0, EFF_MAX);
            tire += Normalize(Math.Abs(_cns.GetEffector(
                _legs[legName].GetSecondHorEffectorName())), 0, EFF_MAX);
            tire += Normalize(Math.Abs(_cns.GetEffector(
                _legs[legName].GetFirstVerEffectorName())), 0, EFF_MAX);
            tire += Normalize(Math.Abs(_cns.GetEffector(
                _legs[legName].GetSecondVerEffectorName())), 0, EFF_MAX);
            return tire;
        }

        private double ApplyEffsForLeg(string leg)
        {
            var lf = _legs[leg];
            var lfHor = _cns.GetEffector(lf.GetFirstHorEffectorName());
            lfHor += _cns.GetEffector(lf.GetSecondHorEffectorName());
            var res = lf.ApplyHorEffector(lfHor);
            
            var lfVer = _cns.GetEffector(lf.GetFirstVerEffectorName());
            lfVer += _cns.GetEffector(lf.GetSecondVerEffectorName());
            lf.ApplyVerEffector(lfVer);

            return res;
        }
        
        public void AdvantageMoment()
        {
            _cns.AdvantageMoment();
        }
    }

    public class Leg
    {
        private readonly Bug _owner;
        private readonly string _name;
        private double _horizontalAngle;
        private double _verticalAngle;

        private const double TirePart = 0.9;
        public const double EffPart = 0.1;

        public double ApplyHorEffector(double eff)
        {
            var tirePerc = _owner.TireLevel/(Bug.COM_MAX - Bug.COM_MIN);
            var result = eff * tirePerc * TirePart + eff * (1 - TirePart);
            
            result *= EffPart;
            var oldVal = _horizontalAngle;
            if (_horizontalAngle + result > Bug.ANGLE_MAX) _horizontalAngle = Bug.ANGLE_MAX;
            else if (_horizontalAngle + result < Bug.ANGLE_MIN) _horizontalAngle = Bug.ANGLE_MIN;
            else _horizontalAngle += result;

            return Math.Abs(_verticalAngle - Bug.ANGLE_MAX) < 0.01 //if leg touchs earth
                ? _horizontalAngle - oldVal
                : 0.0;
        }

        public void ApplyVerEffector(double eff)
        {
            var tirePerc = _owner.TireLevel / (Bug.COM_MAX - Bug.COM_MIN);
            var result = eff * tirePerc * TirePart + eff * (1 - TirePart);

            result *= EffPart;
            if (_verticalAngle + result > Bug.ANGLE_MAX) _verticalAngle = Bug.ANGLE_MAX;
            else if (_verticalAngle + result < Bug.ANGLE_MIN) _verticalAngle = Bug.ANGLE_MIN;
            else _verticalAngle += result;
        }
        
        public string GetHorizontalAngleSensorName()
        {
            return _name + "HorSens";
        }

        public string GetVerticalAngleSensorName()
        {
            return _name + "VerSens";
        }

        public string GetTouchSensorName()
        {
            return _name + "TouchSens";
        }

        public string GetFirstHorEffectorName()
        {
            return _name + "HorEffFirst";
        }

        public string GetSecondHorEffectorName()
        {
            return _name + "HorEffSecond";
        }

        public string GetFirstVerEffectorName()
        {
            return _name + "VerEffFirst";
        }

        public string GetSecondVerEffectorName()
        {
            return _name + "VerEffSecond";
        }

        public Leg(Bug owner, string name, double horizontalAngle, double verticalAngle)
        {
            _owner = owner;
            _name = name;
            _horizontalAngle = horizontalAngle;
            _verticalAngle = verticalAngle;
        }
    }
}
