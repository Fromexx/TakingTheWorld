using System.Collections.Generic;
using UnityEngine;

namespace Country
{
    public class RegionBorder : MonoBehaviour
    {
        private string _enemyCountryTag = "";
        private Country _country;
        private Region _enemyRegion;
        private bool _isFindUnionRegions;
        private Region _ourRegion;
        private List<RegionBorder> _borders;
        private List<Region> _unionRegions;
        private bool _isFindEnemyRegion;

        private void Awake()
        {
            transform.parent.parent.TryGetComponent(out _country);
            _unionRegions = new List<Region>();
        }

        public void Init(string enemyCountryTag, Region enemyRegion)
        {
            _enemyCountryTag = enemyCountryTag;
            _enemyRegion = enemyRegion;

            _isFindEnemyRegion = false;
            _isFindUnionRegions = false;
        }

        public void InitWithOurRegion(string enemyCountryTag, Region ourRegion)
        {
            _isFindEnemyRegion = true;
            _enemyCountryTag = enemyCountryTag;
            _ourRegion = ourRegion;
            
            _isFindEnemyRegion = true;
            _isFindUnionRegions = false;
        }

        public void InitWithRegion(Region region, List<RegionBorder> borders)
        {
            _ourRegion = region;
            _borders = borders;
            
            _isFindEnemyRegion = false;
            _isFindUnionRegions = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            SelectRegions(other);
            SelectEnemyRegion(other);
            SelectUnionRegion(other);
        }

        private void SelectRegions(Collider other)
        {
            if (_isFindUnionRegions || _isFindEnemyRegion || !other.CompareTag(_enemyCountryTag)) return;
            
            other.TryGetComponent(out Region enemyRegion);
            if (!(_enemyRegion is null) && enemyRegion != _enemyRegion) return;
            transform.parent.TryGetComponent(out Region ourRegion);
                
            _country.SetAttackRegions(ourRegion, enemyRegion);
        }

        private void SelectEnemyRegion(Collider other)
        {
            if (!_isFindEnemyRegion || !other.CompareTag(_enemyCountryTag)) return;
            
            other.TryGetComponent(out Region enemyRegion);

            _country.SetAttackRegions(_ourRegion, enemyRegion);
        }

        private void SelectUnionRegion(Collider other)
        {
            if (!_isFindUnionRegions || !other.CompareTag(_ourRegion.tag)) return;

            other.TryGetComponent(out Region region);
            _unionRegions.Add(region);

            if (_unionRegions.Count == _borders.Count) _country.SetUnionRegions(_unionRegions, _borders);
        }
    }
}