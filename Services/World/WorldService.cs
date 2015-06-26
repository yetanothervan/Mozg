﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Action = Interfaces.Action;

namespace WorldService
{
    public class WorldService : IWorldService
    {
        public const string TheBug = "TheBug";
        private readonly Dictionary<string, ICreature> _creatures;
        private readonly Dictionary<string, CreatureState> _creaturesStates;
        private readonly Dictionary<string, Action> _pendingActions;
        private readonly List<Food> _foodList;


        public WorldService()
        {
            _foodList = new List<Food>
            {
                new Food() {X = 200.0, Y = 200.0}
            };
            _pendingActions = new Dictionary<string, Action>();
            _creatures = new Dictionary<string, ICreature>
            {
                {TheBug, new Bug.Bug(TheBug, this)}
            };
            _creaturesStates = new Dictionary<string, CreatureState>
            {
                {TheBug, new CreatureState(50.0, 50.0, 0.0)}
            };
        }


        public void DoStep()
        {
            GetActions();
            CalculateStep();
            AdvantageMoment();
        }

        public ICreature GetFirstCreature()
        {
            return _creatures[TheBug];
        }

        private void GetActions()
        {
            foreach (var creature in _creatures)
            {
                var actions = creature.Value.GetActions();
                if (actions != null && actions.Any())
                    foreach (var action in actions)
                        _pendingActions.Add(creature.Key, action);
            }
        }

        private void CalculateStep()
        {
            foreach (var pendingAction in _pendingActions)
            {
                switch (pendingAction.Value.ActionType)
                {
                    case ActionType.Rotate:
                    {
                        var state = _creaturesStates[pendingAction.Key];
                        state.Angle += pendingAction.Value.Value;
                        _creaturesStates[pendingAction.Key] = state;
                    }
                        break;
                    case ActionType.Move:
                    {
                        var state = _creaturesStates[pendingAction.Key];
                        state.X += pendingAction.Value.Value*Math.Cos(state.Angle);
                        state.Y += pendingAction.Value.Value*Math.Sin(state.Angle);
                        _creaturesStates[pendingAction.Key] = state;
                    }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void AdvantageMoment()
        {
            foreach (var creature in _creatures)
                creature.Value.AdvantageMoment();
        }

        public bool IsCreatureOnFood(string name)
        {
            CreatureState state;
            if (_creaturesStates.TryGetValue(name, out state))
                return GetFoodOnCoords(state.X, state.Y) != null;
            return false;
        }

        private Food? GetFoodOnCoords(double x, double y)
        {
            return _foodList.FirstOrDefault(food => food.X - x < 0.01 && food.Y - y < 0.01);
        }

        private Food? GetNearestFood(double x, double y)
        {
            var result =
                _foodList.Select(food => new {food, Dist = GetDist(food.X, food.Y, x, y)})
                    .OrderByDescending(arg => arg.Dist).FirstOrDefault();
            if (result != null)
                return result.food;
            return null;
        }

        double GetDist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public void EatFoodUnderMe(string name)
        {
            CreatureState state;
            if (_creaturesStates.TryGetValue(name, out state))
            {
                var food = GetFoodOnCoords(state.X, state.Y);
                if (food != null)
                    _foodList.Remove(food.Value);
            }
        }

        public double GetNearestFoodAngle(string name)
        {
            CreatureState state;
            if (_creaturesStates.TryGetValue(name, out state))
            {
                var food = GetNearestFood(state.X, state.Y);
                if (food != null)
                {
                    double sin = (state.X - food.Value.X) / GetDist(state.X, state.Y, food.Value.X, food.Value.Y);
                    double cos = (state.Y - food.Value.Y) / GetDist(state.X, state.Y, food.Value.X, food.Value.Y);
                    double sin2 = state.Angle;
                    double cos2 = Math.Sqrt(1 - Math.Pow(sin2, 2));

                    return sin*cos2 - cos*sin2;
                }
            }
            return 0.0;
        }

        public double GetNearestFoodDist(string name)
        {
            CreatureState state;
            if (_creaturesStates.TryGetValue(name, out state))
            {
                var food = GetNearestFood(state.X, state.Y);
                if (food != null)
                    return GetDist(state.X, state.Y, food.Value.X, food.Value.Y);
            }
            return 10000;
        }
    }

    struct CreatureState
    {
        public double X;
        public double Y;
        public double Angle;

        public CreatureState(double x, double y, double angle) : this()
        {
            X = x;
            Y = y;
            Angle = angle;
        }
    }

    struct Food
    {
        public double X;
        public double Y;
    }
}
