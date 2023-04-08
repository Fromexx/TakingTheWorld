using Assets.Scripts.Country.Region;
using Country;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class GeneralAsset : MonoBehaviour
    {
        public static GeneralAsset Instance => _instance;

        private static GeneralAsset _instance;

        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public float TimeBetweenCountryBallSpawn { get; private set; }
        [field: SerializeField] public float TimeBetweenIncreaseCountryBall { get; private set; }
        [field: SerializeField] public RegionTuneView RegionTuneView { get; private set; }
        [field: SerializeField] public List<Assets.Scripts.Country.Country> AllCountries;
        [field: SerializeField] public float TimeBetweenAttack;
        [field: SerializeField] public List<MainCountryBall> AllMainCountryBalls { get; private set; }
        [field: SerializeField] public GameObject SelectCountryUI;
        [field: SerializeField] public SoldierVoicesPlayer SoldierVoicesPlayer;

        [HideInInspector] public List<Region> UnionRegions;
        [HideInInspector] public int IterationCount;
        [HideInInspector] public bool AttackStarted;
        [HideInInspector] public List<Collider> VerifedColliders;
        [HideInInspector] public List<Region> RegionsForAttack;
        [HideInInspector] public int EnemyRegionForAttackCount;
        [HideInInspector] public List<Region> PlayerRegionsForAttack;
        [HideInInspector] public List<Region> EnemyRegionsForAttack;
        [HideInInspector] public bool IsSelectedCountry = true;
        [HideInInspector] public Assets.Scripts.Country.Country PlayerCountry;
        [HideInInspector] public bool IsSelectPlayerRegion;
        [HideInInspector] public bool IsSettingsWindowOpen;
        [HideInInspector] public bool IsClickedAtRegionTune;

        private void Awake() => _instance = this;
    }
}