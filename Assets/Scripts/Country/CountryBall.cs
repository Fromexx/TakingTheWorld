﻿using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Country
{
    public class CountryBall : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Vector3 _target;
        private bool _atTargetPosition;
        private Region _enemyRegion;
        private Transform _ownCountry;
        private bool _isInit;
        private Region _ownRegion;

        private const string MainCountryBallTag = "MainCountryBall";

        private void Update()
        {
            if (_atTargetPosition)
            {
                _enemyRegion.ProtectRegion(_ownCountry, _ownRegion);
                Destroy(gameObject);
            }
            
            if(_isInit) Move();
        }
        
        public void Init(Vector3 target, Region enemyRegion, Transform ownCountry, Region ownRegion)
        {
            _isInit = true;
            _atTargetPosition = false;
            _target = target;
            _enemyRegion = enemyRegion;
            _ownCountry = ownCountry;
            _ownRegion = ownRegion;
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        }

        private void StopMove()
        {
            _atTargetPosition = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(MainCountryBallTag))
            {
                other.transform.parent.TryGetComponent(out Region region);
                if (region == _enemyRegion)
                {
                    StopMove();
                }
            }
        }
    }
}