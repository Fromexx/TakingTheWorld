using Assets.Scripts.CameraLogic;
using Assets.Scripts.CameraLogic.CameraSwipes;
using UnityEngine;

public class AndroidCameraSwipes : CameraSwipes, ICameraMovementController
{
    private Vector3 _currentCameraPosition;
    private Vector3 _cameraPositionBeforeSwipe;
    private Vector3 _startSwipeTouchPosition;
    private readonly float _sensitivity = 0.2f;
    private bool _isSwiping = false;
    private int _trackingFingerId;

    public AndroidCameraSwipes()
    {
        _cameraPositionBeforeSwipe = CameraConstants.INITIAL_CAMERA_POSITION;
        _currentCameraPosition = CameraConstants.INITIAL_CAMERA_POSITION;
        _cameraBorderCorrection = Vector3.zero;
    }
    public override Vector3 GetCameraPosition()
    {
        return _currentCameraPosition;
    }

    public override void UpdateCameraPosition()
    {
        
        if(Input.touchCount == 0)
        {
            _isSwiping = false;
            return;
        }

        foreach(Touch touch in Input.touches)
            if(Input.GetTouch(touch.fingerId).phase == TouchPhase.Began && !_isSwiping)
            {
                if (CanMakeSwipe() == false)
                    continue;
                Debug.Log($"Swipe started. Camera at {_currentCameraPosition}");
                _isSwiping = true;
                _trackingFingerId = touch.fingerId;
                _startSwipeTouchPosition = GetCurrentMousePosition();
                _cameraPositionBeforeSwipe = _currentCameraPosition;
                _cameraBorderCorrection = Vector3.zero;
                break;
            }

        if(Input.GetTouch(_trackingFingerId).phase == TouchPhase.Ended)
        {
            if (_isSwiping)
                _cameraPositionBeforeSwipe += GetSwipeDelta();
            _cameraBorderCorrection = Vector3.zero;
            _isSwiping = false;
        }
        if(_isSwiping)
        {
            _currentCameraPosition = _cameraPositionBeforeSwipe + GetSwipeDelta();
        }else
        {
            _currentCameraPosition = _cameraPositionBeforeSwipe;
        }

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
        var objectUnderMouse = _camera.MakeRaycastToMousePosition(Input.GetTouch(0).position);
        Debug.Log(objectUnderMouse);
        return objectUnderMouse == null || objectUnderMouse.GetComponent<NotSwipeableAttribute>() == null;
    }

    private Vector3 GetSwipeDelta() =>
            (-1) * (GetCurrentMousePosition() - _startSwipeTouchPosition) * _sensitivity + _cameraBorderCorrection;

    private Vector3 GetCurrentMousePosition()
    {
        var touchPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        touchPosition.z = touchPosition.y;
        touchPosition.y = _currentCameraPosition.y;

        return touchPosition;
    }
}
