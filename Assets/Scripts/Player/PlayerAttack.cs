using Country;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public void Attack(Country.Country countryForAttack, Region ourRegion, Region enemyRegion)
        {
            print("Attack");
            ourRegion.AttackEnemyRegion(enemyRegion);
        }
    }
}