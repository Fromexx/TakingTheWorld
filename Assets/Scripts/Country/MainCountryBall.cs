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
        private Region _region;
        private int _currentCountryBallCount;
        private TMP_Text _text;

        private void Awake()
        {
            try
            {
                var regionTransform = transform.parent;
                var countryTransform = regionTransform.parent;

                countryTransform.TryGetComponent(out _player);
                countryTransform.TryGetComponent(out _country);
                regionTransform.TryGetComponent(out _region);
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

        private void UpdateText() => _text.text = _currentCountryBallCount.ToString();
        
        private void OnMouseDown()
        {
            if (_player is null) return;
            if (!_country.IsPlayerCountry) return;
            
            transform.parent.TryGetComponent(out Region ownRegion);
            _player?.SetOwnRegionForAttack(ownRegion);
        }
        
        private void OnMouseUp()
        {
            Ray ray = GeneralAsset.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Int32.MaxValue))
            {
                hit.collider.transform.parent.TryGetComponent(out Region enemyRegion);
                if (enemyRegion is null) return;
                _player?.SetEnemyRegionForAttack(enemyRegion);
            }
        }
    }
}