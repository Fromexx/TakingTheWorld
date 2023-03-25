using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }

    private void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        
        MovementInputVector = new Vector2(horizontal, vertical);
        MovementInputVector.Normalize();
    }
}