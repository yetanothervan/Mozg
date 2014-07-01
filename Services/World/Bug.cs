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
        public const double ANGLE_MIN = -100.0;
        public const double COM_MAX = 1000.0;
        public const double COM_MIN = 0.0;
        public const double EFF_MAX = 100.0;
        public const double EFF_MIN = 0.0;
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

            _cns.UpdateSensors();
        }

        public List<Action> GetActions()
        {
            ApplyEffsForLeg(LeftFront);
            ApplyEffsForLeg(RightFront);
            ApplyEffsForLeg(LeftBack);
            ApplyEffsForLeg(RightBack);

            _cns.AddSensor(leg.Value.GetTouchSensorName(), 0.0, 1.0);
            _cns.AddEffector(leg.Value.GetFirstHorEffectorName(), 0.0, 100.0);
            _cns.AddEffector(leg.Value.GetSecondHorEffectorName(), 0.0, 100.0);
            _cns.AddEffector(leg.Value.GetFirstVerEffectorName(), 0.0, 100.0);
            _cns.AddEffector(leg.Value.GetSecondVerEffectorName(), 0.0, 100.0);
            
            return null;
        }

        private void ApplyEffsForLeg(string leg)
        {
            var lf = _legs[leg];
            var lfHor = _cns.GetEffector(lf.GetFirstHorEffectorName());
            lfHor += _cns.GetEffector(lf.GetSecondHorEffectorName());
            lf.ApplyHorEffector(lfHor);

            var lfVer = _cns.GetEffector(lf.GetFirstVerEffectorName());
            lfVer += _cns.GetEffector(lf.GetSecondVerEffectorName());
            lf.ApplyVerEffector(lfVer);
        }

        //РАСЧЕТ
   /*   double leftComponent = Effs[(int)s0eff.l1].ValueModal + Effs[(int)s0eff.l2].ValueModal;
      double rightComponent = Effs[(int)s0eff.r1].ValueModal + Effs[(int)s0eff.r2].ValueModal;
      double upComponent = Effs[(int)s0eff.u1].ValueModal + Effs[(int)s0eff.u2].ValueModal;
      double downComponent = Effs[(int)s0eff.d1].ValueModal + Effs[(int)s0eff.d2].ValueModal;

      const double koeff = 0.005;
      double curTire = Sens[(int)s0sens.tire].ValueModal / BoundValue.MaxValueModal;

      double leftAdd = koeff * leftComponent * (0.9 * curTire + 0.1); //90% зависит от tire
      double rightAdd = koeff * rightComponent * (0.9 * curTire + 0.1);
      double upAdd = koeff * upComponent * (0.9 * curTire + 0.1);
      double downAdd = koeff * downComponent * (0.9 * curTire + 0.1);

      //min = 0 max = 1;
      double newTire = (NormalizeModal(Effs[(int)s0eff.l1]) + NormalizeModal(Effs[(int)s0eff.l2]) +
        NormalizeModal(Effs[(int)s0eff.r1]) + NormalizeModal(Effs[(int)s0eff.r2]) +
        NormalizeModal(Effs[(int)s0eff.u1]) + NormalizeModal(Effs[(int)s0eff.u2]) +
        NormalizeModal(Effs[(int)s0eff.d1]) + NormalizeModal(Effs[(int)s0eff.d2])) / 8;

      newTire /= 1000;  //1000 моментов времени с максимальным напряжением

      //ОБНОВЛЕНИЕ      
      if (Sens[(int)s0sens.tire].ValueModal - newTire >= 0)
        Sens[(int)s0sens.tire].ValueModal -= newTire;
      else Sens[(int)s0sens.tire].ValueModal = 0;

      if (leftAdd >= rightAdd)
        UpdateSensor(leftAdd, rightAdd, ref Sens[(int)s0sens.l], ref Sens[(int)s0sens.r]);      
      else
        UpdateSensor(rightAdd, leftAdd, ref Sens[(int)s0sens.r], ref Sens[(int)s0sens.l]);

      if (upAdd >= downAdd)
        UpdateSensor(upAdd, downAdd, ref Sens[(int)s0sens.u], ref Sens[(int)s0sens.d]);
      else
        UpdateSensor(downAdd, upAdd, ref Sens[(int)s0sens.d], ref Sens[(int)s0sens.u]);

      if (Sens[(int)s0sens.d].Value == BoundValue.MaxValue)
        Sens[(int)s0sens.touch].Value = BoundValue.MaxValue;
      else
        Sens[(int)s0sens.touch].Value = BoundValue.MinValue;
    }

    void UpdateSensor(double a, double b, ref BoundValue sa, ref BoundValue sb)
    {
      a -= b;
      if (a > 0)
      {
        if (sa.Value + a > BoundValue.MaxValue)
          sa.Value = BoundValue.MaxValue;
        else
          sa.Value += a;
        sb.ValueModal = BoundValue.MaxValueModal - sa.ValueModal;
      }
    }

    public static double NormalizeModal(BoundValue val)
    {
      // =(-1600/(A101-125)-12,8)/51,2
      const double onePercent = (BoundValue.MaxValue - BoundValue.MinValue)/100.0;
      double percents = val.ValueModal / onePercent;
      return val.ValueModal * (-1600.0 / (percents - 125.0) - 12.8) / 51.2;      
    }
  }*/

        public void AdvantageMoment()
        {
            throw new NotImplementedException();
        }
    }

    public class Leg
    {
        private readonly Bug _owner;
        private readonly string _name;
        private double _horizontalAngle;
        private double _verticalAngle;

        private const double TirePart = 0.9;
        private const double EffPart = 0.1;

        public void ApplyHorEffector(double eff)
        {
            var tirePerc = _owner.TireLevel/(Bug.COM_MAX - Bug.COM_MIN);
            var result = eff * tirePerc * TirePart + eff * (1 - TirePart);

            result *= EffPart;
            if (_horizontalAngle + result > Bug.ANGLE_MAX) _horizontalAngle = Bug.ANGLE_MAX;
            else if (_horizontalAngle + result < Bug.ANGLE_MIN) _horizontalAngle = Bug.ANGLE_MIN;
            else _horizontalAngle += result;
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
