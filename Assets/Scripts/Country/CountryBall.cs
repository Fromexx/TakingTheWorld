using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Country
{
    public class CountryBall : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _isMainCountryBall;

        private Vector3 _target;
        private bool _atTargetPosition;
        private Region _enemyRegion;
        private Country _ownCountry;
        private bool _isInit;

        private const string MainCountryBallTag = "MainCountryBall";

        private void Update()
        {
            if (_atTargetPosition)
            {
                _enemyRegion.ProtectRegion(_ownCountry);
                Destroy(gameObject);
            }
            
            if(_isInit && !_isMainCountryBall) Move();
        }
        
        public void Init(Vector3 target, Region enemyRegion, Country ownCountry)
        {
            _isInit = true;
            _atTargetPosition = false;
            _target = target;
            _enemyRegion = enemyRegion;
            _ownCountry = ownCountry;
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
                print(region);
                if (region == _enemyRegion)
                {
                    StopMove();
                }
            }
        }
    }
}