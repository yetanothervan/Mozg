using Interfaces;

namespace CnsService
{
    public class CnsService : ICnsService
    {
        public ICns CreateCnc()
        {
            return new Cns();
        }
    }
}