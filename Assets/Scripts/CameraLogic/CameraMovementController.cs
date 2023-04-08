using Assets.Scripts.CameraLogic;
using UnityEngine;

public interface ICameraMovementController
{
    public Vector3 GetCameraPosition();
    public void UpdateCameraPosition();

    public void AttachCamera(ICamera camera);
    public void DetachCamera();
}
