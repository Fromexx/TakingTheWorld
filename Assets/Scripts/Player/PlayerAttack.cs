using Assets.Scripts.Country.Region;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public void Attack(Region ownRegion, Region enemyRegion)
        {
            ownRegion.AttackEnemyRegion(enemyRegion);
        }
    }
}