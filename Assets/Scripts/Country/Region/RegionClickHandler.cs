using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Country.Region
{
    [RequireComponent(typeof(Region))]
    internal class RegionClickHandler : MonoBehaviour
    {
        private Region _region;
        private readonly float DEAD_ZONE = 20f;
        private Vector3 _startMousePosition;
        private bool _hasMouseExitedCollider = false;
        private bool _isClickStarted = false;
        private void Awake()
        {
            _region = GetComponent<Region>();
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
                return;

            _startMousePosition = Input.mousePosition;
            _hasMouseExitedCollider = false;
            _isClickStarted = true;
        }

        private void OnMouseExit()
        {
            _hasMouseExitedCollider = true;
        }
        private void OnMouseUp()
        {
            if (!_isClickStarted)
                return;
            if (_hasMouseExitedCollider)
                return;

            var delta = Input.mousePosition - _startMousePosition;
            if (delta.magnitude < DEAD_ZONE)
                _region.OnMouseClick();

            _isClickStarted = false;
        }
    }
}
