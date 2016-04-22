namespace Interfaces
{
    public struct Action
    {
        public ActionType ActionType;
        public double Value;
    }

    public enum ActionType
    {
        Move,
        Rotate
    }
}
