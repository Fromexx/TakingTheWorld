using UnityEngine;

namespace Country
{
    public class SelectRegion : MonoBehaviour
    {
        [SerializeField] private GameObject _uiAttack;

        private Player.Player _player;
        private Region _region;

        private void Awake()
        {
            transform.parent.TryGetComponent(out Enemy.Enemy enemy);
            _player = enemy?.Player;
            TryGetComponent(out _region);
        }

        private void OnMouseDown()
        {
            _uiAttack.SetActive(true);
            _player.SelectRegionForAttack(_region);
        }

        private void OnMouseEnter()
        {
            
        }

        private void OnMouseExit()
        {
            
        }
    }
}