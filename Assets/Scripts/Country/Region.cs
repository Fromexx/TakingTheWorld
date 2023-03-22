using System.Collections;
using Assets;
using UnityEngine;

namespace Country
{
    public class Region : MonoBehaviour
    {
        [field: SerializeField] public Transform MainCountryBall { get; private set; }
        [SerializeField] private int _startCountryBallCount;
        
        private int _currentCountryBallCount;
        private Vector3 _startMainCountryBallScale;
        [SerializeField] private int _currentIncreaseCount;
        private MainCountryBall _mainCountryBall;

        private void Awake()
        {
            _currentCountryBallCount = _startCountryBallCount;

            MainCountryBall.TryGetComponent(out _mainCountryBall);
            
            for (int i = 0; i < _currentCountryBallCount; i++)
            {
                _mainCountryBall.IncreaseScale();
            }

            StartCoroutine(IncreaseCountryBallCount());
        }

        public void Init(Player.Player player) => _mainCountryBall.Init(player);

        public void AttackEnemyRegion(Region enemyRegion) => StartCoroutine(SpawnCountryBall(enemyRegion));

        public void ProtectRegion(Transform enemyCountry)
        {
            transform.parent.TryGetComponent(out Country ownCountry);
            enemyCountry.TryGetComponent(out Country enemyCountryComponent);
            
            if (ownCountry == enemyCountryComponent)
            {
                IncrementCurrentCountryBallCount();
                return;
            }

            DecrementCurrentCountryBallCount();

            if (_currentCountryBallCount != 0) return;
            
            _currentCountryBallCount = 1;
            enemyCountryComponent.AddRegion(this, transform);
        }

        private IEnumerator IncreaseCountryBallCount()
        {
            yield return new WaitForSeconds(GeneralAsset.Instance.TimeBetweenIncreaseCountryBall);

            IncrementCurrentCountryBallCount();

            StartCoroutine(IncreaseCountryBallCount());
        }

        private IEnumerator SpawnCountryBall(Region enemyRegion)
        {
            transform.parent.TryGetComponent(out Country country);
            var countryBallSpawnerPosition = MainCountryBall.position;
            var countryBallCountToSpawn = _currentCountryBallCount - 1;
            
            for (int i = 0; i < countryBallCountToSpawn; i++)
            {
                _currentCountryBallCount -= 1;

                var countryBall = Instantiate(country.CountryBallPrefab, new Vector3(countryBallSpawnerPosition.x, countryBallSpawnerPosition.y, countryBallSpawnerPosition.z),
                    Quaternion.identity);
            
                countryBall.TryGetComponent(out CountryBall countryBallComponent);
                
                countryBallComponent.Init(enemyRegion.MainCountryBall.position, enemyRegion, transform.parent);
            
                _mainCountryBall.DecreaseScale();
                
                yield return new WaitForSeconds(GeneralAsset.Instance.TimeBetweenCountryBallSpawn);
            }
        }

        private void IncrementCurrentCountryBallCount()
        {
            if (_currentCountryBallCount == GeneralAsset.Instance.MaxCountryBallCount) return;
            
            _currentCountryBallCount += 1;

            var mainCountryBallPosition = MainCountryBall.position;

            var startYScale = MainCountryBall.localScale.y;
            
            _mainCountryBall.IncreaseScale();
            MainCountryBall.position = new Vector3(mainCountryBallPosition.x, mainCountryBallPosition.y + (MainCountryBall.localScale.y - startYScale), mainCountryBallPosition.z);
        }

        private void DecrementCurrentCountryBallCount()
        {
            _currentCountryBallCount -= 1;
            
            _mainCountryBall.DecreaseScale();
        }
    }
}