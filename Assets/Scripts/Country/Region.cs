using UnityEngine;

namespace Country
{
    public class Region : MonoBehaviour
    {
        [field: SerializeField] public Transform MainCountryBall { get; private set; }
        
        [SerializeField] private int _startCountryBallCount;

        [SerializeField] private int _currentCountryBallCount;

        private void Awake()
        {
            _currentCountryBallCount = _startCountryBallCount;
        }

        public void AttackEnemyRegion(Region enemyRegion)
        {
            transform.parent.TryGetComponent(out Country country);
            var countryBallSpawnerPosition = MainCountryBall.position;

            var countryBallContainer = Instantiate(country.CountryContainer.CountryBallContainerPrefab,
                new Vector3(countryBallSpawnerPosition.x, countryBallSpawnerPosition.y, countryBallSpawnerPosition.z), Quaternion.identity).transform;
            countryBallContainer.name += gameObject.tag;

            var countryBallCountToSpawn = _currentCountryBallCount - 1;
            
            for (int i = 0; i < countryBallCountToSpawn; i++)
            {
                _currentCountryBallCount -= 1;
                
                var countryBall = Instantiate(country.CountryBallPrefab, new Vector3(countryBallSpawnerPosition.x + i * 2, countryBallSpawnerPosition.y, countryBallSpawnerPosition.z),
                    Quaternion.identity);
                
                countryBall.transform.SetParent(countryBallContainer);
                countryBall.TryGetComponent(out CountryBall countryBallComponent);
                transform.parent.TryGetComponent(out Country ownCountry);
                
                countryBallComponent.Init(enemyRegion.MainCountryBall.position, enemyRegion, ownCountry);
            }
        }

        public void ProtectRegion(Country enemyCountry)
        {
            _currentCountryBallCount -= 1;
            print(_currentCountryBallCount);

            if (_currentCountryBallCount == 0)
            {
                print($"The region {tag}: {name} does not belong to anyone");
            }
            else if (_currentCountryBallCount < 0)
            {
                _currentCountryBallCount = 1;
                print($"The region {tag}: {name} belong to {enemyCountry.tag}");
                gameObject.tag = enemyCountry.tag;
                enemyCountry.AddBorders(this);
            }
        }
    }
}