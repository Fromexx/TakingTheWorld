using UnityEngine;

namespace CameraLogic
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float _maxFov;
        [SerializeField] private float _minFov;
        [SerializeField] private float _defaultFov = 60;
        [SerializeField] private float _zoomDuration = 2;

        private Camera _camera;
        private float _zoomMultiplier;

        private void Awake()
        {
            TryGetComponent(out _camera);
            _zoomMultiplier = _defaultFov / _minFov;

        }

        void Update()
        {
            if (Input.GetKey(KeyCode.O))
            {
                ZoomCamera(_minFov);
            }
            else if (Input.GetKey(KeyCode.L))
            {
                ZoomCamera(_maxFov);
            }
        }

        void ZoomCamera(float target)
        {
            float angle = Mathf.Abs((_defaultFov / _zoomMultiplier) - _defaultFov);
            _camera.fieldOfView = Mathf.MoveTowards(_camera.fieldOfView, target, angle / _zoomDuration * Time.deltaTime);
        }
    }
}