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

        private void Update()
        {
            if (_atTargetPosition)
            {
                _enemyRegion.ProtectRegion(_ownCountry);
                Destroy(gameObject);
            }
            
            if(_isInit && !_isMainCountryBall) Move();
        }

        private void Move()
        {
            if (Round(transform.position) == Round(_target))
            {
                _atTargetPosition = true;
                _target = Vector3.zero;
                return;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        }
        
        public void Init(Vector3 target, Region enemyRegion, Country ownCountry)
        {
            _isInit = true;
            _atTargetPosition = false;
            _target = target;
            _enemyRegion = enemyRegion;
            _ownCountry = ownCountry;
        }
        
        private Vector3 Round(Vector3 vector3, int decimalPlaces = 0)
        {
            float multiplier = 1;
            
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            
            return new Vector3(
                Mathf.Round(vector3.x * multiplier) / multiplier,
                Mathf.Round(vector3.y * multiplier) / multiplier,
                Mathf.Round(vector3.z * multiplier) / multiplier);
        }
    }
}