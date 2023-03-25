using Assets;
using Country;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private PlayerAttack _playerAttack;
        private Region _ownRegion;
        private Region _enemyRegion;

        private void Awake()
        {
            TryGetComponent(out _playerAttack);
        }
        
        public void Attack()
        {
            if (!GeneralAsset.Instance.AttackStarted) return;
            
            _playerAttack.Attack(_ownRegion, _enemyRegion);
            
            _ownRegion = null;
            _enemyRegion = null;
        }

        public void SetOwnRegionForAttack(Region ownRegion) => _ownRegion = ownRegion;

        public void SetEnemyRegionForAttack(Region enemyRegion)
        {
            if (_ownRegion == enemyRegion) return;
            
            _enemyRegion = enemyRegion;
            Attack();
        }
    }
}