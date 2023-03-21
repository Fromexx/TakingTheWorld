using ArtificialIntelligence;
using Country;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private CountryContainer _countryContainer;
        [field: SerializeField] public Player.Player Player { get; private set; }
        
        private EnemyAttack _enemyAttack;
        private Country.Country _countryForAttack;
        private Country.Country _country;

        private void Awake()
        {
            TryGetComponent(out _country);
            TryGetComponent(out _enemyAttack);

            // SelectCountryForAttack();
            // _country.RegionsSets += Attack;
            // AttackPrepare();
        }

        private void SelectCountryForAttack()
        {
            while (_countryForAttack is null)
            {
                System.Random random = new System.Random();

                _countryForAttack = _countryContainer.Countries[random.Next(0, _countryContainer.Countries.Count)];
                
                if(_countryForAttack == _country) _countryForAttack = null;
            }
        }

        private void AttackPrepare()
        {
            _country.SelectRegionsForAttack(_countryForAttack.tag);
        }

        private void Attack()
        {
            _country.GetRegionsForAttack(out var thisEnemyRegion, out var enemyRegion);
            
            _enemyAttack.Attack(_countryForAttack, thisEnemyRegion, enemyRegion);
        }
    }
}