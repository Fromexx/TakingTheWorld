using System;
using Assets;
using UnityEngine;

namespace Country
{
    public class MainCountryBall : MonoBehaviour
    {
        private Player.Player _player;
        private int _currentIncreaseCount;

        private void Awake()
        {
            transform.parent.parent.TryGetComponent(out _player);
        }

        public void Init(Player.Player player) => _player = player;

        public void IncreaseScale()
        {
            if (_currentIncreaseCount == GeneralAsset.Instance.MaxFactorCount) return;
            transform.localScale *= GeneralAsset.Instance.MainCountryBallIncreaseFactor;
            _currentIncreaseCount += 1;
        }

        public void DecreaseScale()
        {
            if (_currentIncreaseCount == 0) return;
            transform.localScale /= GeneralAsset.Instance.MainCountryBallIncreaseFactor;
            _currentIncreaseCount -= 1;
        }
        
        private void OnMouseDown()
        {
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