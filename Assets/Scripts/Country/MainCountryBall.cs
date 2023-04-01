using System;
using Assets;
using TMPro;
using UnityEngine;

namespace Country
{
    public class MainCountryBall : MonoBehaviour
    {
        private Player.Player _player;
        private Country _country;
        private int _currentCountryBallCount;
        private TMP_Text _text;

        private void Awake()
        {
            try
            {
                var countryTransform = transform.parent.parent;
                
                countryTransform.TryGetComponent(out _country);
                _text = GetComponentInChildren<TMP_Text>();
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

        public void Init(Country country) => _country = country;

        public void InitPlayer(Player.Player player) => _player = player;

        private void UpdateText() => _text.text = _currentCountryBallCount.ToString();
        
        private void OnMouseDown()
        {
            if (_player is null) return;
            if (!_country.IsPlayerCountry) return;
            
            transform.parent.TryGetComponent(out Region ownRegion);
            _player.SetOwnRegionForAttack(ownRegion);
        }
        
        private void OnMouseUp()
        {
            if (_player is null) return;
            if (!_player.IsOwnRegionStillOwn()) return;
            
            Ray ray = GeneralAsset.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Int32.MaxValue))
            {
                hit.collider.transform.parent.TryGetComponent(out Region enemyRegion);
                if (enemyRegion is null) return;
                _player.SetEnemyRegionForAttack(enemyRegion);
            }
        }
    }
}