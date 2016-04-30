namespace Interfaces
{
    public interface IFourLeg
    {
        ILeg GetFrontLeftLeg();
        ILeg GetBackLeftLeg();
        ILeg GetFrontRightLeg();
        ILeg GetBackRightLeg();
    }
}