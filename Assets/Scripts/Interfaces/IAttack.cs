using Country;

namespace Interfaces
{
    public interface IAttack
    {
        void Attack(Region ownRegion, Region enemyRegion);
    }
}