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
        public event Action<List<RegionBorder>> UnionRegionsSets;
        
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
            _regions[0].TryGetComponent(out Renderer renderer);
            _material = renderer.material;
        }

        public void SelectRegionsForAttack(string enemyCountryTag, Region enemyRegion = null)
        {
            EnableAllRegionBorders(enemyCountryTag, enemyRegion);
        }

        public void SelectRegionForAttack(string enemyRegionTag, Region ourRegion, List<RegionBorder> borders)
        {
            EnableRegionBordersForFindEnemyRegion(enemyRegionTag, ourRegion, borders);
        }

        public void SelectUnionRegions(Region region, List<RegionBorder> borders)
        {
            EnableRegionBorders(region, borders);
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

        public void SetUnionRegions(List<Region> regions, List<RegionBorder> borders)
        {
            _unionRegions = regions;
            UnionRegionsSets?.Invoke(borders);
        }

        public void GetRegionsForAttack(out Region ourRegion, out Region enemyRegion)
        {
            ourRegion = _ourRegionForAttack;
            enemyRegion = _enemyRegionForAttack;
            
            _ourRegionForAttack = null;
            _enemyRegionForAttack = null;
        }

        public List<Region> GetUnionRegions() => _unionRegions;

        public void AddRegion(Region capturedRegion, Region invaderRegion, List<Region> regionsForAttack)
        {
            var capturedRegionParent = capturedRegion.transform.parent;
            capturedRegionParent.TryGetComponent(out Renderer renderer);
            renderer.material = _material;
            capturedRegionParent.TryGetComponent(out _enemyCountry);
            capturedRegion.tag = tag;
            capturedRegion.transform.SetParent(transform);
            _regions.Add(capturedRegion);
            var regionTag = invaderRegion.tag;

            TryGetComponent(out Player.Player player);
            if (player is null) return;
            capturedRegion.Init(player);

            var isEnemyRegionRemained = false;
            
            foreach (var regionForAttack in regionsForAttack.Where(regionForAttack => !regionForAttack.CompareTag(regionTag)))
            {
                print(regionForAttack);
                isEnemyRegionRemained = true;
            }

            if (isEnemyRegionRemained) return;
            
            AttackFinish(_enemyCountry, invaderRegion);
        }

        public void RemoveRegion(Region region) => _regions.Remove(region);

        private void AttackFinish(Country enemyCountry, Region invaderRegion)
        {
            print("Win!");
                
            GeneralAsset.Instance.AttackStarted = false;
            
            foreach (var country in GeneralAsset.Instance.AllCountries) country.gameObject.SetActive(true);
            
            EnableAllRegions();
            enemyCountry.EnableAllRegions();
            
            if (!IsPlayerCountry) return;
            float money = GeneralAsset.Instance.EnemyRegionForAttackCount * 100 * invaderRegion.CurrentMoney;
            Economy.Economy.IncreaseMoney(money);
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
            
            foreach (var border in borders)
            {
                border.Init(enemyCountryTag, enemyRegion, borders);
                border.gameObject.SetActive(true);
            }
        }

        private void EnableRegionBordersForFindEnemyRegion(string enemyTag, Region ourRegion, List<RegionBorder> borders)
        {
            foreach (var border in borders)
            {
                border.InitWithOurRegion(enemyTag, ourRegion, borders);
                border.gameObject.SetActive(true);
            }
        }

        private void EnableRegionBorders(Region region, List<RegionBorder> borders)
        {
            foreach (var border in borders)
            {
                border.InitWithRegion(region, borders);
                border.gameObject.SetActive(true);
            }
        }
    }
}