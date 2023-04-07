using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CameraLogic
{
    public class PcCameraSwipes : ICameraMovementController
    {
        private Vector3 _currentCameraPosition;
        private Vector3 _cameraPositionBeforeSwipe;
        private Vector3 _startSwipeMousePosition;
        private float _sensitivity = 0.1f;

        public PcCameraSwipes()
        {
            _cameraPositionBeforeSwipe = CameraConstants.INITIAL_CAMERA_POSITION;
        }
        public Vector3 GetCameraPosition()
        {
            return _currentCameraPosition;
        }

        public void UpdateCameraPosition()
        {
                if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                _startSwipeMousePosition = GetCurrentMousePosition();
                _cameraPositionBeforeSwipe = _currentCameraPosition;
            }

            if (Input.GetMouseButtonUp((int)MouseButton.Left))
                _cameraPositionBeforeSwipe = _cameraPositionBeforeSwipe + GetSwipeDelta();

            if (Input.GetMouseButton((int)MouseButton.Left))
                _currentCameraPosition = _cameraPositionBeforeSwipe + GetSwipeDelta();
            else
                _currentCameraPosition = _cameraPositionBeforeSwipe;
        }

        private Vector3 GetSwipeDelta() => (-1) *(GetCurrentMousePosition() - _startSwipeMousePosition) * _sensitivity;

        private Vector3 GetCurrentMousePosition()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = mousePosition.y;
            mousePosition.y = _currentCameraPosition.y;

            return mousePosition;
        }
    }
}
