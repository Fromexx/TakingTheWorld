using System;
using System.Collections.Generic;
using Assets;
using UnityEngine;

namespace Country
{
    public class RegionBorder : MonoBehaviour
    {
        public event Action<List<RegionBorder>> NotFoundUnionRegions;
        
        private string _enemyCountryTag = "";
        private Country _country;
        private Region _enemyRegion;
        private bool _isFindUnionRegions;
        private Region _ourRegion;
        private List<RegionBorder> _borders;
        private bool _isFindEnemyRegion;

        private void Awake()
        {
            transform.parent.parent.TryGetComponent(out _country);
            GeneralAsset.Instance.UnionRegions = new List<Region>();
        }

        public void Init(string enemyCountryTag, Region enemyRegion, List<RegionBorder> borders)
        {
            _enemyCountryTag = enemyCountryTag;
            _enemyRegion = enemyRegion;
            _borders = borders;

            GeneralAsset.Instance.IterationCount = 0;
            _isFindEnemyRegion = false;
            _isFindUnionRegions = false;
        }

        public void InitWithOurRegion(string enemyCountryTag, Region ourRegion, List<RegionBorder> borders)
        {
            _isFindEnemyRegion = true;
            _enemyCountryTag = enemyCountryTag;
            _ourRegion = ourRegion;
            _borders = borders;
            
            GeneralAsset.Instance.IterationCount = 0;
            _isFindEnemyRegion = true;
            _isFindUnionRegions = false;
        }

        public void InitWithRegion(Region region, List<RegionBorder> borders)
        {
            _ourRegion = region;
            _borders = borders;
            
            GeneralAsset.Instance.IterationCount = 0;
            _isFindEnemyRegion = false;
            _isFindUnionRegions = true;
            GeneralAsset.Instance.VerifedColliders = new List<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            SelectRegions(other);
            SelectEnemyRegion(other);
            SelectUnionRegion(other);
        }

        private void SelectRegions(Collider other)
        {
            if (!_isFindEnemyRegion || !_isFindUnionRegions || !other.TryGetComponent(out Region enemyRegion)) return;

            GeneralAsset.Instance.IterationCount++;
            
            if (!other.CompareTag(_enemyCountryTag))
            {
                if(IterationCountEqualsBorderCount()) DisableBorders();
                return;
            }
            
            if (!(_enemyRegion is null) && enemyRegion != _enemyRegion) return;
            transform.parent.TryGetComponent(out Region ourRegion);
                
            _country.SetAttackRegions(ourRegion, enemyRegion);
            
            if (!IterationCountEqualsBorderCount()) return;
            DisableBorders();
        }

        private void SelectEnemyRegion(Collider other)
        {
            if (!_isFindEnemyRegion || !other.TryGetComponent(out Region enemyRegion)) return;

            GeneralAsset.Instance.IterationCount++;
            
            if (!other.CompareTag(_enemyCountryTag))
            {
                if(IterationCountEqualsBorderCount()) DisableBorders();
                return;
            }

            _country.SetAttackRegions(_ourRegion, enemyRegion);
            DisableBorders();
        }

        private void SelectUnionRegion(Collider other)
        {
            if (!_isFindUnionRegions || !other.TryGetComponent(out Region region) || GeneralAsset.Instance.VerifedColliders.Contains(other)) return;

            GeneralAsset.Instance.IterationCount++;
            GeneralAsset.Instance.VerifedColliders.Add(other);

            if (!other.CompareTag(_ourRegion.tag))
            {
                if (IterationCountEqualsBorderCount())
                {
                    DisableBorders();
                    
                    if(GeneralAsset.Instance.UnionRegions.Count == 0) NotFoundUnionRegions?.Invoke(_borders);
                    
                    _country.SetUnionRegions(GeneralAsset.Instance.UnionRegions, _borders);
                }
                return;
            }

            GeneralAsset.Instance.UnionRegions.Add(region);
            
            if (!IterationCountEqualsBorderCount() && GeneralAsset.Instance.UnionRegions.Count != _borders.Count) return;
            
            DisableBorders();
            _country.SetUnionRegions(GeneralAsset.Instance.UnionRegions, _borders);
        }

        private bool IterationCountEqualsBorderCount() => GeneralAsset.Instance.IterationCount == _borders.Count;

        private void DisableBorders()
        {
            foreach (var border in _borders) border.gameObject.SetActive(false);
        }
    }
}