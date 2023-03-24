using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            print("tjhij");
            
            _unionRegions = regions;
            UnionRegionsSets?.Invoke(borders);
        }

        public void GetRegionsForAttack(out Region ourRegion, out Region enemyRegion)
        {
            ourRegion = _ourRegionForAttack;
            enemyRegion = _enemyRegionForAttack;
            
            DisableAllRegionBorders();
            _ourRegionForAttack = null;
            _enemyRegionForAttack = null;
        }

        public List<Region> GetUnionRegions() => _unionRegions;

        public void AddRegion(Region region, Region invaderRegion)
        {
            gameObject.tag = tag;
            region.transform.SetParent(transform);
            _regions.Add(region);

            TryGetComponent(out Player.Player player);
            if (player is null) return;
            region.Init(player);

            Economy.Economy.IncreaseMoney(invaderRegion.CurrentMoney);
        }

        private void EnableAllRegionBorders(string enemyCountryTag, Region enemyRegion, Region ownRegion = null)
        {
            foreach (var border in _regions.SelectMany(region => region.GetComponentsInChildren<RegionBorder>()))
            {
                border.Init(enemyCountryTag, enemyRegion);
                border.gameObject.SetActive(true);
            }
        }

        private void EnableRegionBordersForFindEnemyRegion(string enemyTag, Region ourRegion, List<RegionBorder> borders)
        {
            foreach (var border in borders)
            {
                border.InitWithOurRegion(enemyTag, ourRegion);
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

        private void DisableAllRegionBorders()
        {
            foreach (var border in _regions.SelectMany(region => region.GetComponentsInChildren<RegionBorder>()))
            {
                border.gameObject.SetActive(false);
            }
        }
    }
}