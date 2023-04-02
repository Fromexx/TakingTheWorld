using Assets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Country
{
    public class RegionBorder : MonoBehaviour
    {
        public event Action NotFoundUnionRegions;

        private string _enemyCountryTag = "";
        private Country _country;
        private Region _enemyRegion;
        private bool _isFindUnionRegions;
        private Region _ourRegion;
        private List<RegionBorder> _borders;
        private bool _isFindEnemyRegion;
        private List<Region> _regions;

        private void Awake()
        {
            var parent = transform.parent;
            parent.parent.TryGetComponent(out _country);
            GeneralAsset.Instance.UnionRegions = new List<Region>();
            parent.TryGetComponent(out _ourRegion);
        }

        public void Init(string enemyCountryTag, Region enemyRegion, List<RegionBorder> borders, List<Region> regions)
        {
            _enemyCountryTag = enemyCountryTag;
            _enemyRegion = enemyRegion;
            _borders = borders;
            _regions = regions;

            GeneralAsset.Instance.IterationCount = 0;
            _isFindEnemyRegion = false;
            _isFindUnionRegions = false;

            gameObject.SetActive(true);
        }

        public void InitWithOurRegion(string enemyCountryTag, List<RegionBorder> borders, List<Region> regions)
        {
            _isFindEnemyRegion = true;
            _enemyCountryTag = enemyCountryTag;
            _borders = borders;
            _regions = regions;

            GeneralAsset.Instance.IterationCount = 0;
            _isFindEnemyRegion = true;
            _isFindUnionRegions = false;

            gameObject.SetActive(true);
        }

        public void InitWithRegion(List<RegionBorder> borders, List<Region> regions)
        {
            _borders = borders;
            _regions = regions;

            GeneralAsset.Instance.IterationCount = 0;
            _isFindEnemyRegion = false;
            _isFindUnionRegions = true;
            GeneralAsset.Instance.VerifedColliders = new List<Collider>();

            gameObject.SetActive(true);
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
                if (IterationCountEqualsBordersCount()) DisableBorders();
                return;
            }

            if (!(_enemyRegion is null) && enemyRegion != _enemyRegion) return;
            transform.parent.TryGetComponent(out Region ourRegion);

            _country.SetAttackRegions(ourRegion, enemyRegion);

            if (!IterationCountEqualsBordersCount()) return;
            DisableBorders();
        }

        private void SelectEnemyRegion(Collider other)
        {
            if (!_isFindEnemyRegion || !other.TryGetComponent(out Region enemyRegion)) return;

            GeneralAsset.Instance.IterationCount++;

            if (!other.CompareTag(_enemyCountryTag))
            {
                if (IterationCountEqualsBordersCount()) DisableBorders();
                return;
            }

            DisableBorders();
            _country.SetAttackRegions(_ourRegion, enemyRegion);
        }

        private void SelectUnionRegion(Collider other)
        {
            if (!_isFindUnionRegions || !other.TryGetComponent(out Region region) || GeneralAsset.Instance.VerifedColliders.Contains(other)) return;

            GeneralAsset.Instance.IterationCount++;
            GeneralAsset.Instance.VerifedColliders.Add(other);

            if (!other.CompareTag(_ourRegion.tag))
            {
                if (IterationCountEqualsBordersCount())
                {
                    DisableBorders();

                    if (GeneralAsset.Instance.UnionRegions.Count == 0)
                    {
                        NotFoundUnionRegions?.Invoke();
                        return;
                    }

                    _country.SetUnionRegions(GeneralAsset.Instance.UnionRegions);
                }
                return;
            }

            GeneralAsset.Instance.UnionRegions.Add(region);

            if (!IterationCountEqualsBordersCount() && GeneralAsset.Instance.UnionRegions.Count != _regions.Count) return;

            DisableBorders();
            _country.SetUnionRegions(GeneralAsset.Instance.UnionRegions);
        }

        private bool IterationCountEqualsRegionsCount() => GeneralAsset.Instance.IterationCount == _regions.Count - 1;
        private bool IterationCountEqualsBordersCount() => GeneralAsset.Instance.IterationCount == _borders.Count;


        private void DisableBorders()
        {
            foreach (var border in _borders) border.gameObject.SetActive(false);
        }
    }
}