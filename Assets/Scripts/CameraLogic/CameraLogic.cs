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

        private void CameraMovement()
        {
            _cameraMovement.Move(_keyboardInput.MovementInputVector);
        }
    }
}