using System;
using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

namespace Country
{
    public class Country : MonoBehaviour
    {
        public event Action RegionsSets;
        public event Action UnionRegionsSets;
        
        [field: SerializeField] public GameObject CountryBallPrefab { get; private set; }
        [field: SerializeField] public bool IsPlayerCountry;
        
        [SerializeField] private List<Region> _regions;
        
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
            catch (Exception e)
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

            TryGetComponent(out Player.Player player);
            capturedRegion.Init(player);

            var isEnemyRegionRemained = GeneralAsset.Instance.RegionsForAttack.Any(regionForAttack => !regionForAttack.CompareTag(regionTag));

            if (isEnemyRegionRemained) return;
            
            AttackFinish(_enemyCountry, invaderRegion);
        }

        public void RemoveRegion(Region region) => _regions.Remove(region);

        private void AttackFinish(Country enemyCountry, Region invaderRegion)
        {
            print("Win!");
                
            GeneralAsset.Instance.AttackStarted = false;
            
            foreach (var country in GeneralAsset.Instance.AllCountries) country.gameObject.SetActive(true);
            
            if (!IsPlayerCountry)
            {
                float money = GeneralAsset.Instance.EnemyRegionForAttackCount * 100 * invaderRegion.CurrentMoney;
                Economy.Economy.IncreaseMoney(money);

                foreach (var region in GeneralAsset.Instance.PlayerRegionsForAttack) region.Init();
            }
            else if (IsPlayerCountry)
            {
                foreach (var region in GeneralAsset.Instance.EnemyRegionsForAttack) region.Init();
            }
            
            foreach (var region in GeneralAsset.Instance.RegionsForAttack)
            {
                region.StopIncreaseCountryBallCountCoroutine();
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
    }
}