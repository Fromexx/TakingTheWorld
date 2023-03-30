using System.Collections.Generic;
using UnityEngine;

namespace CameraLogic
{
    public class CameraLogic : MonoBehaviour
    {
        [SerializeField] private KeyboardInput _keyboardInput;
        [SerializeField] private GameObject _cube;
        
        private CameraMovement _cameraMovement;

        private void Awake()
        {
            TryGetComponent(out _cameraMovement);
        }

        private void Update()
        {
            CameraMovement();
        }
        
        public void TranslateCameraToCenterOfObjects(List<Transform> objects)
        {
            print("plh");
            
            foreach (var obj in objects)
            {
                print(obj);
            }
            
            Instantiate(_cube, GetObjectsCenter(objects), Quaternion.identity);
        }

        private Vector3 GetObjectsCenter(List<Transform> objects)
        {
            var bounds = new Bounds(objects[0].position, Vector3.zero);

            for (int i = 0; i < objects.Count; i++)
            {
                bounds.Encapsulate(objects[i].position);
            }

            return bounds.center;
        }

        private void CameraMovement()
        {
            _cameraMovement.Move(_keyboardInput.MovementInputVector);
        }
    }
}