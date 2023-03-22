using Country;
using Interfaces;
using UnityEngine;

namespace ArtificialIntelligence
{
    public class EnemyAttack : MonoBehaviour, IAttack
    {
        public void Attack(Region ourRegion, Region enemyRegion)
        {
            print("Attack");
            ourRegion.AttackEnemyRegion(enemyRegion);
        }
    }
}