using UnityEngine;

namespace CameraLogic
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _maxAxesValues;
        [SerializeField] private Vector2 _minAxesValues;

        public void Move(Vector3 movementDirection)
        {
            var cameraPosition = transform.position;

            if (cameraPosition.x >= _maxAxesValues.x && movementDirection.x > 0 
                || cameraPosition.x <= _minAxesValues.x && movementDirection.x < 0) movementDirection.x = 0;

            if (cameraPosition.z >= _maxAxesValues.y && movementDirection.y > 0 
                || cameraPosition.z <= _minAxesValues.y && movementDirection.y < 0) movementDirection.y = 0;

            transform.Translate(movementDirection * (_speed * Time.deltaTime));
        }
    }
}