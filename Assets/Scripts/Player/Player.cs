using Country;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private Country.Country _country;
        private PlayerAttack _playerAttack;
        private Country.Country _countryForAttack;

        private void Awake()
        {
            TryGetComponent(out _playerAttack);
            TryGetComponent(out _country);
        }
        
        public void Attack()
        {
            _country.GetRegionsForAttack(out var thisEnemyRegion, out var enemyRegion);
            
            _playerAttack.Attack(_countryForAttack, thisEnemyRegion, enemyRegion);

            _countryForAttack = null;
        }

        public void SelectRegionForAttack(Region enemyRegion)
        {
            _country.SelectRegionsForAttack(enemyRegion.tag, enemyRegion);
        }
    }
}