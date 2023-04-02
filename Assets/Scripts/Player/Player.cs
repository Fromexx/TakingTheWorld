using Assets;
using Country;
using System;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public event Action EnemyRegionSets;

        private PlayerAttack _playerAttack;
        private Region _ownRegion;
        private Region _enemyRegion;

        private void Awake()
        {
            TryGetComponent(out _playerAttack);
        }

        public void SetOwnRegionForAttack(Region ownRegion) => _ownRegion = ownRegion;

        public void ResetOwnRegionForAttack() => _ownRegion = null;

        public bool IsOwnRegionStillOwn()
        {
            if (_ownRegion is null) return false;

            _ownRegion.transform.parent.TryGetComponent(out Country.Country country);
            return country.IsPlayerCountry;
        }

        public void SetEnemyRegionForAttack(Region enemyRegion)
        {
            if (_ownRegion == enemyRegion || _ownRegion is null) return;

            _enemyRegion = enemyRegion;
            EnemyRegionSets?.Invoke();
            Attack();
        }

        private void Attack()
        {
            if (!GeneralAsset.Instance.AttackStarted) return;

            _playerAttack.Attack(_ownRegion, _enemyRegion);

            _ownRegion = null;
            _enemyRegion = null;
        }
    }
}