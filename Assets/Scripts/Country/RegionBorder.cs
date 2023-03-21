using UnityEngine;

namespace Country
{
    public class RegionBorder : MonoBehaviour
    {
        private string _enemyCountryTag = "";
        private Country _country;
        private Region _enemyRegion;

        private void Awake()
        {
            transform.parent.parent.TryGetComponent(out _country);
        }

        public void SetEnemyCountryTag(string tag)
        {
            _enemyCountryTag = tag;
        }

        public void SetEnemyRegion(Region enemyRegion) => _enemyRegion = enemyRegion;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_enemyCountryTag)) return;
            
            other.TryGetComponent(out Region enemyRegion);
            if (!(_enemyRegion is null) && enemyRegion != _enemyRegion) return;
            transform.parent.TryGetComponent(out Region ourRegion);
                
            _country.SetAttackRegions(ourRegion, enemyRegion);
            _enemyRegion = null;
        }
    }
}