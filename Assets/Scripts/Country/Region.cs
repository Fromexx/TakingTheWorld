using System;
using System.Collections;
using System.Collections.Generic;
using Assets;
using Economy;
using Interfaces;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Country
{
    public class Region : MonoBehaviour, ISaveableRegion
    {
        public float CurrentMoney { get; private set; }
        [field: SerializeField] public MainCountryBall MainCountryBall { get; private set; }
        [field: SerializeField] public byte Id { get; private set; }

        public List<RegionBorder> Borders;

        private int _currentCountryBallCount;
        private Country _country;
        private TuneLevel _tuneLevel;
        private Region _playerRegion;
        private Country _playerCountry;
        private float _timeBetweenAttack;
        private Transform _mainCountryBallTransform;
        private byte _countryBallLevel;
        private byte _moneyLevel;

        private void Awake()
        {
            try
            {
                _tuneLevel = new TuneLevel(_countryBallLevel, _moneyLevel);
                _currentCountryBallCount = _tuneLevel.GetCountryBallTuneCount();
                CurrentMoney = _tuneLevel.GetMoneyTune();

                transform.parent.TryGetComponent(out _country);
                MainCountryBall.TryGetComponent(out _mainCountryBallTransform);

                MainCountryBall.Init(_currentCountryBallCount);

                _timeBetweenAttack = GeneralAsset.Instance.TimeBetweenAttack;
            }
            catch(Exception)
            {
            }
        }

        public void InitCountry(Country country) => _country = country;

        public void Init()
        {
            _tuneLevel = new TuneLevel(1, 1);
            _currentCountryBallCount = _tuneLevel.GetCountryBallTuneCount();
            CurrentMoney = _tuneLevel.GetMoneyTune();

            MainCountryBall.Init(_currentCountryBallCount);
        }

        public void StartCoroutineAttack() => StartCoroutine(Attack());
        public void StopCoroutineAttack() => StopCoroutine(Attack());
        
        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(_timeBetweenAttack);
            
            var playerRegions = GeneralAsset.Instance.PlayerRegionsForAttack;

            System.Random random = new System.Random();
            var playerRegionIndex = random.Next(0, playerRegions.Count);
            
            AttackEnemyRegion(playerRegions[playerRegionIndex]);
            
            StartCoroutine(Attack());
        }

        public void AttackEnemyRegion(Region enemyRegion)
        {
            StopIncreaseCountryBallCountCoroutine();
            enemyRegion.StopIncreaseCountryBallCountCoroutine();
            StartCoroutine(SpawnCountryBall(enemyRegion));
        }

        public void ProtectRegion(Transform enemyCountry, Region enemyRegion, bool isLastCountryBall)
        {
            transform.parent.TryGetComponent(out Country ownCountry);
            enemyCountry.TryGetComponent(out Country enemyCountryComponent);
            
            if (ownCountry == enemyCountryComponent)
            {
                IncrementCurrentCountryBallCount(true);
                return;
            }

            DecrementCurrentCountryBallCount();

            if (isLastCountryBall) StartIncreaseCountryBallCountCoroutine();

            if (_currentCountryBallCount != 0) return;
            
            _currentCountryBallCount = 1;
            MainCountryBall.Init(_currentCountryBallCount);
            _country.RemoveRegion(this);
            
            enemyCountryComponent.AddRegion(this, enemyRegion);
        }

        public void TuneCountryBallCount()
        {
            _tuneLevel.UpCountryBallLevel();

            var cost = _tuneLevel.GetCountryBallTuneCost();
            if (cost == -1)
            {
                _tuneLevel.DownCountryBallLevel();
                return;
            }
            
            if (!Economy.Economy.DecreaseMoney(cost)) return;
            
            _currentCountryBallCount = _tuneLevel.GetCountryBallTuneCount();
            MainCountryBall.Init(_currentCountryBallCount);

            GeneralAsset.Instance.RegionTuneView.Render(_tuneLevel, this);
        }

        public void TuneMoney()
        {
            _tuneLevel.UpMoneyLevel();

            var cost = _tuneLevel.GetMoneyTuneCost();
            if (cost == -1)
            {
                _tuneLevel.DownMoneyLevel();
                return;
            }
            
            if (!Economy.Economy.DecreaseMoney(cost)) return;
            
            CurrentMoney = _tuneLevel.GetMoneyTune();
            
            GeneralAsset.Instance.RegionTuneView.Render(_tuneLevel, this);
        }

        public void RecoverCountryBall()
        {
            _currentCountryBallCount = _tuneLevel.GetCountryBallTuneCount();
            MainCountryBall.Init(_currentCountryBallCount);
        }

        private void StartIncreaseCountryBallCountCoroutine() => StartCoroutine(IncreaseCountryBallCount());
        public void StopIncreaseCountryBallCountCoroutine() => StopCoroutine(IncreaseCountryBallCount());
        public void StopAllRegionCoroutines() => StopAllCoroutines();

        private IEnumerator SpawnCountryBall(Region enemyRegion)
        {
            transform.parent.TryGetComponent(out Country country);
            var countryBallSpawnerPosition = _mainCountryBallTransform.position;
            var needCountryBallCountToSpawn = enemyRegion.CompareTag(tag) ? enemyRegion._tuneLevel.GetCountryBallTuneCount() - enemyRegion._currentCountryBallCount
                : enemyRegion._currentCountryBallCount + enemyRegion._tuneLevel.GetCountryBallTuneCount();
            var countryBallCountToSpawn = _currentCountryBallCount - 1 - needCountryBallCountToSpawn < 0
                ? _currentCountryBallCount
                : needCountryBallCountToSpawn;

            enemyRegion.StopIncreaseCountryBallCountCoroutine();
            
            GeneralAsset.Instance.SoldierVoicesPlayer.Play();

            for (int i = 0; i < countryBallCountToSpawn; i++)
            {
                if (_currentCountryBallCount == 1) break;
                
                _currentCountryBallCount -= 1;
                MainCountryBall.Init(_currentCountryBallCount);

                var countryBall = Instantiate(country.CountryBallPrefab, new Vector3(countryBallSpawnerPosition.x, countryBallSpawnerPosition.y, countryBallSpawnerPosition.z),
                    Quaternion.identity);
            
                countryBall.TryGetComponent(out CountryBall countryBallComponent);
                
                countryBallComponent.Init(enemyRegion._mainCountryBallTransform.position, enemyRegion, transform.parent, this, countryBallCountToSpawn, i+1);
                
                yield return new WaitForSeconds(GeneralAsset.Instance.TimeBetweenCountryBallSpawn);
            }
            
            StartIncreaseCountryBallCountCoroutine();
        }

        private IEnumerator IncreaseCountryBallCount()
        {
            yield return new WaitForSeconds(GeneralAsset.Instance.TimeBetweenIncreaseCountryBall);
            
            IncrementCurrentCountryBallCount();

            StartCoroutine(IncreaseCountryBallCount());
        }

        private void IncrementCurrentCountryBallCount(bool isCountryBallFromAnotherRegion = false)
        {
            if (!isCountryBallFromAnotherRegion && _currentCountryBallCount >= _tuneLevel.GetCountryBallTuneCount()) return;
            
            _currentCountryBallCount += 1;
            MainCountryBall.Init(_currentCountryBallCount);

            var mainCountryBallPosition = _mainCountryBallTransform.position;

            var startYScale = _mainCountryBallTransform.localScale.y;
            
            _mainCountryBallTransform.position = new Vector3(mainCountryBallPosition.x, mainCountryBallPosition.y + (_mainCountryBallTransform.localScale.y - startYScale), mainCountryBallPosition.z);
        }

        private void DecrementCurrentCountryBallCount()
        {
            _currentCountryBallCount -= 1;
            MainCountryBall.Init(_currentCountryBallCount);
        }
        
        private void AttackPrepare()
        {
            foreach (var border in Borders) border.gameObject.SetActive(false);

            foreach (var country in GeneralAsset.Instance.AllCountries)
            {
                if (country == _country || country == GeneralAsset.Instance.PlayerCountry) continue;

                country.gameObject.SetActive(false);
            }
            
            _country.DisableNotInvolvedRegions(GeneralAsset.Instance.RegionsForAttack);
            GeneralAsset.Instance.PlayerCountry.DisableNotInvolvedRegions(GeneralAsset.Instance.RegionsForAttack);

            foreach (var region in GeneralAsset.Instance.RegionsForAttack) region.StartIncreaseCountryBallCountCoroutine();

            var regionsTransform = new List<Transform>();

            foreach (var region in GeneralAsset.Instance.RegionsForAttack)
            {
                regionsTransform.Add(region.transform);
            }
            
            GeneralAsset.Instance.AttackStarted = true;

            _country.TryGetComponent(out Enemy.Enemy enemy);
            enemy.StartAttack();
        }

        private void OnRegionsSets()
        {
            _country.RegionsSets -= OnRegionsSets;
            
            _country.GetRegionsForAttack(out var ourRegion, out _playerRegion);
            GeneralAsset.Instance.RegionsForAttack.Add(ourRegion);
            GeneralAsset.Instance.EnemyRegionsForAttack.Add(ourRegion);
            GeneralAsset.Instance.RegionsForAttack.Add(_playerRegion);
            GeneralAsset.Instance.PlayerRegionsForAttack.Add(_playerRegion);
            
            GeneralAsset.Instance.EnemyRegionForAttackCount = 1;

            foreach (var border in Borders) border.NotFoundUnionRegions += OnOurUnionRegionsSets;
            
            _country.UnionRegionsSets += OnOurUnionRegionsSets;
            _country.SelectUnionRegions(Borders);
        }

        private void OnOurUnionRegionsSets()
        {
            _country.UnionRegionsSets -= OnOurUnionRegionsSets;
            
            var unionRegions = _country.GetUnionRegions();
            if (!(unionRegions is null))
            {
                GeneralAsset.Instance.EnemyRegionForAttackCount += unionRegions.Count;
                
                foreach (var region in unionRegions)
                {
                    GeneralAsset.Instance.RegionsForAttack.Add(region);
                    GeneralAsset.Instance.EnemyRegionsForAttack.Add(region);
                }
            }
            
            _playerRegion.transform.parent.TryGetComponent(out _playerCountry);

            foreach (var border in _playerRegion.Borders) border.NotFoundUnionRegions += OnPlayerUnionRegionsSets;
            _playerCountry.UnionRegionsSets += OnPlayerUnionRegionsSets;

            _playerCountry.SelectUnionRegions(_playerRegion.Borders);
        }

        private void OnPlayerUnionRegionsSets()
        {
            _country.UnionRegionsSets -= OnOurUnionRegionsSets;

            var unionRegions = _playerCountry.GetUnionRegions();
            if (!(unionRegions is null))
            {
                foreach (var region in unionRegions)
                {
                    GeneralAsset.Instance.RegionsForAttack.Add(region);
                    GeneralAsset.Instance.PlayerRegionsForAttack.Add(region);
                }
            }

            AttackPrepare();
        }
        
        private void OnMouseDown()
        {
            if (GeneralAsset.Instance.IsSelectedCountry)
            {
                _country.IsPlayerCountry = true;
                GeneralAsset.Instance.PlayerCountry = _country;
                _country.AddComponent<PlayerAttack>();
                var player = _country.AddComponent<Player.Player>();

                _country.TryGetComponent(out Enemy.Enemy enemy);
                Destroy(enemy);

                foreach (var mainCountryBall in GeneralAsset.Instance.AllMainCountryBalls) mainCountryBall.InitPlayer(player);

                GeneralAsset.Instance.SelectCountryUI.SetActive(false);
                GeneralAsset.Instance.IsSelectedCountry = false;

                return;
            }
            
            if (GeneralAsset.Instance.AttackStarted) return;
            if (_country.IsPlayerCountry)
            {
                GeneralAsset.Instance.RegionTuneView.Render(_tuneLevel, this);
                return;
            }
            
            GeneralAsset.Instance.RegionTuneView.OnClose();
            GeneralAsset.Instance.RegionsForAttack = new List<Region>();
            GeneralAsset.Instance.PlayerRegionsForAttack = new List<Region>();
            GeneralAsset.Instance.EnemyRegionsForAttack = new List<Region>();
            _country.SelectRegionForAttack(GeneralAsset.Instance.PlayerCountry.tag, Borders);
            
            _country.RegionsSets += OnRegionsSets;
        }

        public void Import(ProgressRegion progressRegion)
        {
            _countryBallLevel = progressRegion.CountryBallLevel;
            _moneyLevel = progressRegion.MoneyLevel;
        }

        public ProgressRegion Export()
        {
            ProgressRegion progressRegion = new ProgressRegion
            {
                Id = Id,
                CountryBallLevel = (byte) _tuneLevel.CurrentCountryBallLevel,
                MoneyLevel = (byte) _tuneLevel.CurrentMoneyLevel
            };

            return progressRegion;
        }
    }
}