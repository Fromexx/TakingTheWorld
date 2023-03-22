using Country;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour, IAttack
    {
        public void Attack(Region ownRegion, Region enemyRegion)
        {
            print("Attack");
            ownRegion.AttackEnemyRegion(enemyRegion);
        }
    }
}