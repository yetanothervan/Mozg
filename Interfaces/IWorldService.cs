namespace Interfaces
{
    public interface IWorldService
    {
        void DoStep();
        ICreature GetFirstCreature();
    }
}
