using Assets;
using Assets.Scripts.Country.Region;
using System;
using TMPro;
using UnityEngine;

namespace Country
{
    public class MainCountryBall : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _circle;

        private Player.Player _player;
        private Assets.Scripts.Country.Country _country;
        private int _currentCountryBallCount;
        private bool _isAttackRegion;

        private void Awake()
        {
            try
            {
                var countryTransform = transform.parent.parent;

                countryTransform.TryGetComponent(out _country);
            }
            catch (Exception)
            {
            }
        }

        public void Init(int currentCountryBallCount)
        {
            _currentCountryBallCount = currentCountryBallCount;
            UpdateText();
        }

        public void Init(Assets.Scripts.Country.Country country)
        {
            _country = country;
            _circle.SetActive(false);
            GeneralAsset.Instance.IsSelectPlayerRegion = false;
        }

        public void InitPlayer(Player.Player player) => _player = player;

        public void DisableCircle() => _circle.SetActive(false);

        public void DestroyMainCountryBall() => Destroy(gameObject);

        private void UpdateText() => _text.text = _currentCountryBallCount.ToString();

        private void OnEnemyRegionSets()
        {
            _circle.SetActive(false);
            _player.EnemyRegionSets -= OnEnemyRegionSets;
        }

        private void OnMouseDown()
        {
            var instance = GeneralAsset.Instance;

            if (instance.IsClickedAtRegionTune)
            {
                instance.IsClickedAtRegionTune = false;
                return;
            }

            if (instance.IsSettingsWindowOpen || _player is null || !_country.IsPlayerCountry || !instance.AttackStarted) return;

            instance.IsSelectPlayerRegion = true;
            _circle.SetActive(true);
            _isAttackRegion = true;

            transform.parent.TryGetComponent(out Region ownRegion);
            _player.SetOwnRegionForAttack(ownRegion);
        }

        private void OnMouseEnter()
        {
            if (!GeneralAsset.Instance.IsSelectPlayerRegion) return;
            _circle.SetActive(true);
            _player.EnemyRegionSets += OnEnemyRegionSets;
        }

        private void OnMouseExit()
        {
            if (_isAttackRegion) return;
            _circle.SetActive(false);
        }

        private void OnMouseUp()
        {
            var instance = GeneralAsset.Instance;

            if (instance.IsSettingsWindowOpen || _player is null || !_player.IsOwnRegionStillOwn() || !instance.AttackStarted) return;

            GeneralAsset.Instance.IsSelectPlayerRegion = false;
            _circle.SetActive(false);

            Ray ray = GeneralAsset.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Int32.MaxValue))
            {
                hit.collider.transform.parent.TryGetComponent(out Region enemyRegion);
                if (enemyRegion is null)
                {
                    _player.ResetOwnRegionForAttack();
                    return;
                }

                _isAttackRegion = false;
                _player.SetEnemyRegionForAttack(enemyRegion);
            }
        }

        private void OnDestroy()
        {
            try
            {
                _player.EnemyRegionSets -= OnEnemyRegionSets;
            }
            catch { }
        }
    }

}