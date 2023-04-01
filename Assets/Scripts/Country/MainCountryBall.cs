using System;
using Assets;
using TMPro;
using UnityEngine;

namespace Country
{
    public class MainCountryBall : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _circle;

        private Player.Player _player;
        private Country _country;
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

        public void Init(Country country)
        {
            _country = country;
            _circle.SetActive(false);
            GeneralAsset.Instance.IsSelectPlayerRegion = false;
        }

        public void InitPlayer(Player.Player player) => _player = player;

        private void UpdateText() => _text.text = _currentCountryBallCount.ToString();

        private void OnEnemyRegionSets()
        {
            _circle.SetActive(false);
            _player.EnemyRegionSets -= OnEnemyRegionSets;
        }

        private void OnMouseDown()
        {
            if (_player is null || !_country.IsPlayerCountry || !GeneralAsset.Instance.AttackStarted) return;

            GeneralAsset.Instance.IsSelectPlayerRegion = true;
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
            print("tojhkto");
            _circle.SetActive(false);
        }

        private void OnMouseUp()
        {
            if (_player is null || !_player.IsOwnRegionStillOwn() || !GeneralAsset.Instance.AttackStarted) return;

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