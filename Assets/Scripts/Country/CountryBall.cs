using Assets;
using Assets.Scripts.Country.Region;
using UnityEngine;
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
        private int _countryBallCount;
        private int _countryBallNumber;

        private const string MainCountryBallTag = "MainCountryBall";

        private void Update()
        {
            if (_atTargetPosition)
            {
                if (!GeneralAsset.Instance.AttackStarted)
                {
                    Destroy(gameObject);
                    return;
                }
                _enemyRegion.ProtectRegion(_ownCountry, _ownRegion, _countryBallNumber == _countryBallCount);
                Destroy(gameObject);
            }

            if (_isInit) Move();
        }

        public void Init(Vector3 target, Region enemyRegion, Transform ownCountry, Region ownRegion, int countryBallCount, int countryBallNumber)
        {
            _isInit = true;
            _atTargetPosition = false;
            _target = target;
            _enemyRegion = enemyRegion;
            _ownCountry = ownCountry;
            _ownRegion = ownRegion;
            _countryBallCount = countryBallCount;
            _countryBallNumber = countryBallNumber;
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(MainCountryBallTag))
            {
                other.transform.parent.TryGetComponent(out Region region);
                
                if (region == _enemyRegion)
                {
                    _atTargetPosition = true;
                }
            }
        }
    }
}