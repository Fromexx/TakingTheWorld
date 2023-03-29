using System.Collections.Generic;
using Country;
using UnityEngine;

namespace Assets
{
    public class GeneralAsset : MonoBehaviour
    {
        public static GeneralAsset Instance => _instance;

        private static GeneralAsset _instance;

        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public float MainCountryBallIncreaseFactor { get; private set; }
        [field: SerializeField] public int MaxFactorCount { get; private set; }
        [field: SerializeField] public float TimeBetweenCountryBallSpawn { get; private set; }
        [field: SerializeField] public float TimeBetweenIncreaseCountryBall { get; private set; }
        [field: SerializeField] public Country.Country PlayerCountry { get; private set; }
        [field: SerializeField] public RegionTuneView RegionTuneView { get; private set; }
        [field: SerializeField] public List<Country.Country> AllCountries;

        [HideInInspector] public List<Region> UnionRegions;
        [HideInInspector] public int IterationCount;
        [HideInInspector] public bool AttackStarted;
        [HideInInspector] public List<Collider> VerifedColliders;
        [HideInInspector] public List<Region> RegionsForAttack;
        [HideInInspector] public int EnemyRegionForAttackCount;
        [HideInInspector] public List<Region> EnemyRegionsForAttack;

        private void Awake() => _instance ??= this;
    }
}