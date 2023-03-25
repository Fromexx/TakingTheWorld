using UnityEngine;

namespace CameraLogic
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        public void Move(Vector3 movementDirection)
        {
            transform.Translate(movementDirection * (_speed * Time.deltaTime));
        }
    }
}