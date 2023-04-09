using UnityEngine;

namespace Assets.Scripts.CameraLogic
{
    public static class CameraConstants
    {
        public static readonly Vector3 INITIAL_CAMERA_POSITION = new Vector3(883.1f, 1412.5f, -92.4f);

        public static readonly float MAXIMUM_HORIZONTAL_POSITION = 1150;
        public static readonly float MINIMUM_HORIZONTAL_POSITION = 330;

        public static readonly float MAXIMUM_VERTICAL_POSITION = -60;
        public static readonly float MINIMUM_VERTICAL_POSITION = -400;
    }
}
