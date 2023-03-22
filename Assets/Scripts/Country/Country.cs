using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Country
{
    public class Country : MonoBehaviour
    {
        public event Action RegionsSets;
        
        [SerializeField] private List<GameObject> _borders;
        [field: SerializeField] public GameObject CountryBallPrefab { get; private set; }
        
        private Region _ourRegionForAttack;
        private Region _enemyRegionForAttack;
        
        public void SelectRegionsForAttack(string enemyCountryTag, Region enemyRegion = null)
        {
            EnableAllRegionBorders(enemyCountryTag, enemyRegion);
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

        public void GetRegionsForAttack(out Region ourRegion, out Region enemyRegion)
        {
            ourRegion = _ourRegionForAttack;
            enemyRegion = _enemyRegionForAttack;
            
            DisableAllRegionBorders();
            _ourRegionForAttack = null;
            _enemyRegionForAttack = null;
        }

        public void AddRegion(Region regionComponent, Transform region)
        {
            gameObject.tag = tag;
            region.SetParent(transform);
            _borders.AddRange(regionComponent.GetComponentsInChildren<RegionBorder>());

            TryGetComponent(out Player.Player player);
            if (player is null) return;
            regionComponent.Init(player);
        }

        private void EnableAllRegionBorders(string enemyCountryTag, Region enemyRegion)
        {
            foreach (var border in _borders)
            {
                border.TryGetComponent(out RegionBorder regionBorder);
                regionBorder.SetEnemyCountryTag(enemyCountryTag);
                if(!(enemyRegion is null)) regionBorder.SetEnemyRegion(enemyRegion);
                border.SetActive(true);
            }
        }

        private void DisableAllRegionBorders()
        {
            foreach (var border in _borders)
            {
                border.SetActive(false);
            }
        }
    }
}