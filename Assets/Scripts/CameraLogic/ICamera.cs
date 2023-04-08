using UnityEngine;

namespace Assets.Scripts.CameraLogic
{
    public interface ICamera
    {
        public GameObject MakeRaycastToMousePosition(Vector3 mousePosition);
    }
}
