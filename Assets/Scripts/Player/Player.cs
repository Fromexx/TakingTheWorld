using Country;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private PlayerAttack _playerAttack;
        private Country.Country _countryForAttack;

        private Region _ownRegion;
        private Region _enemyRegion;

        private void Awake()
        {
            TryGetComponent(out _playerAttack);
        }
        
        public void Attack()
        {
            _playerAttack.Attack(_countryForAttack, _ownRegion, _enemyRegion);

            _countryForAttack = null;
        }

        public void SetOwnRegionForAttack(Region ownRegion) => _ownRegion = ownRegion;

        public void SetEnemyRegionForAttack(Region enemyRegion)
        {
            _enemyRegion = enemyRegion;
            Attack();
        }
    }
}