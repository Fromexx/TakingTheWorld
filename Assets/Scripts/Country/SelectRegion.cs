using System;
using Singleton;
using UnityEngine;

namespace Country
{
    public class SelectRegion : MonoBehaviour
    {
        private Player.Player _player;
        private Region _region;

        private void Awake()
        {
            transform.parent.TryGetComponent(out _player);

            if (_player is null)
            {
                transform.parent.TryGetComponent(out Enemy.Enemy enemy);
                _player = enemy.Player;
            }
            
            TryGetComponent(out _region);
        }

        private void OnMouseDown()
        {
            _player.SetOwnRegionForAttack(_region);
        }

        private void OnMouseUp()
        {
            Ray ray = GeneralAsset.Instance.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Int32.MaxValue))
            {
                hit.collider.TryGetComponent(out Region region);
                _player.SetEnemyRegionForAttack(region);
            }
        }
    }
}