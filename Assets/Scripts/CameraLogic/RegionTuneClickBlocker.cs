using System.Collections.Generic;
using Country;
using UnityEngine;

namespace CameraLogic
{
    public class RegionTuneClickBlocker : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Transform _camera;
        [SerializeField] private RegionTuneView _regionTuneView;
        [SerializeField] private List<GameObject> _blockers;

        private void Awake()
        {
            _regionTuneView.RegionTuneEnabling += EnableBlockers;
            _regionTuneView.RegionTuneDisabling += DisableBlockers;
        }

        private void OnDestroy()
        {
            _regionTuneView.RegionTuneEnabling -= EnableBlockers;
            _regionTuneView.RegionTuneDisabling -= DisableBlockers;
        }

        private void Update() => transform.position = new Vector3(_camera.position.x + _offset.x, transform.position.y, _camera.position.z + _offset.y);

        private void EnableBlockers()
        {
            foreach (var blocker in _blockers)
            {
                blocker.SetActive(true);
            }
        }

        private void DisableBlockers()
        {
            foreach (var blocker in _blockers)
            {
                blocker.SetActive(false);
            }
        }
    }
}