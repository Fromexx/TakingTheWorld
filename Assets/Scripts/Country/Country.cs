using Assets;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Country
{
    public class Country : MonoBehaviour, ISaveableCountry
    {
        public event Action RegionsSets;
        public event Action UnionRegionsSets;

        [field: SerializeField] public GameObject CountryBallPrefab { get; private set; }
        [field: SerializeField] public bool IsPlayerCountry;
        [field: SerializeField] public byte Id { get; private set; }

        [SerializeField] private List<Region> _regions;
        [SerializeField] private List<byte> _ownRegionsId;
        [SerializeField] private GameObject _world;

        private Region _ourRegionForAttack;
        private Region _enemyRegionForAttack;
        private List<Region> _unionRegions;
        private Material _material;
        private Country _enemyCountry;

        private void Awake()
        {
            try
            {
                _regions[0].TryGetComponent(out Renderer renderer);
                _material = renderer.material;
            }
            catch (Exception)
            {
            }
        }

        public void SelectRegionsForAttack(string enemyCountryTag, Region enemyRegion = null)
        {
            EnableAllRegionBorders(enemyCountryTag, enemyRegion);
        }

        public void SelectRegionForAttack(string enemyRegionTag, List<RegionBorder> borders)
        {
            EnableRegionBordersForFindEnemyRegion(enemyRegionTag, borders);
        }

        public void SelectUnionRegions(List<RegionBorder> borders)
        {
            EnableRegionBorders(borders);
        }

        public void SetAttackRegions(Region ourRegion, Region enemyRegion)
        {
            if (_ourRegionForAttack != null || _enemyRegionForAttack != null)
            {
                Debug.Log("OurRegionForAttack or EnemyRegionForAttack already assigned! Code execution stopped!");
            }

            _ourRegionForAttack = ourRegion;
            _enemyRegionForAttack = enemyRegion;

            RegionsSets?.Invoke();
        }

        public void SetUnionRegions(List<Region> regions)
        {
            _unionRegions = regions;
            UnionRegionsSets?.Invoke();
        }

        public Renderer GetRenderer()
        {
            _regions[0].TryGetComponent(out Renderer renderer);
            return renderer;
        }

        public void GetRegionsForAttack(out Region ourRegion, out Region enemyRegion)
        {
            ourRegion = _ourRegionForAttack;
            enemyRegion = _enemyRegionForAttack;

            _ourRegionForAttack = null;
            _enemyRegionForAttack = null;
        }

        public List<Region> GetUnionRegions() => _unionRegions;

        public void AddRegion(Region capturedRegion, Region invaderRegion)
        {
            capturedRegion.TryGetComponent(out Renderer renderer);
            renderer.material = _material;
            capturedRegion.transform.parent.TryGetComponent(out _enemyCountry);
            capturedRegion.tag = tag;
            capturedRegion.transform.SetParent(transform);
            _regions.Add(capturedRegion);
            var regionTag = invaderRegion.tag;

            capturedRegion.InitCountry(this);
            capturedRegion.MainCountryBall.Init(this);

            if (!IsPlayerCountry)
            {
                GeneralAsset.Instance.EnemyRegionsForAttack.Add(capturedRegion);
                GeneralAsset.Instance.PlayerRegionsForAttack.Remove(capturedRegion);
                capturedRegion.StartCoroutineAttack();
            }
            else if (IsPlayerCountry)
            {
                GeneralAsset.Instance.EnemyRegionsForAttack.Remove(capturedRegion);
                GeneralAsset.Instance.PlayerRegionsForAttack.Add(capturedRegion);
                print(capturedRegion);
                capturedRegion.StopCoroutineAttack();
            }

            var isEnemyRegionRemained = GeneralAsset.Instance.RegionsForAttack.Any(regionForAttack => !regionForAttack.CompareTag(regionTag));

            if (isEnemyRegionRemained) return;

            AttackFinish(_enemyCountry, invaderRegion);
        }

        private void AddRegion(Region region, Country givingCountry)
        {
            var renderer = givingCountry.GetRenderer();
            renderer.material = _material;
            region.tag = tag;
            region.transform.SetParent(transform);
            _regions.Add(region);
        }

        public void RemoveRegion(Region region) => _regions.Remove(region);

        private void AttackFinish(Country enemyCountry, Region invaderRegion)
        {
            print("Win!");
            
            GeneralAsset.Instance.AttackStarted = false;

            TryGetComponent(out Enemy.Enemy enemy);
            enemy?.StopAttack();

            foreach (var country in GeneralAsset.Instance.AllCountries) country.gameObject.SetActive(true);

            if (!IsPlayerCountry)
            {
                foreach (var region in GeneralAsset.Instance.PlayerRegionsForAttack) region.Init();
            }
            else if (IsPlayerCountry)
            {
                float money = GeneralAsset.Instance.EnemyRegionForAttackCount * 100 * invaderRegion.CurrentMoney;
                Economy.Economy.IncreaseMoney(money);

                foreach (var region in GeneralAsset.Instance.EnemyRegionsForAttack) region.Init();
            }

            foreach (var region in GeneralAsset.Instance.RegionsForAttack)
            {
                region.StopAllRegionCoroutines();
                region.RecoverCountryBall();
            }

            EnableAllRegions();
            enemyCountry.EnableAllRegions();
        }

        public void DisableNotInvolvedRegions(List<Region> regions)
        {
            var iteration = 0;

            foreach (var countryRegion in _regions)
            {
                foreach (var involvedRegion in regions)
                {
                    iteration++;

                    if (countryRegion == involvedRegion)
                    {
                        iteration = 0;
                        break;
                    }
                    if (iteration == regions.Count)
                    {
                        countryRegion.gameObject.SetActive(false);
                        iteration = 0;
                    }
                }
            }
        }

        private void EnableAllRegions()
        {
            foreach (var region in _regions) region.gameObject.SetActive(true);
        }

        private void EnableAllRegionBorders(string enemyCountryTag, Region enemyRegion, Region ownRegion = null)
        {
            var borders = _regions.SelectMany(region => region.GetComponentsInChildren<RegionBorder>()).ToList();

            foreach (var border in borders) border.Init(enemyCountryTag, enemyRegion, borders, _regions);
        }

        private void EnableRegionBordersForFindEnemyRegion(string enemyTag, List<RegionBorder> borders)
        {
            foreach (var border in borders) border.InitWithOurRegion(enemyTag, borders, _regions);
        }

        private void EnableRegionBorders(List<RegionBorder> borders)
        {
            foreach (var border in borders) border.InitWithRegion(borders, _regions);
        }

        public void AddOwnRegions()
        {
            foreach (var country in _world.GetComponentsInChildren<Country>())
            {
                var regionCount = country.transform.childCount;
                
                for (int regionIndex = 0; regionIndex < regionCount; regionIndex++)
                {
                    var regionTransform = country.transform.GetChild(regionIndex);
                    regionTransform.TryGetComponent(out Region region);
                    if (_ownRegionsId.Contains(region.Id)) AddRegion(region, country);
                }
            }
        }

        public void Import(ProgressCountry progressCountry)
        {
            _ownRegionsId = progressCountry.OwnRegionsId;
            IsPlayerCountry = Convert.ToBoolean(progressCountry.IsPlayerCountry);
            AddOwnRegions();
        }

        public ProgressCountry Export()
        {
            return new ProgressCountry()
            {
                Id = Id,
                IsPlayerCountry = Convert.ToByte(IsPlayerCountry),
                OwnRegionsId = _ownRegionsId
            };
        }
    }
}
