using Assets;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        private Country.Country _countryForAttack;
        
        public void StartAttack()
        {
            foreach (var region in GeneralAsset.Instance.EnemyRegionsForAttack)
            {
                region.StartCoroutineAttack();
            }
        }

        public void StopAttack()
        {
            foreach (var region in GeneralAsset.Instance.EnemyRegionsForAttack)
            {
                region.StopCoroutineAttack();
            }
        }
    }
}