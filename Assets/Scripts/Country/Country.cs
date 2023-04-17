using Country;
using Interfaces;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Country
{
    public class Country : MonoBehaviour, ISaveableCountry
    {
        public event Action RegionsSets;
        public event Action UnionRegionsSets;

        [field: SerializeField] public GameObject CountryBallPrefab { get; private set; }
        [field: SerializeField] public bool IsPlayerCountry;
        [field: SerializeField] public byte Id { get; private set; }

        [SerializeField] private List<Region.Region> _regions;
        [SerializeField] private List<byte> _ownRegionsId;
        [SerializeField] private GameObject _mainCountryBallPrefab;

        private Region.Region _ourRegionForAttack;
        private Region.Region _enemyRegionForAttack;
        private List<Region.Region> _unionRegions;
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

        public void ConvertToPlayerCountry()
        {
            var instance = GeneralAsset.Instance;
            
            IsPlayerCountry = true;
            instance.PlayerCountry = this;
            gameObject.AddComponent<PlayerAttack>();
            var player = gameObject.AddComponent<Player.Player>();

            TryGetComponent(out Enemy.Enemy enemy);
            Destroy(enemy);

            foreach (var mainCountryBall in instance.AllMainCountryBalls) mainCountryBall.InitPlayer(player);

            instance.SelectCountryUI.SetActive(false);
            instance.IsSelectedCountry = false;
            
            instance.SelectedCountryColor.EnableGameObject();
            _regions[0].TryGetComponent(out Renderer renderer);
            instance.SelectedCountryColor.SetSelectedCountryColor(renderer.material.color);
        }

        public void SelectRegionForAttack(string enemyRegionTag, List<RegionBorder> borders)
        {
            EnableRegionBordersForFindEnemyRegion(enemyRegionTag, borders);
        }

        public void SelectUnionRegions(List<RegionBorder> borders)
        {
            EnableRegionBorders(borders);
        }

        public void SetAttackRegions(Region.Region ourRegion, Region.Region enemyRegion)
        {
            if (_ourRegionForAttack != null || _enemyRegionForAttack != null) return;

            _ourRegionForAttack = ourRegion;
            _enemyRegionForAttack = enemyRegion;

            RegionsSets?.Invoke();
        }

        public void SetUnionRegions(List<Region.Region> regions)
        {
            _unionRegions = regions;
            UnionRegionsSets?.Invoke();
        }

        public void GetRegionsForAttack(out Region.Region ourRegion, out Region.Region enemyRegion)
        {
            ourRegion = _ourRegionForAttack;
            enemyRegion = _enemyRegionForAttack;

            _ourRegionForAttack = null;
            _enemyRegionForAttack = null;
        }

        public List<Region.Region> GetUnionRegions() => _unionRegions;

        public void AddRegion(Region.Region capturedRegion, Region.Region invaderRegion)
        {
            capturedRegion.TryGetComponent(out Renderer renderer);
            renderer.material = _material;
            capturedRegion.transform.parent.TryGetComponent(out _enemyCountry);
            capturedRegion.tag = tag;
            capturedRegion.transform.SetParent(transform);
            var regionTag = invaderRegion.tag;

            SetCountryBall(capturedRegion);
            
            _regions.Add(capturedRegion);
            _ownRegionsId.Add(capturedRegion.Id);

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
                capturedRegion.StopCoroutineAttack();
            }

            var isEnemyRegionRemained = GeneralAsset.Instance.RegionsForAttack.Any(regionForAttack => !regionForAttack.CompareTag(regionTag));

            if (isEnemyRegionRemained) return;

            AttackFinish(_enemyCountry, invaderRegion);
        }

        private void SetCountryBall(Region.Region capturedRegion)
        {
            var instance = GeneralAsset.Instance;
            
            var capturedRegionMainCountryBallTransform = capturedRegion.MainCountryBall.transform;
            var position = capturedRegionMainCountryBallTransform.localPosition;
            var rotation = capturedRegionMainCountryBallTransform.localRotation;
            var scale = capturedRegionMainCountryBallTransform.localScale;

            instance.AllMainCountryBalls.Remove(capturedRegion.MainCountryBall);

            capturedRegion.MainCountryBall.DestroyMainCountryBall();

            var mainCountryBall = Instantiate(_mainCountryBallPrefab).transform;
            mainCountryBall.SetParent(capturedRegion.transform);
            mainCountryBall.TryGetComponent(out MainCountryBall mainCountryBallComponent);
            capturedRegion.SetMainCountryBall(mainCountryBallComponent);
            
            instance.AllMainCountryBalls.Add(mainCountryBallComponent);

            mainCountryBall.localPosition = position;
            mainCountryBall.localRotation = rotation;
            mainCountryBall.localScale = scale;
        }

        private void AddRegion(Region.Region region, Country givingCountry)
        {
            region.TryGetComponent(out MeshRenderer renderer);
            renderer.material = _material;
            region.tag = tag;
            region.transform.SetParent(transform);

            _regions.Add(region);
            givingCountry.RemoveRegion(region);
            SetCountryBall(region);
        }

        public void RemoveRegion(Region.Region region)
        {
            _regions.Remove(region);
            _ownRegionsId.Remove(region.Id);
        }

        private void AttackFinish(Country enemyCountry, Region.Region invaderRegion)
        {
            var instance = GeneralAsset.Instance;

            if (enemyCountry._regions.Count == 0)
            {
                if (enemyCountry.IsPlayerCountry)
                {
                    instance.PlayerLostMenu.SetActive(true);
                }
                else
                {
                    instance.AllCountries.Remove(enemyCountry);
                    Destroy(enemyCountry.gameObject);
                }
            }
            
            instance.AttackStarted = false;
            
            TryGetComponent(out Enemy.Enemy enemy);
            enemy?.StopAttack();

            foreach (var country in instance.AllCountries) country.gameObject.SetActive(true);

            if (!IsPlayerCountry)
            {
                foreach (var region in instance.PlayerRegionsForAttack) region.Init();
            }
            else if (IsPlayerCountry)
            {
                float money = instance.EnemyRegionForAttackCount * 100 * invaderRegion.CurrentMoney;
                Economy.Economy.IncreaseMoney(money);

                foreach (var region in instance.EnemyRegionsForAttack) region.Init();
            }

            foreach (var region in instance.RegionsForAttack)
            {
                region.MainCountryBall.DisableCircle();
                region.StopAllRegionCoroutines();
                region.RecoverCountryBall();
            }

            EnableAllRegions();
            enemyCountry.EnableAllRegions();
        }

        public void DisableNotInvolvedRegions(List<Region.Region> regions)
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
            foreach (var country in transform.parent.GetComponentsInChildren<Country>())
            {
                var regionCount = country.transform.childCount;

                for (int regionIndex = 0; regionIndex < regionCount; regionIndex++)
                {
                    var regionTransform = country.transform.GetChild(regionIndex);
                    regionTransform.TryGetComponent(out Region.Region region);
                    if (_ownRegionsId.Contains(region.Id))
                    {
                        AddRegion(region, country);
                        regionCount--;
                        regionIndex--;
                    }
                }
            }
        }

        public void Import(ProgressCountry progressCountry)
        {
            _ownRegionsId = progressCountry.OwnRegionsId;
            if (Convert.ToBoolean(progressCountry.IsPlayerCountry)) ConvertToPlayerCountry();
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
