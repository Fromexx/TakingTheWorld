﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets;
using Economy;
using UnityEngine;

namespace Country
{
    public class Region : MonoBehaviour
    {
        [field: SerializeField] public Transform MainCountryBall { get; private set; }
        [field: SerializeField] public float CurrentMoney { get; private set; }
        [field: SerializeField] public List<RegionBorder> Borders { get; private set; }

        [SerializeField] private int _currentCountryBallCount;
        private Vector3 _startMainCountryBallScale;
        private int _currentIncreaseCount;
        private MainCountryBall _mainCountryBall;
        private Country _country;
        private TuneLevel _tuneLevel;
        private Region _playerRegion;
        private Country _playerCountry;

        private void Awake()
        {
            try
            {
                _tuneLevel = new TuneLevel(1, 1);
                _currentCountryBallCount = _tuneLevel.GetCountryBallTuneCount();
                CurrentMoney = _tuneLevel.GetMoneyTune();

                transform.parent.TryGetComponent(out _country);

                MainCountryBall.TryGetComponent(out _mainCountryBall);
    
                for (int i = 0; i < _currentCountryBallCount; i++)
                {
                    _mainCountryBall.IncreaseScale();
                }
            }
            catch (Exception e)
            {
            }
        }

        public void Init(Player.Player player)
        {
            transform.parent.TryGetComponent(out _country);
            _mainCountryBall.Init(player);
        }

        public void Init()
        {
            _tuneLevel = new TuneLevel(1, 1);
            _currentCountryBallCount = _tuneLevel.GetCountryBallTuneCount();
            CurrentMoney = _tuneLevel.GetMoneyTune();
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
                IncrementCurrentCountryBallCount();
                return;
            }

            DecrementCurrentCountryBallCount();

            if (isLastCountryBall) StartIncreaseCountryBallCountCoroutine();

            if (_currentCountryBallCount != 0) return;
            
            _currentCountryBallCount = 1;
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

            for (int i = 0; i < _currentCountryBallCount; i++)
            {
                _mainCountryBall.IncreaseScale();
            }
        }

        private void StartIncreaseCountryBallCountCoroutine() => StartCoroutine(IncreaseCountryBallCount());
        public void StopIncreaseCountryBallCountCoroutine() => StopCoroutine(IncreaseCountryBallCount());

        private IEnumerator SpawnCountryBall(Region enemyRegion)
        {
            transform.parent.TryGetComponent(out Country country);
            var countryBallSpawnerPosition = MainCountryBall.position;
            var needCountryBallCountToSpawn = enemyRegion.CompareTag(tag) ? enemyRegion._tuneLevel.GetCountryBallTuneCount() - enemyRegion._currentCountryBallCount
                : enemyRegion._currentCountryBallCount + enemyRegion._tuneLevel.GetCountryBallTuneCount();
            var countryBallCountToSpawn = _currentCountryBallCount - 1 - needCountryBallCountToSpawn < 0
                ? _currentCountryBallCount
                : needCountryBallCountToSpawn;
            
            enemyRegion.StopIncreaseCountryBallCountCoroutine();

            for (int i = 0; i < countryBallCountToSpawn; i++)
            {
                _currentCountryBallCount -= 1;

                var countryBall = Instantiate(country.CountryBallPrefab, new Vector3(countryBallSpawnerPosition.x, countryBallSpawnerPosition.y, countryBallSpawnerPosition.z),
                    Quaternion.identity);
            
                countryBall.TryGetComponent(out CountryBall countryBallComponent);
                
                countryBallComponent.Init(enemyRegion.MainCountryBall.position, enemyRegion, transform.parent, this, countryBallCountToSpawn, i+1);
            
                _mainCountryBall.DecreaseScale();
                
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

        private void IncrementCurrentCountryBallCount()
        {
            if (_currentCountryBallCount >= _tuneLevel.GetCountryBallTuneCount()) return;
            
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

            GeneralAsset.Instance.AttackStarted = true;
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
            if (_country.IsPlayerCountry && !GeneralAsset.Instance.AttackStarted)
            {
                GeneralAsset.Instance.RegionTuneView.Render(_tuneLevel, this);
                return;
            }

            if (GeneralAsset.Instance.AttackStarted) return;

            GeneralAsset.Instance.RegionsForAttack = new List<Region>();
            GeneralAsset.Instance.PlayerRegionsForAttack = new List<Region>();
            GeneralAsset.Instance.EnemyRegionsForAttack = new List<Region>();
            _country.SelectRegionForAttack(GeneralAsset.Instance.PlayerCountry.tag, Borders);
            
            _country.RegionsSets += OnRegionsSets;
        }
    }
}