using UnityEngine;

namespace Assets.Scripts.CameraLogic
{
    public class CameraMovementFactory
    {
        public static ICameraMovementController Create() =>
            Application.platform switch
            {
                RuntimePlatform.Android => new AndroidCameraSwipes(),
                RuntimePlatform.WindowsEditor => new PcCameraSwipes(),
                _ => new PcCameraSwipes()
            };
        
    }
}
