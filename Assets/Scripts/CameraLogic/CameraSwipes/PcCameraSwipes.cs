using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.CameraLogic
{
    public class PcCameraSwipes : CameraSwipes.CameraSwipes, ICameraMovementController
    {
        private Vector3 _currentCameraPosition;
        private Vector3 _cameraPositionBeforeSwipe;
        private Vector3 _startSwipeMousePosition;
        private readonly float _sensitivity = 0.271f;
        
        private bool _isSwiping = false;

        public PcCameraSwipes()
        {
            _cameraPositionBeforeSwipe = CameraConstants.INITIAL_CAMERA_POSITION;
            _cameraBorderCorrection = Vector3.zero;
        }
        public override Vector3 GetCameraPosition()
        {
            return _currentCameraPosition;
        }

        public override void UpdateCameraPosition()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                if (CanMakeSwipe() == false)
                    return;
                _isSwiping = true;
                _startSwipeMousePosition = GetCurrentMousePosition();
                _cameraPositionBeforeSwipe = _currentCameraPosition;
                _cameraBorderCorrection = Vector3.zero;
            }

            if (Input.GetMouseButtonUp((int)MouseButton.Left))
            {
                if (_isSwiping)
                    _cameraPositionBeforeSwipe += GetSwipeDelta();
                _cameraBorderCorrection = Vector3.zero;
                _isSwiping = false;
            }

            if (Input.GetMouseButton((int)MouseButton.Left) && _isSwiping)
                _currentCameraPosition = _cameraPositionBeforeSwipe + GetSwipeDelta();
            else
                _currentCameraPosition = _cameraPositionBeforeSwipe;

            PutCameraInBounds(
                CameraConstants.MINIMUM_VERTICAL_POSITION,
                CameraConstants.MAXIMUM_VERTICAL_POSITION,
                ref _cameraBorderCorrection.z,
                ref _currentCameraPosition.z
                );

            PutCameraInBounds(
                CameraConstants.MINIMUM_HORIZONTAL_POSITION,
                CameraConstants.MAXIMUM_HORIZONTAL_POSITION,
                ref _cameraBorderCorrection.x,
                ref _currentCameraPosition.x
                );
        }

        private bool CanMakeSwipe()
        {
            var objectUnderMouse = _camera.MakeRaycastToMousePosition(Input.mousePosition);
            Debug.Log(objectUnderMouse);
            return objectUnderMouse == null || objectUnderMouse.GetComponent<NotSwipeableAttribute>() == null;
        }

        private Vector3 GetSwipeDelta() =>
            (-1) * (GetCurrentMousePosition() - _startSwipeMousePosition) * _sensitivity + _cameraBorderCorrection;

        private Vector3 GetCurrentMousePosition()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = mousePosition.y;
            mousePosition.y = _currentCameraPosition.y;

            return mousePosition;
        }
    }
}
