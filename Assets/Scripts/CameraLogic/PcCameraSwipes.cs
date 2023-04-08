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
        private readonly float _sensitivity = 0.271f;
        private Vector3 _cameraBorderCorrection;
        private bool _isSwiping = false;

        private ICamera _camera;

        public void AttachCamera(ICamera camera)
        {
            _camera = camera;
        }

        public void DetachCamera()
        {
            _camera = null;
        }

        public PcCameraSwipes()
        {
            _cameraPositionBeforeSwipe = CameraConstants.INITIAL_CAMERA_POSITION;
            _cameraBorderCorrection = Vector3.zero;
        }
        public Vector3 GetCameraPosition()
        {
            return _currentCameraPosition;
        }

        public void UpdateCameraPosition()
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
            var objectUnderMouse = _camera.MakeRaycastToMousePosition();
            Debug.Log(objectUnderMouse);
            return objectUnderMouse == null || objectUnderMouse.GetComponent<NotSwipeableAttribute>() == null;
        }
        private void PutCameraInBounds(float minimumCoord, float maximumCoord, ref float borderCorrection, ref float cameraPosition)
        {
            if (minimumCoord > maximumCoord)
                throw new ArgumentException($"Минимальное положение камеры не может быть больше максимального\nМаксимальное: {maximumCoord}\nМинимальное: {minimumCoord}");

            if(cameraPosition < minimumCoord)
            {
                borderCorrection += minimumCoord - cameraPosition;
                cameraPosition = minimumCoord;
            }

            if(cameraPosition > maximumCoord)
            {
                borderCorrection += maximumCoord - cameraPosition;
                cameraPosition = maximumCoord;
            }
        }

        private Vector3 GetSwipeDelta() => 
            (-1) *(GetCurrentMousePosition() - _startSwipeMousePosition) * _sensitivity + _cameraBorderCorrection;

        private Vector3 GetCurrentMousePosition()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = mousePosition.y;
            mousePosition.y = _currentCameraPosition.y;

            return mousePosition;
        }
    }
}
