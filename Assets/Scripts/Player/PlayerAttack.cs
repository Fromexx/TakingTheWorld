using Country;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour, IAttack
    {
        public void Attack(Region ownRegion, Region enemyRegion)
        {
            ownRegion.AttackEnemyRegion(enemyRegion);
        }
    }
}