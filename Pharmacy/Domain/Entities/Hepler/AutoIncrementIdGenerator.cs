using Pharmacy.Application.Abstraction;

namespace Pharmacy.Domain.Entities.Helper
{
    /*
        Generate Id for each Entity Automatic

    */
    public class AutoIncrementIdGenerator  
    {
        private int _max = 0;

        public int NextId() => ++_max;
    }
}