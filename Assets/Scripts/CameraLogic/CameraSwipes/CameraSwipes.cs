using System;
using UnityEngine;

namespace Assets.Scripts.CameraLogic.CameraSwipes
{
    public abstract class CameraSwipes : ICameraMovementController
    {
        protected ICamera _camera;
        protected Vector3 _cameraBorderCorrection;
        public void AttachCamera(ICamera camera)
        {
            _camera = camera;
        }

        public void DetachCamera()
        {
            _camera = null;
        }

        public abstract Vector3 GetCameraPosition();

        public abstract void UpdateCameraPosition();

        protected void PutCameraInBounds(float minimumCoord, float maximumCoord, ref float borderCorrection, ref float cameraPosition)
        {
            if (minimumCoord > maximumCoord)
                throw new ArgumentException($"Минимальное положение камеры не может быть больше максимального\nМаксимальное: {maximumCoord}\nМинимальное: {minimumCoord}");

            if (cameraPosition < minimumCoord)
            {
                borderCorrection += minimumCoord - cameraPosition;
                cameraPosition = minimumCoord;
            }

            if (cameraPosition > maximumCoord)
            {
                borderCorrection += maximumCoord - cameraPosition;
                cameraPosition = maximumCoord;
            }
        }


    }
}
